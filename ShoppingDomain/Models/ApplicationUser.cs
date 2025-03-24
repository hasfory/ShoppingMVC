using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ShoppingDomain.Models;

public partial class ApplicationUser : IdentityUser
{
    //public int Id { get; set; }
    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Неправильний формат електронної адреси")]
    [Required(ErrorMessage = "Це поле є обов'язковим.")]
    public string PhoneOrEmail { get; set; } = null!;
    [Display(Name = "Ім'я та прізвище")]
    [Required(ErrorMessage = "Це поле є обов'язковим.")]
    public string NameSurname { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
    }