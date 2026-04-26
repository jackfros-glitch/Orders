using Microsoft.EntityFrameworkCore;
using Orders.Contracts;
using Orders.Entities.Models;

namespace Orders.Repository
{
    public class PaymentReferenceRepository
        : RepositoryBase<PaymentReference>,
            IPaymentReferenceRepository
    {
        public PaymentReferenceRepository(RepositoryContext repositoryContext)
            : base(repositoryContext) { }

        public void CreateReference(PaymentReference paymentReference) => Create(paymentReference);

        public async Task<PaymentReference> GetReferenceByOrderIdAsync(
            Guid orderId,
            bool trackChanges
        ) =>
            await FindByCondition((r) => r.OrderId.Equals(orderId), trackChanges)
                .SingleOrDefaultAsync();

        public async Task<PaymentReference> GetPaymentReferenceAsync(
            Guid referenceRefId,
            bool trackChanges,
            bool eagerLoad
        ) =>
            !eagerLoad
                ? await FindByCondition((pr) => pr.Reference.Equals(referenceRefId), trackChanges)
                    .SingleOrDefaultAsync()
                : await FindByCondition((pr) => pr.Reference.Equals(referenceRefId), trackChanges)
                    .Include(pr => pr.Order)
                    .ThenInclude(o=> o.OrderItems)
                    .SingleOrDefaultAsync();
    }
}
