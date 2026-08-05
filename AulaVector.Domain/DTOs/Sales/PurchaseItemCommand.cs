namespace AulaVector.Domain.DTOs.Sales;

public class PurchasedItemCommand
{
    public Guid ProductId { get; set; }
    public decimal UnitPrice { get; set; }
}