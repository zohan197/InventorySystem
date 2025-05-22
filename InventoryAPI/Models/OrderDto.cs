using System.ComponentModel.DataAnnotations;

public class OrderDto
{
    public int Id { get; set; }
    [Required]
    public string CustomerName { get; set; } = string.Empty;
    [Required]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
    public decimal TotalAmount { get; set; }
    [Required]
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    // Optional for display only
    public string? ProductName { get; set; }
}
