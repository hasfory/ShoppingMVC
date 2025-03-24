using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingDomain.Models;

public partial class Payment : Entity
{
    //public int Id { get; set; }
    [Display(Name = "Номер транзакції")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Номер транзакції повинен бути цілим числом.")]
    [Required(ErrorMessage = "Номер транзакції є обов'язковим.")]
    public string TransactionNumber { get; set; } = null!;
    [Display(Name = "Сума платежу")]
    [Required(ErrorMessage = "Сума платежу є обов'язковою.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Сума платежу повинна бути більше 0.")]
    public decimal? PaymentAmount { get; set; }
    [Display(Name = "Спосіб оплати")]
    [Required(ErrorMessage = "Будь ласка, оберіть спосіб оплати.")]
    public string PaymentMethod { get; set; } = null!;
    [Display(Name = "Користувач")]
    public string? UserId { get; set; }
    [Display(Name = "Користувач")]
    public virtual ApplicationUser? User { get; set; } = null!;
}