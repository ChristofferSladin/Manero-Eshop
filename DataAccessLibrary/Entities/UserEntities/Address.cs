using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Entities.UserEntities;

public class Address
{
    [Key] 
    public int AddressId { get; set; }
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string Country { get; set; } = null!;
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string StreetName { get; set; } = null!;
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string City { get; set; } = null!;
    [Required]
    [Column(TypeName = "nvarchar(10)")]
    public string ZipCode { get; set; } = null!;

    [Required]
    public string Id { get; set; } = null!;
    [Required]
    [ForeignKey(nameof(Id))]
    public ApplicationUser ApplicationUser { get; set; } = null!;
}
