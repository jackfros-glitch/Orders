using Orders.Entities.Models;

namespace Orders.Contracts
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync(bool trackChanges);
        Task<Payment> GetPaymentAsync(Guid id, bool trackChanges = false);
        void CreatePayment(Payment payment);
        Task<IEnumerable<Payment>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeletePayment(Payment payment);

        Task<Payment> GetPaymentByRefIdAsync(Guid referenceId, bool trackChanges);
    }
}
