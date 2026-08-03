namespace AulaVector.Models.Enums
{
    /// <summary>
    /// Defines the possible states of a payment transaction from an external API.
    /// </summary>
    public enum PaymentStatus
    {
        Pending = 0,
        Approved = 1,
        Canceled = 2,
        Refunded = 3
    }
}