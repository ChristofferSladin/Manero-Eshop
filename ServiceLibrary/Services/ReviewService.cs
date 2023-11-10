using Newtonsoft.Json.Linq;
using ServiceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Services
{
    public class ReviewService : IReviewService
    {
        public async Task<List<Review>> GetFilteredReviewsAsync(int? page, int? take, string? orderBy, string? orderDirection, string? filterByName)
        {
            var reviews = new List<Review>();
            try
            {
                var uriBuilder = new UriBuilder("https://localhost:7067/reviews/filter");
                var query = new StringBuilder();
                if (page.HasValue)
                    query.Append($"page={page.Value}&");
                if (take.HasValue)
                    query.Append($"take={take.Value}&");
                if (!string.IsNullOrEmpty(orderBy))
                    query.Append($"orderByField={Uri.EscapeDataString(orderBy)}&");
                if (!string.IsNullOrEmpty(orderDirection))
                    query.Append($"orderDirection={Uri.EscapeDataString(orderDirection)}&");
                if (!string.IsNullOrEmpty(filterByName))
                    query.Append($"filterByName={Uri.EscapeDataString(filterByName)}&");
                if (query.Length > 0)
                    query.Length--;

                uriBuilder.Query = query.ToString();

                var baseUrl = uriBuilder.Uri.ToString();
                using var client = new HttpClient();
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri(baseUrl);
                request.Method = HttpMethod.Get;
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    dynamic jsonArray = JArray.Parse(responseString);
                    foreach (var review in jsonArray)
                    {
                        reviews.Add(review.ToObject<Review>());
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return reviews;
        }
    }
}
