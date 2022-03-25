namespace Internify.Web.Areas.Identity.Pages.Account
{
    using Internify.Data;
    using Internify.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.ComponentModel.DataAnnotations;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly InternifyDbContext data;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            InternifyDbContext data)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.data = data;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public void OnGetAsync(string returnUrl = "/")
        {
            ReturnUrl = "/";
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = "/")
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Email = Input.Email,
                    UserName = Input.Email
                };

                var foundUser = await userManager.FindByEmailAsync(Input.Email);
                var hasBeenPreviouslyDeleted = foundUser?.IsDeleted == true;

                if (hasBeenPreviouslyDeleted)
                {
                    object record = null;

                    // Check candidates
                    record = data.Candidates.FirstOrDefault(x => x.UserId == foundUser.Id);

                    if (record != null)
                    {
                        data.Candidates.Remove((Candidate)record);
                    }
                    else
                    {
                        // Check universities
                        record = data.Universities.FirstOrDefault(x => x.UserId == foundUser.Id);

                        if (record != null)
                        {
                            data.Universities.Remove((University)record);
                        }
                        else
                        {
                            // Check companies
                            record = data.Companies.FirstOrDefault(x => x.UserId == foundUser.Id);

                            if (record != null)
                            {
                                data.Companies.Remove((Company)record);
                            }
                        }
                    }

                    await userManager.DeleteAsync(foundUser);
                }

                var result = await userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("SelectRole", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
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