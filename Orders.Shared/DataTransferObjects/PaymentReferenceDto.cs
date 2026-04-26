using System.Net;
using Orders.Shared.Enums;

namespace Orders.Shared.DataTransferObjects;

public record PaymentReferenceDto
{
    public Guid Id { get; set; } 

    public Guid Reference { get; set;}
    public Guid OrderId { get; set; }
    public Decimal Amount { get; set; }
    
}