using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingDomain.Models;

public partial class User : Entity
{
    //public int Id { get; set; }
    [Display(Name = "Email")]
    public string PhoneOrEmail { get; set; } = null!;
    [Display(Name = "Ім'я та прізвище")]
    public string NameSurname { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}
