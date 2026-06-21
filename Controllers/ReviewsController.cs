using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NexsolCrmBackendVersion2.Dtos.Reviews;
using NexsolCrmBackendVersion2.Models.Reviews;
using NexsolCrmBackendVersion2.Services.ReviewsService;

namespace NexsolCrmBackendVersion2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly ReviewService _service;
        private readonly IMongoCollection<Review> _reviews;

        public ReviewsController(IMongoDatabase database)
        {
            _service = new();
            _reviews = database.GetCollection<Review>("reviews");
        }

        #region GET

        [HttpGet("all")]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _service.GetAllReviewsService(_reviews);
            return Ok(reviews);
        }

        #endregion

        #region PATCH

        [HttpPatch("review/{id}")]
        public async Task<IActionResult> UpdateOneReview(string id, [FromBody] ReviewDto newReview)
        {
            var review = await _service.UpdateOneReviewService(id, newReview, _reviews);

            if (review == null)
                return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(review);
        }

        #endregion

        #region DELETE

        [HttpDelete("review/{id}")]
        public async Task<IActionResult> DeleteOneReview(string id)
        {
            var deletedReview = await _service.DeleteOneReviewService(id, _reviews);

            if (!deletedReview)
                return NotFound();

            return Ok(new { success = true });
        }

        #endregion
    }
}
