using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }
    [Authorize]
    [HttpGet("summary")]
    public async Task<IActionResult> GetDashboardSummary()
    {
        var totalOrders = await _context.Orders.CountAsync();
        var totalRevenue = await _context.Orders.SumAsync(o => (decimal?)o.TotalAmount) ?? 0;
        var activeProducts = await _context.Products.CountAsync(p => p.IsActive);
        var lowStockCount = await _context.Products.CountAsync(p => p.Stock <= 5);

        var salesOverTime = _context.Orders
        .GroupBy(o => o.OrderDate.Date)
        .Select(g => new {
            date = g.Key,
            total = g.Sum(x => x.TotalAmount)
        })
        .OrderBy(x => x.date)
        .AsEnumerable() // switch to client-side evaluation
        .Select(x => new {
            date = x.date.ToString("yyyy-MM-dd"), // safely use .ToString here
            x.total
        })
        .ToList(); 


        var topProducts = await _context.OrderItems
            .Include(oi => oi.Product)
            .GroupBy(oi => oi.Product.Name)
            .Select(g => new {
                name = g.Key,
                quantity = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.quantity)
            .Take(5)
            .ToListAsync();

        return Ok(new {
            totalOrders,
            totalRevenue,
            activeProducts,
            lowStockCount,
            salesOverTime,
            topProducts
        });
    }
}
