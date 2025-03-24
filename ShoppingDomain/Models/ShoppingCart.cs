using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingDomain.Models;

public partial class ShoppingCart : Entity
{
    //public int Id { get; set; }
    [Display(Name = "Користувач")]
    public string? UserId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [Display(Name = "Користувач")]
    public virtual ApplicationUser? User { get; set; } = null!;
}
