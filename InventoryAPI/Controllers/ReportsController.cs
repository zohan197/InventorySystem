using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
        _context = context;
    }
    [Authorize]
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummaryReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
{
    var ordersQuery = _context.Orders
        .Include(o => o.Items)
        .ThenInclude(oi => oi.Product)
        .AsQueryable();

    if (startDate.HasValue)
        ordersQuery = ordersQuery.Where(o => o.OrderDate >= startDate.Value);

    if (endDate.HasValue)
        ordersQuery = ordersQuery.Where(o => o.OrderDate <= endDate.Value);

    var orders = await ordersQuery
        .Select(o => new {
            o.Id,
            o.CustomerName,
            o.OrderDate,
            o.TotalAmount
        }).ToListAsync();

    var orderItems = await _context.OrderItems
        .Include(oi => oi.Product)
        .Include(oi => oi.Order)
        .Where(oi => 
            (!startDate.HasValue || oi.Order.OrderDate >= startDate.Value) &&
            (!endDate.HasValue || oi.Order.OrderDate <= endDate.Value))
        .Select(oi => new {
            oi.Id,
            OrderId = oi.OrderId,
            ProductName = oi.Product != null ? oi.Product.Name : "N/A",
            oi.Quantity,
            oi.UnitPrice,
            Subtotal = oi.Quantity * oi.UnitPrice
        }).ToListAsync();

    var products = await _context.Products
        .Select(p => new {
            p.Id,
            p.Name,
            p.UnitPrice
        }).ToListAsync();

    var categories = await _context.Categories
        .Select(c => new {
            c.Id,
            c.Name
        }).ToListAsync();

    return Ok(new {
        orders,
        orderItems,
        products,
        categories
    });
}
    }
