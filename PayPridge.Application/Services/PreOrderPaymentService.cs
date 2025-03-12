using MassTransit;
using PayPridge.Application.DTOs;
using PayPridge.Application.Interfaces;
using PayPridge.Domain.Domain;
using PayPridge.Domain.Events;

namespace PayPridge.Application.Services
{
    public class PreOrderPaymentService : IOrderPaymentService
    {
        private readonly IRequestClient<CreatePreOrderPaymentCommand> _createPreOrderPaymentClient;
        private readonly IRequestClient<CompleteOrderPaymentCommand> _completeOrderPaymentClient;
        private readonly IProductService _productService;

        // Constructor'a hem CreatePreOrderPaymentCommand hem de CompleteOrderPaymentCommand için IRequestClient ekleyin
        public PreOrderPaymentService(IRequestClient<CreatePreOrderPaymentCommand> createPreOrderPaymentClient,
                                      IRequestClient<CompleteOrderPaymentCommand> completeOrderPaymentClient,
                                      IProductService productService)
        {
            _createPreOrderPaymentClient = createPreOrderPaymentClient;
            _completeOrderPaymentClient = completeOrderPaymentClient;
            _productService = productService;
        }

        public async Task<OrderPaymentConfirmation> CompleteOrderAsync(Guid id)
        {
            var completeOrderPayment = new CompleteOrderPaymentCommand(id);

            var response = await _completeOrderPaymentClient.GetResponse<OrderPaymentConfirmation>(completeOrderPayment);

            return response.Message;

        }
        public async Task<OrderPaymentConfirmation> CreateOrderAsync(OrderRequest request)
        {
            if (request == null || request.Products == null || !request.Products.Any())
            {
                return new OrderPaymentConfirmation(Guid.Empty, false, "Invalid request or products are empty");
            }

            decimal productPriceTotal = 0;

            foreach (var product in request.Products)
            {
                if (product.Quantity <= 0)
                {
                    return new OrderPaymentConfirmation(Guid.Empty, false, $"{product.ProductId }Invalid product quantity");
                }

                var productControl = await _productService.FindAsync(product.ProductId);

                if (productControl == null)
                {
                    return new OrderPaymentConfirmation(Guid.Empty, false, $"{product.ProductId} Product is not found");
                }
                if (productControl.Stock < product.Quantity)
                {
                    return new OrderPaymentConfirmation(Guid.Empty, false, $"{product.ProductId} Stock is not enough");
                }
                if (productControl.Price != product.Price)
                {
                    return new OrderPaymentConfirmation(Guid.Empty, false, $"{product.ProductId} Product price does not match");
                }

                productPriceTotal += productControl.Price * product.Quantity;
            }

            if (request.TotalAmount <= 0)
            {
                return new OrderPaymentConfirmation(Guid.Empty, false, "Invalid total amount");
            }

            if (request.TotalAmount != productPriceTotal)
            {
                return new OrderPaymentConfirmation(Guid.Empty, false, "Total amount is not correct");
            }

            var order = new Order(request.Products);

            var createPreOrderPayment = new CreatePreOrderPaymentCommand(order.Id, request.Products, order.TotalPrice);

            var response = await _createPreOrderPaymentClient.GetResponse<OrderPaymentConfirmation>(createPreOrderPayment);

            return response.Message;
        }

    }

}
