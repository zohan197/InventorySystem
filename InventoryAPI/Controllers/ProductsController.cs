using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? categoryId)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        var products = await query
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                Stock = p.Stock,
                UnitPrice = p.UnitPrice
            })
            .ToListAsync();

        return Ok(products);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    // Retrieve the category from the database
    var category = await _context.Categories
        .FirstOrDefaultAsync(c => c.Id == product.CategoryId);

    if (category == null)
        return NotFound("Category not found.");

    // Add the category to the product's Category navigation property
    product.Category = category;

    _context.Products.Add(product);
    await _context.SaveChangesAsync();

    // Map to DTO
    var productDto = new ProductDto
    {
        Id = product.Id,
        Name = product.Name,
        CategoryId = product.CategoryId,
        CategoryName = category.Name,
        Stock = product.Stock,
        UnitPrice = product.UnitPrice,
        IsActive = product.IsActive
    };

    return Ok(productDto);
}

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product product)
    {
        var existing = await _context.Products
        .Include(p => p.Category)  // Include the Category data
        .FirstOrDefaultAsync(p => p.Id == id);
        
        if (existing == null)
        {
            return NotFound();
        }

        existing.Name = product.Name;
        existing.CategoryId = product.CategoryId;
        existing.IsActive = product.IsActive;
        existing.Stock = product.Stock;
        existing.UnitPrice = product.UnitPrice;

        await _context.SaveChangesAsync();

        await _context.Entry(existing).Reference(p => p.Category).LoadAsync();

        var productDto = new ProductDto
        {
            Id = existing.Id,
            Name = existing.Name,
            IsActive = existing.IsActive,
            CategoryId = existing.CategoryId,
            CategoryName = existing.Category?.Name ?? "Unknown",
            Stock = existing.Stock,
            UnitPrice = existing.UnitPrice
        };

        return Ok(productDto);
    }
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
