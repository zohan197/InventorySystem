using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
    {
        var orders = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();

        var result = orders
        .OrderByDescending(o => o.Id)
        .Select(o => new OrderDto
        {
            Id = o.Id,
            CustomerName = o.CustomerName,
            OrderDate = o.OrderDate,
            TotalAmount = o.Items.Sum(i => i.UnitPrice * i.Quantity),
            Items = o.Items.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                ProductName = i.Product?.Name
            }).ToList()
        }).ToList();


        return Ok(result);
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Create(OrderDto dto)
    {
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    if (dto.Items == null || !dto.Items.Any())
        return BadRequest("Order must contain at least one item.");

        // Start a transaction
    using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        var productIds = dto.Items.Select(i => i.ProductId).ToList();
        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();

        var orderItems = new List<OrderItem>();

        foreach (var item in dto.Items)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);

            if (product == null)
                return BadRequest($"Product with ID {item.ProductId} does not exist.");

            if (product.Stock < item.Quantity)
                return BadRequest($"Not enough stock for product '{product.Name}'. Available: {product.Stock}, Requested: {item.Quantity}");

            product.Stock -= item.Quantity;

            orderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            });
        }

        var totalAmount = orderItems.Sum(i => i.Quantity * i.UnitPrice);
        int userIdClaim = int.Parse(User.FindFirst("userId")?.Value ?? "0");
        var order = new Order
        {
            CustomerName = dto.CustomerName,
            OrderDate = dto.OrderDate,
            TotalAmount = totalAmount,
            Items = orderItems,
            CreatedByUserId = userIdClaim // Assuming you have a method to get the current user's ID
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

            // ✅ Load related data like product names
            var savedOrder = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            var result = new OrderDto
            {
                Id = savedOrder.Id,
                CustomerName = savedOrder.CustomerName,
                OrderDate = savedOrder.OrderDate,
                TotalAmount = savedOrder.TotalAmount,
                Items = savedOrder.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    ProductName = i.Product?.Name
                }).ToList()
            };

        return Ok(result); 
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }


    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetById(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound();

        var result = new OrderDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            OrderDate = order.OrderDate,
            Items = order.Items.Select(i => new OrderItemDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                ProductName = i.Product?.Name
            }).ToList()
        };

        return Ok(result);
    }
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product) // Ensure we include Product in the query
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound("Order not found.");

        // Add back the quantities to the respective products
        foreach (var item in order.Items)
        {
            if (item.Product != null)
            {
                item.Product.Stock += item.Quantity;
            }
        }

        _context.OrderItems.RemoveRange(order.Items); // Remove related items first
        _context.Orders.Remove(order);                // Then remove the order itself

        await _context.SaveChangesAsync();

        return Ok("Order deleted and product quantities restored.");
    }

}