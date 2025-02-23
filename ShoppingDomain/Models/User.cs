using System;
using System.Collections.Generic;

namespace ShoppingDomain.Models;

public partial class User
{
    public int Id { get; set; }

    public string PhoneOrEmail { get; set; } = null!;

    public string NameSurname { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}
