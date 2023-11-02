using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public DateTime Created { get; set; }
        public string Content { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int ProductId { get; set; }
        public string Id { get; set; } = null!;
    }
}
