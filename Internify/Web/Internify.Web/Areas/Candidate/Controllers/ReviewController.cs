namespace Internify.Web.Areas.Candidate.Controllers
{
    using Infrastructure.Extensions;
    using Internify.Models.InputModels.Review;
    using Microsoft.AspNetCore.Mvc;
    using Services.Candidate;
    using Services.Company;
    using Services.Review;

    public class ReviewController : CandidateControllerBase
    {
        private readonly IReviewService reviewService;
        private readonly ICandidateService candidateService;
        private readonly ICompanyService companyService;

        public ReviewController(
            IReviewService reviewService,
            ICandidateService candidateService,
            ICompanyService companyService)
        {
            this.reviewService = reviewService;
            this.candidateService = candidateService;
            this.companyService = companyService;
        }

        public IActionResult CandidateReviews(ReviewListingQueryModel queryModel)
        {
            if (!candidateService.Exists(queryModel.CandidateId))
            {
                return NotFound();
            }

            if (candidateService.GetIdByUserId(User.Id()) != queryModel.CandidateId)
            {
                return Unauthorized();
            }

            queryModel = reviewService.CandidateReviews(
                queryModel.CandidateId,
                queryModel.CompanyId,
                queryModel.Title,
                queryModel.Rating,
                queryModel.CurrentPage,
                queryModel.ReviewsPerPage);

            queryModel.Companies = companyService.GetCompaniesSelectOptions();

            return View(queryModel);
        }

        public IActionResult Add(string companyId)
        {
            if (!companyService.Exists(companyId))
            {
                return NotFound();
            }

            var candidateId = candidateService.GetIdByUserId(User.Id());

            if (!candidateService.IsCandidateInCompany(candidateId, companyId))
            {
                return Unauthorized();
            }

            var reviewModel = new AddReviewFormModel
            {
                CandidateId = candidateId,
                CompanyId = companyId
            };

            return View(reviewModel);
        }

        [HttpPost]
        public IActionResult Add(AddReviewFormModel review)
        {
            if (!companyService.Exists(review.CompanyId)
                || !candidateService.Exists(review.CandidateId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(review);
            }

            var result = reviewService.Add(
                review.CandidateId,
                review.CompanyId,
                review.Title,
                review.Rating,
                review.Content);

            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction("Details", "Company", new { id = review.CompanyId });
        }
    }
}