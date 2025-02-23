using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingDomain.Models;

public partial class ShoppingCartProduct : Entity
{
    //public int Id { get; set; }
    [Display(Name = "Бонусна карта")]
    public int ShoppingCartId { get; set; }
    [Display(Name = "Товар")]
    public int ProductId { get; set; }
    [Display(Name = "Товар")]
    public virtual Product? Product { get; set; } = null!;
    [Display(Name = "Бонусна карта")]
    public virtual ShoppingCart? ShoppingCart { get; set; } = null!;
}
