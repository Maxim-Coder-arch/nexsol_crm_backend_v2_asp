using MongoDB.Driver;
using NexsolCrmBackendVersion2.Dtos.Reviews;
using NexsolCrmBackendVersion2.Models.Reviews;

namespace NexsolCrmBackendVersion2.Services.ReviewsService
{
    public class ReviewService
    {
        public async Task<List<Review>> GetAllReviewsService(IMongoCollection<Review> _reviews)
        {
            return await _reviews.Find(_ => true).ToListAsync();
        }

        public async Task<Review?> UpdateOneReviewService(string id, ReviewDto newReview, IMongoCollection<Review> _reviews)
        {
            var filter = Builders<Review>.Filter.Eq(r => r.Id, id);
            var updateReview = Builders<Review>.Update.Set(r => r.Status, newReview.Status).Set(r => r.UpdatedAt, DateTime.UtcNow);
            var result = await _reviews.UpdateOneAsync(filter, updateReview);

            if (result.MatchedCount == 0)
                return null;

            return await _reviews.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteOneReviewService(string id, IMongoCollection<Review> _reviews)
        {
            var filter = Builders<Review>.Filter.Eq(r => r.Id, id);
            var result = await _reviews.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
