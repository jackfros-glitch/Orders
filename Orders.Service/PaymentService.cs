using System.Runtime.InteropServices;
using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Orders.Contracts;
using Orders.Entities.Exceptions;
using Orders.Entities.Models;
using Orders.Service.Contracts;
using Orders.Shared.DataTransferObjects;
using Orders.Shared.Enums;

namespace Orders.Service;

public class PaymentService : IPaymentService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IConfiguration _config;

    private readonly IMapper _mapper;
    private readonly IServiceManager _service;

    public PaymentService(
        IRepositoryManager repoManager,
        ILoggerManager loggerManager,
        IConfiguration configuration,
        IMapper mapper,
        IServiceManager serviceManager
    )
    {
        _repositoryManager = repoManager;
        _loggerManager = loggerManager;
        _config = configuration;
        _mapper = mapper;
        _service = serviceManager;
    }

    public async Task<PaymentReference> GeneratePaymentReference(Guid orderId)
    {
        var order = await _repositoryManager.Order.GetOrderAsync(orderId, true, false);

        if (order is null)
            throw new OrderNotFoundException(orderId);

        var paymentReference = new PaymentReference
        {
            OrderId = orderId,
            Amount = order.TotalAmount,
        };
        _repositoryManager.PaymentReference.CreateReference(paymentReference);
        await _repositoryManager.SaveAsync();

        return paymentReference;
    }

    public async Task<Payment> Pay(Guid reference)
    { 

        var paymentRefence = await _repositoryManager.PaymentReference.GetPaymentReferenceAsync(reference, true, true);
        var existingPayment = await _repositoryManager.Payment.GetPaymentByRefIdAsync(paymentRefence.Id, false);
        if (existingPayment is not null && existingPayment.Status == PaymentStatus.Paid) return existingPayment;

        var order = paymentRefence.Order;
        await _service.OrderService.PlaceOrder(order);
        var payment = new Payment { Amount = order.TotalAmount, PaymentReferenceId = paymentRefence.Id };
        _repositoryManager.Payment.CreatePayment(payment);
        await _repositoryManager.SaveAsync();
        return payment;
    }
}
