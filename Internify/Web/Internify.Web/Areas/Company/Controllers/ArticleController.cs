namespace Internify.Web.Areas.Company.Controllers
{
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.Article;
    using Microsoft.AspNetCore.Mvc;
    using Services.Article;
    using Services.Company;

    public class ArticleController : CompanyControllerBase
    {
        private readonly IArticleService articleService;
        private readonly ICompanyService companyService;

        public ArticleController(
            IArticleService articleService,
            ICompanyService companyService)
        {
            this.articleService = articleService;
            this.companyService = companyService;
        }

        public IActionResult Add()
        {
            var articleModel = new AddArticleFormModel
            {
                CompanyId = companyService.GetIdByUserId(User.Id())
            };

            return View(articleModel);
        }

        [HttpPost]
        public IActionResult Add(AddArticleFormModel article)
        {
            if (!companyService.Exists(article.CompanyId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(article);
            }

            var id = articleService.Add(
                article.CompanyId,
                article.Title,
                article.ImageUrl,
                article.Content,
                HttpContext.Request.Host.Value);

            if (id == null)
            {
                return BadRequest();
            }

            return RedirectToAction("Details", "Article", new { id = id });
        }

        public IActionResult Edit(string id)
        {
            // check if company is owner of article.

            return null;
        }

        [HttpPost]
        public IActionResult Edit(EditArticleFormModel article)
        {
            // check if company is owner of article.

            return null;
        }
    }
}