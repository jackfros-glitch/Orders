using Microsoft.EntityFrameworkCore.Storage;
using Orders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IProductRepository> _productRepository;
        private readonly Lazy<IOrderRepository> _orderRepository;
        private readonly Lazy<IOrderItemRepository> _orderItemRepository;
        private readonly Lazy<ICustomerRepository> _customerRepository;
        private readonly Lazy<IPaymentReferenceRepository> _paymentRefernceRepository;
        private readonly Lazy<IPaymentRepository> _paymentRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(repositoryContext));
            _orderRepository = new Lazy<IOrderRepository>(() => new OrderRepository(repositoryContext));
            _orderItemRepository = new Lazy<IOrderItemRepository>(() => new OrderItemRepository(repositoryContext));
            _customerRepository = new Lazy<ICustomerRepository>(() => new CustomerRepository(repositoryContext));
            _paymentRefernceRepository = new Lazy<IPaymentReferenceRepository>( ()=> new PaymentReferenceRepository(repositoryContext));
            _paymentRepository = new Lazy<IPaymentRepository>( ()=> new PaymentRepository(repositoryContext));

        }

        public IProductRepository Product => _productRepository.Value;

        public IOrderRepository Order => _orderRepository.Value;

        public IOrderItemRepository OrderItem =>  _orderItemRepository.Value;

        public ICustomerRepository Customer => _customerRepository.Value;

        public IPaymentReferenceRepository PaymentReference => _paymentRefernceRepository.Value;

        public IPaymentRepository Payment => _paymentRepository.Value;

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();

        public async Task<IDbContextTransaction> BeginTransactionAsync() => await _repositoryContext.Database.BeginTransactionAsync();

        
    }
}
