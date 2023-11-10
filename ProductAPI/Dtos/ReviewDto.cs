using DataAccessLibrary.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Dtos
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Rating { get; set; }
        public DateTime Created { get; set; }
        public string Content { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int ProductId { get; set; }
        public string Id { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
