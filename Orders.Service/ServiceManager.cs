using AutoMapper;
using Orders.Service.Contracts;
using Orders.Contracts;
using Microsoft.Extensions.Configuration;

namespace Orders.Service;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IOrderService> _orderService;

    private readonly Lazy<IProductService> _productService;
    private readonly Lazy<IPaymentService> _paymentService;

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IConfiguration config, IMapper mapper)
    {
    _orderService = new Lazy<IOrderService>(() => new OrderService(repositoryManager, logger, config, mapper, this));
    _productService = new Lazy<IProductService>(() => new ProductService(repositoryManager, logger, config, mapper));
    _paymentService = new Lazy<IPaymentService>(()=> new PaymentService(repositoryManager, logger, config, mapper, this));
    }
    
    public IOrderService OrderService => _orderService.Value;

    public IProductService ProductService => _productService.Value;

    public IPaymentService PaymentService => _paymentService.Value;
}