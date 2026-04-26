using Orders.Entities.Models;

namespace Orders.Contracts
{
    public interface IPaymentReferenceRepository
    {
        void CreateReference(PaymentReference paymentReference);

        Task<PaymentReference> GetReferenceByOrderIdAsync(Guid orderId, bool trackChanges);

        Task<PaymentReference> GetPaymentReferenceAsync(
            Guid referenceRefId,
            bool trackChanges,
            bool eagerLoad
        );
    }
}
