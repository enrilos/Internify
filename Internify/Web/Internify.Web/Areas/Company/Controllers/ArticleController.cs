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
            var companyId = companyService.GetIdByUserId(User.Id());

            if (!articleService.IsOwnedByCompany(id, companyId))
            {
                return Unauthorized();
            }

            var article = articleService.GetEditModel(id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpPost]
        public IActionResult Edit(EditArticleFormModel article)
        {
            var companyId = companyService.GetIdByUserId(User.Id());

            if (!articleService.IsOwnedByCompany(article.Id, companyId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(article);
            }

            var editResult = articleService.Edit(
                article.Id,
                article.Title,
                article.Content);

            if (!editResult)
            {
                return BadRequest();
            }

            return RedirectToAction("Details", "Article", new { id = article.Id });
        }

        public IActionResult Delete(string id)
        {
            var companyId = companyService.GetIdByUserId(User.Id());

            if (!articleService.IsOwnedByCompany(id, companyId))
            {
                return Unauthorized();
            }

            var result = articleService.Delete(id);

            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction("CompanyArticles", "Article", new { companyId = companyId });
        }
    }
}