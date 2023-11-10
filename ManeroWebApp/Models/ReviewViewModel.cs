namespace ManeroWebApp.Models
{
    public class ReviewViewModel
    {
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string? ProductName { get; set; }
        public DateTime Created { get; set; }
        public string Content { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int ProductId { get; set; }
        public string Id { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfileImage { get; set; }
        public int ReviewCount { get; set; }
    }
}
