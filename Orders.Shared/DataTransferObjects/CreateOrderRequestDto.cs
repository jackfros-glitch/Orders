using System.Net;
using Orders.Shared.Enums;

namespace Orders.Shared.DataTransferObjects;

public record CreateOrderRequestDto
{
    public List<ProductRequestDto> Products{ get; set; } = new List<ProductRequestDto>();


}