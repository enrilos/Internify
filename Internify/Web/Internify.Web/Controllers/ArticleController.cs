namespace Internify.Web.Controllers
{
    using Internify.Models.InputModels.Article;
    using Microsoft.AspNetCore.Mvc;
    using Services.Article;

    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;

        public ArticleController(IArticleService articleService)
            => this.articleService = articleService;

        public IActionResult CompanyArticles([FromQuery] ArticleListingQueryModel queryModel)
        {
            var articles = articleService.GetCompanyArticles(
                queryModel.CompanyId,
                queryModel.CompanyName,
                queryModel.Title,
                queryModel.CurrentPage,
                queryModel.ArticlesPerPage);

            return View(articles);
        }

        public IActionResult Details(string id)
        {
            var article = articleService.GetDetailsModel(id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }
    }
}