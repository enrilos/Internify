namespace Internify.Web.Controllers
{
    using Internify.Models.InputModels.Review;
    using Microsoft.AspNetCore.Mvc;
    using Services.Company;
    using Services.Review;

    public class ReviewController : Controller
    {
        private readonly IReviewService reviewService;
        private readonly ICompanyService companyService;

        public ReviewController(
            IReviewService reviewService,
            ICompanyService companyService)
        {
            this.reviewService = reviewService;
            this.companyService = companyService;
        }

        public IActionResult CompanyReviews(ReviewListingQueryModel queryModel)
        {
            if (!companyService.Exists(queryModel.CompanyId))
            {
                return NotFound();
            }

            queryModel = reviewService.CompanyReviews(
                queryModel.CompanyId,
                queryModel.Title,
                queryModel.Rating,
                queryModel.CurrentPage,
                queryModel.ReviewsPerPage);

            return View(queryModel);
        }
    }
}