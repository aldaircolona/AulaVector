namespace AulaVector.Models;

/// <summary>
/// Represents a digital product (e.g., PDF book) in the store.
/// </summary>
public class Product
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public string Author { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    
    /// <summary>
    /// File path or key to access the PDF stored in the server or storage service.
    /// </summary>
    public string PdfFilePath { get; set; } = string.Empty;
    
    /// <summary>
    /// URL or file path for the book's cover image.
    /// </summary>
    public string CoverImageUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Soft delete flag to enable or disable product visibility in the store.
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    public int PageCount { get; set; }
    
    public long FileSizeBytes { get; set; }

    // Navigation properties
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}