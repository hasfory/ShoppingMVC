using System;
using System.Collections.Generic;

namespace ShoppingDomain.Models;

public partial class ShoppingCartProduct
{
    public int Id { get; set; }

    public int ShoppingCartId { get; set; }

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ShoppingCart ShoppingCart { get; set; } = null!;
}
