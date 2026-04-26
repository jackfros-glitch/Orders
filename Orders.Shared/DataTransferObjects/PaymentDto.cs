namespace Orders.Shared.DataTransferObjects;

public record PaymentDto
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string Status { get; set; }

    // Navigation back to parent
    public Guid PaymentReferenceId { get; set; }
}