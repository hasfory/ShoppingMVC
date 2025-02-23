using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingDomain.Models;

public partial class Product : Entity
{
    //public int Id { get; set; }
    [Display(Name = "Ціна товару")]
    public decimal Price { get; set; }
    [Display(Name = "Бренд")]
    public int BrandId { get; set; }
    [Display(Name = "Наявність товару")]
    public bool Availability { get; set; }
    [Display(Name = "Категорія")]
    public string Category { get; set; } = null!;
    [Display(Name = "Бренд")]
    public virtual Brand? Brand { get; set; } = null!;

    public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; } = new List<ShoppingCartProduct>();
}
