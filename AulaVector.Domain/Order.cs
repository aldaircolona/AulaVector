using AulaVector.Models.Enums;

namespace AulaVector.Models;

/// <summary>
/// Represents a purchase order placed by a user.
/// </summary>
public class Order
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Foreign Key referring to the ASP.NET Core Identity user (AspNetUser.Id).
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// External payment processor transaction ID (e.g., Stripe, PayPal, MercadoPago).
    /// </summary>
    public string TransactionId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "PEN";

    /// <summary>
    /// Payment status (e.g., "Pending", "Completed", "Failed").
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    /// <summary>
    /// Payment method used (e.g., "Credit Card", "PayPal", "Transfer").
    /// </summary>
    public string PaymentMethod { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;    

    // Navigation properties
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}