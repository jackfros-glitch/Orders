using System.Net;
using Orders.Shared.Enums;

namespace Orders.Shared.DataTransferObjects;

public record OrderDto
{
    public Guid Id { get; set; } 
    public List<ProductDto> Products { get; set; }

    public Decimal TotalAmount { get; set; }

    public OrderStatus Status { get; set;}
}