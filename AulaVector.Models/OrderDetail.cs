namespace AulaVector.Models;

/// <summary>
/// Represents a specific product line item within an order and manages download access.
/// </summary>
public class OrderDetail
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    /// <summary>
    /// Historical price captured at the moment of purchase.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Maximum allowed downloads for this purchased item (null means unlimited).
    /// </summary>
    public int? DownloadLimit { get; set; }

    /// <summary>
    /// Number of times the user has downloaded this file.
    /// </summary>
    public int DownloadCount { get; set; } = 0;
}