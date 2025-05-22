using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Category
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}