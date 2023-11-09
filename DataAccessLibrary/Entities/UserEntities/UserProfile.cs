using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Entities.UserEntities
{
    public class UserProfile
    {
        [Key]
        public int UserProfileId { get; set; }

        public string? ProfileImage { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required]
        public string Id { get; set; } = null!;
        [Required]
        [ForeignKey(nameof(Id))]
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
