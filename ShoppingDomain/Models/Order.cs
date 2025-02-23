using System;
using System.Collections.Generic;

namespace ShoppingDomain.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateTime DateOfOrdering { get; set; }

    public string TransactionNumber { get; set; } = null!;

    public int ShoppingCartId { get; set; }

    public virtual ShoppingCart ShoppingCart { get; set; } = null!;
}
