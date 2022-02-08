namespace Internify.Web.Areas.Identity.Pages.Account
{
    using Data.Models;
    using Infrastructure;
    using Internify.Data;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.ComponentModel.DataAnnotations;

    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly InternifyDbContext data;
        private readonly RoleChecker roleChecker;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            InternifyDbContext data,
            RoleChecker roleChecker)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.data = data;
            this.roleChecker = roleChecker;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = "/")
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = "/";
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = "/")
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, lockoutOnFailure: false);
                
                if (result.Succeeded)
                {
                    // User is not authenticated yet.
                    // This means that Role checking with User property won't work. It has to be done manually;

                    var user = await userManager.FindByEmailAsync(Input.Email);

                    // Only for Admin since we have separate tables for other roles
                    var isAdmin = data.UserRoles.Any(x => x.UserId == user.Id);

                    // For other roles...
                    var isInAnyOtherRole = roleChecker.IsUserInAnyRole(user.Id);

                    if (!isAdmin && !isInAnyOtherRole)
                    {
                        return RedirectToAction("SelectRole", "Home");
                    }

                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            //var errors = ModelState.Select(x => x.Value.Errors)
            //               .Where(y => y.Count > 0)
            //               .ToList();

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
