using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingDomain.Models;

public partial class Payment : Entity
{
    //public int Id { get; set; }
    [Display(Name = "Номер транзакції")]
    public string TransactionNumber { get; set; } = null!;
    [Display(Name = "Сума платежу")]
    public decimal PaymentAmount { get; set; }
    [Display(Name = "Спосіб оплати")]
    public string PaymentMethod { get; set; } = null!;
    [Display(Name = "Користувач")]
    public int UserId { get; set; }
    [Display(Name = "Користувач")]
    public virtual User? User { get; set; } = null!;
}
