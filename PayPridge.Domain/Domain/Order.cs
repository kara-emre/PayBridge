using PayPridge.Application.DTOs;
using PayPridge.Domain.Enums;

namespace PayPridge.Domain.Domain
{
    public class Order
    {
        public Guid Id { get; private set; }
        public List<OrderItemDto> Items { get; private set; }
        public decimal TotalPrice { get; private set; }
        public OrderStatus Status { get; private set; }

        private Order() { }

        public Order(List<OrderItemDto> items)
        {
            Id = Guid.NewGuid();
            Items = items;
            TotalPrice = items.Sum(i => i.Price * i.Quantity);
            Status = OrderStatus.Pending;
        }

        public void MarkAsPaid()
        {
            Status = OrderStatus.Paid;
        }
    }
}
