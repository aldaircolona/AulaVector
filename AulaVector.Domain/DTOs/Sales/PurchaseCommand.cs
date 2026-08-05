namespace AulaVector.Domain.DTOs.Sales;
/// <summary>
/// DTO for receiving payload from external payment providers (e.g., Hotmart, Stripe).
/// </summary>
public class PurchaseCommand
{
    public string UserName { get; set; } = string.Empty; 
    
    public string TransactionId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "PEN";
    public string PaymentMethod { get; set; } = string.Empty;
    
    public string PaymentStatus { get; set; } = string.Empty; 

    public List<PurchasedItemCommand> Items { get; set; } = new List<PurchasedItemCommand>();
}