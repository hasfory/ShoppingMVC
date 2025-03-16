using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingDomain.Models;

public partial class Brand : Entity
{
    //public int Id { get; set; }
    [Display(Name = "Назва бренду")]
    [Required(ErrorMessage = "Назва бренду є обов'язковою.")]
    public string BrandName { get; set; } = null!;
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
