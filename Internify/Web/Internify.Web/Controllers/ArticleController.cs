namespace Internify.Web.Controllers
{
    using Internify.Models.InputModels.Article;
    using Microsoft.AspNetCore.Mvc;

    public class ArticleController : Controller
    {
        // services..

        // ctor..

        public IActionResult CompanyArticles([FromQuery] ArticleListingQueryModel queryModel)
        {
            // TODO..
            // filter/paginate..

            return null;
        }

        public IActionResult Details(string id)
        {
            // check if company is owner.

            return View(new Object { });
        }
    }
}