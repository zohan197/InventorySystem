using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _context.Categories
        .Where(c => c.Id != 0)
        .ToListAsync();

        return Ok(categories);
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var existing = await _context.Categories.FirstOrDefaultAsync(c => c.Name == category.Name);
        
        if (existing != null)
        {
            return BadRequest("Category already exists.");
        }
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return Ok(category);
    }
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Category category)
    {
        var existing = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        
        if (existing == null)
        {
            return NotFound();
        }

        existing.Name = category.Name;
        await _context.SaveChangesAsync();
        return Ok(existing);
    }
    [Authorize]
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id)
{
    if (id == 0)
    {
        return BadRequest("Cannot delete the 'Uncategorized' category.");
    }

    var existing = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
    if (existing == null)
    {
        return NotFound();
    }

    // Step 1: Reassign products to 'Uncategorized' (Id = 0)
    var productsToUpdate = await _context.Products
        .Where(p => p.CategoryId == id)
        .ToListAsync();

    foreach (var product in productsToUpdate)
    {
        product.CategoryId = 0;
    }

    // Step 2: Save the product updates
    if (productsToUpdate.Any())
    {
        await _context.SaveChangesAsync();
    }

    // Step 3: Remove the category
    _context.Categories.Remove(existing);
    await _context.SaveChangesAsync();

    return Ok(existing);
}

}

