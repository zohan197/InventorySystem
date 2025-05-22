using System.Text.Json.Serialization;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int Stock { get; set; } = 0;
    public decimal UnitPrice { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    [JsonIgnore] // Prevents circular reference when serializing
    public Category? Category { get; set; }
}