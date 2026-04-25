namespace Orders.Shared.DataTransferObjects;

public record OrderItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }

    public string ProductName { get; set; }
    public int Quantity { get; set; }
}