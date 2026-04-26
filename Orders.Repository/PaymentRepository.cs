using Microsoft.EntityFrameworkCore;
using Orders.Contracts;
using Orders.Entities.Models;

namespace Orders.Repository
{
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void CreatePayment(Payment payment) => Create(payment);

        public void DeletePayment(Payment payment) => Delete(payment);

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync(bool trackChanges) =>
            await FindAll(trackChanges).OrderBy(p => p.Id).ToListAsync();

        public async Task<Payment> GetPaymentAsync(Guid Id, bool trackChanges = false) =>
            await FindByCondition(p => p.Id.Equals(Id), trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<Payment>> GetByIdsAsync(
            IEnumerable<Guid> ids,
            bool trackChanges
        ) => await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();

        public async Task<Payment> GetPaymentByRefIdAsync(Guid referenceId, bool trackChanges) => await FindByCondition((p)=> p.PaymentReferenceId.Equals(referenceId), trackChanges).SingleOrDefaultAsync();
    }
}
