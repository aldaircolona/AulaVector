using AulaVector.Data;
using AulaVector.Domain.Models;
using AulaVector.Domain.DTOs.Sales;
using AulaVector.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AulaVector.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SalesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<SalesController> _logger;

    public SalesController(
        ApplicationDbContext context, 
        UserManager<ApplicationUser> userManager,
        ILogger<SalesController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    /// Endpoint to register a new purchase via external Webhook.
    /// Example URL: POST ./api/sales/webhook
    /// </summary>
    [HttpPost("webhook")]
    public async Task<IActionResult> RegisterPurchase([FromBody] PurchaseCommand payload)
    {
        try
        {
            // 1. Verify if the customer exists in our Identity system via Email
            var user = await _userManager.FindByNameAsync(payload.UserName);
            if (user == null)
            {
                _logger.LogWarning($"Compra recibida para un usuario no registrado: {payload.UserName}");
                return NotFound(new { Message = "Usuario no encontrado en el sistema" });
            }

            // 2. Prevent duplicate transactions (Idempotency check)
            var transactionExists = await _context.Set<Order>()
                .AnyAsync(o => o.TransactionId == payload.TransactionId);
                
            if (transactionExists)
            {
                _logger.LogInformation($"Transacción duplicada evitada: {payload.TransactionId}");
                return Ok(new { Message = "La transacción ya fue procesada" }); 
            }

            // 3. Map incoming string status to internal Enum
            var mappedStatus = payload.PaymentStatus.ToUpper() switch
            {
                "APPROVED" => PaymentStatus.Approved,
                "PENDING" => PaymentStatus.Pending,
                "CANCELED" => PaymentStatus.Canceled,
                "REFUNDED" => PaymentStatus.Refunded, 
                _ => PaymentStatus.Pending
            };

            // 4. Create the Order entity using Guid 
            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TransactionId = payload.TransactionId,
                TotalAmount = payload.TotalAmount,
                Currency = payload.Currency,
                PaymentMethod = payload.PaymentMethod,
                PaymentStatus = mappedStatus,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };

            // 5. Build the Order Details (Granting access to digital products)
            var orderDetails = new List<OrderDetail>();
            foreach (var item in payload.Items)
            {
                var productExists = await _context.Set<Product>().AnyAsync(p => p.Id == item.ProductId && p.IsActive);
                if (!productExists) continue;

                orderDetails.Add(new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrderId = newOrder.Id,
                    ProductId = item.ProductId,
                    UnitPrice = item.UnitPrice,
                    DownloadCount = 0
                });
            }

            if (!orderDetails.Any())
            {
                _logger.LogWarning($"Transaction {payload.TransactionId} contained no valid active products.");
                return BadRequest(new { Message = "No valid products found for this transaction." });
            }

            // 6. Persist transaction safely
            await _context.Orders.AddAsync(newOrder);
            await _context.OrderDetails.AddRangeAsync(orderDetails);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Successfully registered purchase {payload.TransactionId} for {payload.UserName}");

            return CreatedAtAction(nameof(RegisterPurchase), new { id = newOrder.Id }, new { Message = "Purchase registered successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Critical error processing webhook for {TransactionId}", payload.TransactionId);
            return StatusCode(500, new { Message = "An internal server error occurred while processing the purchase." });
        }
    }
}