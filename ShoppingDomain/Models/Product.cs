using System;
using System.Collections.Generic;

namespace ShoppingDomain.Models;

public partial class Product
{
    public int Id { get; set; }

    public decimal Price { get; set; }

    public int BrandId { get; set; }

    public bool Availability { get; set; }

    public string Category { get; set; } = null!;

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; } = new List<ShoppingCartProduct>();
}
