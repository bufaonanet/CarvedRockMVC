using CarvedRock.Admin.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarvedRock.Admin.Models;

public class ProductModel
{
    public int Id { get; set; }

    [Required]
    [DisplayName("PRODUCT NAME")]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [DataType(DataType.Currency)]
    [Range(0.01, 1000.00, ErrorMessage = "Value for {0} must be between {1:C} and {2:C}")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [DisplayName("Category")]
    public string CategoryName { get; set; }

    public List<SelectListItem> AvailableCategories { get; set; } = new();

    public static ProductModel ToModel(Product product)
    {
        return new ProductModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId ?? 0,
            CategoryName = product.Category?.Name

        };
    }

    public Product ToEntity()
    {
        return new Product
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Price = Price,
            IsActive = IsActive,
            CategoryId = CategoryId,
        };

    }
}