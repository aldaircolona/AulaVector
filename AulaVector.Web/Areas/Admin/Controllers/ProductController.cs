using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AulaVector.Data;
using AulaVector.Models;
using AulaVector.Utils;

namespace AulaVector.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    // [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Product
        public async Task<IActionResult> Index()
        {
            var products = await _db.Products.ToListAsync();
            return View(products);
        }

        // GET: Admin/Product/Details/{guid}
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var product = await _db.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? coverFile, IFormFile? pdfFile)
        {
            if (ModelState.IsValid)
            {
                product.Id = Guid.NewGuid();
                product.CreatedAt = DateTime.UtcNow;
                product.IsActive = true;

                // Manejo de la portada pública (wwwroot/images/products)
                if (coverFile != null && coverFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = $"{Guid.NewGuid()}_{coverFile.FileName}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await coverFile.CopyToAsync(fileStream);
                    }
                    product.CoverImageUrl = $"/images/products/{uniqueFileName}";
                }

                // Manejo del archivo PDF protegido (fuera de wwwroot o en directorio seguro)
                if (pdfFile != null && pdfFile.Length > 0)
                {
                    string pdfFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "ProtectedFiles", "Pdfs");
                    Directory.CreateDirectory(pdfFolder);
                    string uniquePdfName = $"{Guid.NewGuid()}_{pdfFile.FileName}";
                    string pdfPath = Path.Combine(pdfFolder, uniquePdfName);

                    using (var fileStream = new FileStream(pdfPath, FileMode.Create))
                    {
                        await pdfFile.CopyToAsync(fileStream);
                    }
                    product.PdfFilePath = pdfPath;
                }

                _db.Add(product);
                await _db.SaveChangesAsync();
                TempData["success"] = "Producto creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Admin/Product/Edit/{guid}
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Admin/Product/Edit/{guid}
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Product product, IFormFile? coverFile, IFormFile? pdfFile)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _db.Products.FindAsync(id);
                    if (existingProduct == null) return NotFound();

                    existingProduct.Title = product.Title;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.IsActive = product.IsActive;
                    existingProduct.UpdatedAt = DateTime.UtcNow;

                    // Actualizar Portada si se envió un nuevo archivo
                    if (coverFile != null && coverFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                        Directory.CreateDirectory(uploadsFolder);
                        string uniqueFileName = $"{Guid.NewGuid()}_{coverFile.FileName}";
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await coverFile.CopyToAsync(fileStream);
                        }
                        existingProduct.CoverImageUrl = $"/images/products/{uniqueFileName}";
                    }

                    // Actualizar PDF si se envió un nuevo archivo
                    if (pdfFile != null && pdfFile.Length > 0)
                    {
                        string pdfFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "ProtectedFiles", "Pdfs");
                        Directory.CreateDirectory(pdfFolder);
                        string uniquePdfName = $"{Guid.NewGuid()}_{pdfFile.FileName}";
                        string pdfPath = Path.Combine(pdfFolder, uniquePdfName);

                        using (var fileStream = new FileStream(pdfPath, FileMode.Create))
                        {
                            await pdfFile.CopyToAsync(fileStream);
                        }
                        existingProduct.PdfFilePath = pdfPath;
                    }

                    _db.Update(existingProduct);
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Producto actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _db.Products.AnyAsync(e => e.Id == product.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // POST: Admin/Product/ToggleStatus/{guid} (Soft Delete / Reactivación)
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.IsActive = !product.IsActive;
            product.UpdatedAt = DateTime.UtcNow;

            _db.Update(product);
            await _db.SaveChangesAsync();

            TempData["success"] = product.IsActive ? "Producto activado." : "Producto deshabilitado (Soft Delete).";
            return RedirectToAction(nameof(Index));
        }
    }
}