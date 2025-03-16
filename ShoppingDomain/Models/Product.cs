using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingDomain.Models;

public partial class Product : Entity
{
    //public int Id { get; set; }
    [Display(Name = "Ціна товару")]
    [Required(ErrorMessage = "Це поле є обов'язковим.")]
    public decimal? Price { get; set; }
    [Display(Name = "Бренд")]
    public int BrandId { get; set; }
    [Display(Name = "Наявність товару")]
    public bool Availability { get; set; }
    [Display(Name = "Назва товару")]
    [Required(ErrorMessage = "Це поле є обов'язковим.")]
    [StringLength(100, ErrorMessage = "Назва товару має містити до 100 символів.")]
    public string Name { get; set; } = null!;
    [Display(Name = "Бренд")]
    public virtual Brand? Brand { get; set; } = null!;

    public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; } = new List<ShoppingCartProduct>();
}
