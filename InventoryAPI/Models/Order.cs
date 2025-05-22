using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string CustomerName { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    // Foreign key to the user who created the order
    public int CreatedByUserId { get; set; }

    [ForeignKey(nameof(CreatedByUserId))]
    public User? CreatedBy { get; set; }

    public List<OrderItem> Items { get; set; } = new();
}
