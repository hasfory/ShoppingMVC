using System;
using System.Collections.Generic;

namespace ShoppingDomain.Models;

public partial class Payment
{
    public int Id { get; set; }

    public string TransactionNumber { get; set; } = null!;

    public decimal PaymentAmount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
