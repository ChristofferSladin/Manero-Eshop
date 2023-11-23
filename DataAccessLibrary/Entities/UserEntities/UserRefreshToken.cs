using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Entities.UserEntities
{
    public class UserRefreshToken
    {
        [Key]
        public int TokenId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }

        [Required]
        public string Id { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Id))]
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
