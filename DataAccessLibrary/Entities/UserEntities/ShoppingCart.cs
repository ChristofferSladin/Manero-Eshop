﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Entities.UserEntities;

[Table("ShoppingCart")]
public class ShoppingCart
{
    [Key] 
    public int ShoppingCartId { get; set; }

    [Required]
    public string Id { get; set; } = null!;
    [Required]
    [ForeignKey(nameof(Id))]
    public ApplicationUser ApplicationUser { get; set; } = null!;


    public virtual ICollection<ShoppingCartProduct>? ShoppingCartProducts { get; set; }
}
