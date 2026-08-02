using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AulaVector.Data;
using AulaVector.Models;
using AulaVector.Models.Enums;

namespace AulaVector.Web.Areas.Customers.Controllers
{
    [Area("Customers")]
    [Authorize]
    public class LibraryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibraryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers/Library
        public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            // Fetch only approved orders for the current user, including soft-deleted checks
            var purchasedItems = await _context.OrderDetails
                .Include(od => od.Product)
                .Include(od => od.Order)
                .Where(od => od.Order.UserId == userId 
                          && od.Order.PaymentStatus == PaymentStatus.Approved
                          && od.Product.IsActive)
                .ToListAsync();

            return View(purchasedItems);
        }

        // GET: Customers/Library/Download/{id}
        public async Task<IActionResult> Download(Guid id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid userId)) return Unauthorized();

            var orderDetail = await _context.OrderDetails
                .Include(od => od.Product)
                .Include(od => od.Order)
                .FirstOrDefaultAsync(od => od.Id == id && od.Order.UserId == userId);

            if (orderDetail == null || orderDetail.Order.PaymentStatus != PaymentStatus.Approved)
            {
                return NotFound("Purchase not found or payment not approved.");
            }

            // Enforce download limits
            if (orderDetail.DownloadLimit.HasValue && orderDetail.DownloadCount >= orderDetail.DownloadLimit.Value)
            {
                TempData["ErrorMessage"] = "Download limit exceeded for this product.";
                return RedirectToAction(nameof(Index));
            }

            // Update download audit
            orderDetail.DownloadCount++;
            _context.Update(orderDetail);
            await _context.SaveChangesAsync();

            // Here you would connect to FileService to return the actual physical file.
            // For now, it simulates a download return.
            var filePath = orderDetail.Product.PdfFilePath; 
            var fileName = $"{orderDetail.Product.Title}.pdf";

            // Temporary byte return for compilation purposes. 
            // In reality: return PhysicalFile(filePath, "application/pdf", fileName);
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes("Simulated PDF content");
            return File(fileBytes, "application/pdf", fileName);
        }
    }
}