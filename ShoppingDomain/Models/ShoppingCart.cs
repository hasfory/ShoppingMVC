using System;
using System.Collections.Generic;

namespace ShoppingDomain.Models;

public partial class ShoppingCart
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; } = new List<ShoppingCartProduct>();

    public virtual User User { get; set; } = null!;
}
