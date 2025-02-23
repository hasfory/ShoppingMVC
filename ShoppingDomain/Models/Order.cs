using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingDomain.Models;

public partial class Order : Entity
{
    //public int Id { get; set; }
    [Display(Name = "Дата оформлення замовлення")]
    public DateTime DateOfOrdering { get; set; }
    [Display(Name = "Номер транзакції")]
    public string TransactionNumber { get; set; } = null!;
    [Display(Name = "Статус")]
    public int ShoppingCartId { get; set; }
    [Display(Name = "Кошик")]
    public virtual ShoppingCart? ShoppingCart { get; set; } = null!;
}
