using System.Net;
using Orders.Shared.Enums;

namespace Orders.Shared.DataTransferObjects;

public record OrderDto
{
    public Guid Id { get; set; } 

    public Decimal TotalAmount { get; set; }

    public string Status { get; set;}

    public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
}