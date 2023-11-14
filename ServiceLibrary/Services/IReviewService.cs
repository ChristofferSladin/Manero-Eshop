using ServiceLibrary.Models;

namespace ServiceLibrary.Services
{
    public interface IReviewService
    {
        Task<List<Review>> GetFilteredReviewsAsync(int? page, int? take, string? orderBy, string? orderDirection,
            string? filterByName);
    }
}
