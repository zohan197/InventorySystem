using System.ComponentModel.DataAnnotations;

public class ProductDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    [Range(0, 1)]
    public bool IsActive { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public int Stock { get; set; } = 0;
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
    public decimal UnitPrice { get; set; } = 0;
    [Required]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
    public string CategoryName { get; set; } = string.Empty;
}
