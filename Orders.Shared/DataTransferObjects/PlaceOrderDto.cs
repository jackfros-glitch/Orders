namespace Orders.Shared.DataTransferObjects;

public record PlaceOrderDto
{
    public List<OrderItemDto> OrderItems { get; set; }
    public int TotalAmount { get; set; }
}