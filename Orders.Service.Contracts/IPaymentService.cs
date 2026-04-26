using Orders.Entities.Models;
using Orders.Shared.DataTransferObjects;

namespace Orders.Service.Contracts;

public interface IPaymentService
{
    // Task<OrderDto> GetOrder(Guid orderId);
    Task<PaymentReference> GeneratePaymentReference(Guid orderId);

    Task<Payment> Pay(Guid reference);
}