using SMT.Application.Interfaces;
using SMT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderReadRepository _orderReadRepository;

        public OrderService(IOrderRepository orderRepository, IOrderReadRepository orderReadRepository)
        {
            _orderRepository = orderRepository;
            _orderReadRepository = orderReadRepository;
        }

        public async Task<Order> CreateOrderAsync(string name, string description, DateTime orderDate)
        {
            var order = new Order(name, description, orderDate);
            await _orderRepository.AddAsync(order);
            return order;
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        // Simulate download to production line
        public async Task<string> DownloadOrderAsync(Guid id)
        {
            var orderDownload = await _orderReadRepository.GetOrderForDownloadAsync(id);

            if (orderDownload == null) throw new Exception("Order not found");

            // Return serialized JSON string of order
            var orderJson = System.Text.Json.JsonSerializer.Serialize(orderDownload);
            return orderJson;
        }
    }
}