namespace Orders.Service.Contracts
{
    public interface IServiceManager
    {
        //IAuthenticationService AuthenticationService { get; }
        IOrderService OrderService { get; }

        IProductService ProductService { get; }

        IPaymentService PaymentService { get; }
    }
}
