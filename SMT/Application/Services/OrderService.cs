using Microsoft.Extensions.Logging;
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
        private readonly IBoardRepository _boardRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, 
        IOrderReadRepository orderReadRepository,
        IBoardRepository boardRepository,
        IComponentRepository componentRepository,
        ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _orderReadRepository = orderReadRepository;
            _boardRepository = boardRepository;
            _componentRepository = componentRepository;
            _logger = logger;
        }

        public async Task<Order> CreateOrderAsync(string name, string description, DateTime orderDate)
        {
            _logger.LogInformation("Starting creating the Order {OrderName}", name);

            var order = new Order(name, description, orderDate);

            await _orderRepository.AddAsync(order);

            _logger.LogInformation("Starting creating the Order {OrderName}", name);

            return order;
        }

        public async Task AddBoardToOrderAsync(Guid orderId, Guid boardId)
        {
            
            if (orderId == Guid.Empty)
            {
                _logger.LogError("Order {OrderId} is Empty When Adding board to order", orderId);
                throw new ArgumentException("OrderId cannot be empty");
            }
                
            if (boardId == Guid.Empty)
            {
                _logger.LogError("Board {BoardId} is Empty When Adding board to order", boardId);
                throw new ArgumentException("BoardId cannot be empty");
            }
           
            var order = await GetOrderByIdAsync(orderId);
            if (order == null)
            {
                _logger.LogWarning("OrderId {OrderId} not found When Adding board to order", orderId);
                throw new KeyNotFoundException("Order not found.");
            }
               
            var board = await _boardRepository.GetByIdAsync(boardId);
            if (board == null)
            {
                _logger.LogWarning("BoardId {BoardId} not found When Adding board to order", boardId);
                throw new KeyNotFoundException($"Board not found.");
            }
               

            //Check if the link already exists
            bool exists = order.OrderBoards.Any(ob => ob.BoardId == boardId);
            if (exists)
            {
                _logger.LogWarning(" Order {OrderName} has already the board {BoardName}", order.Name, board.Name);
                throw new InvalidOperationException("Board is already attached to this order.");
            }


            //Create the link
            _logger.LogInformation("Linkinf b/w order and board started");
            var orderBoard = new OrderBoard(order, board);

            // Persist
            await _orderRepository.AddOrderBoardAsync(orderBoard);
            _logger.LogInformation("Linkinf b/w order and board completed");
        }

        public async Task AddComponentToBoardAsync(Guid boardId, Guid componentId, int quantity)
        {
            if (boardId == Guid.Empty)
            {
                _logger.LogError("Board {BoardId} is Empty When Adding board to order", boardId);
                throw new ArgumentException("BoardId cannot be empty");
            }
                

            if (componentId == Guid.Empty)
            {
                _logger.LogError("Component {ComponentId} is Empty When Adding component to board", componentId);
                throw new ArgumentException("ComponentId cannot be empty");
            }
               

            if (quantity <= 0)
            {
                _logger.LogError("Quantity {Quantity} should be greater than 0", quantity);
                throw new ArgumentException("Quantity must be greater than 0");
            }
                

            // Check if board exists
            var board = await _boardRepository.GetByIdAsync(boardId);
            if (board == null)
            {
                _logger.LogWarning("Board not found When Adding board to order");
                throw new KeyNotFoundException($"Board with Id {boardId} not found.");
            }
               

            // Check if component exists
            var component = await _componentRepository.GetByIdAsync(componentId);
            if (component == null)
            {
                _logger.LogWarning("Component not found When Adding component to board");
                throw new KeyNotFoundException($"Component with Id {componentId} not found.");
            }
                

            // Check if the link already exists
            var exists = board.BoardComponents.Any(bc => bc.ComponentId == componentId);
            if (exists)
            {
                _logger.LogWarning(" Board {BoardName} has already the component {ComponentName}", board.Name, component.Name);
                throw new InvalidOperationException("Component already exists on this board.");
            }


            // Create BoardComponent
            _logger.LogInformation("Started Adding component to the board with quantity");
            var boardComponent = new BoardComponent(board, component, quantity);

            // Persist
            await _orderRepository.AddBoardComponentAsync(boardComponent);
            _logger.LogInformation("Started Adding component to the board with quantity");
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("Order {OrderId} not found when GETTING ORDER", id);
                throw new ArgumentNullException("Id not found");
            } 
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
            if (id == Guid.Empty)
            {
                _logger.LogError("Order {OrderId} not found when DELETING ORDER", id);
                throw new ArgumentNullException("Id not found");
            }
            await _orderRepository.DeleteAsync(id);
        }

        // Simulate download to production line
        public async Task<string> DownloadOrderAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("Order {OrderId} not found when DOWNLOADING ORDER", id);
                throw new ArgumentException("Order Id cannot be empty");
            }

            try
            {
                _logger.LogInformation("Starting downloading the SMT production line order {OrderId}", id);

                var orderDownload = await _orderReadRepository.GetOrderForDownloadAsync(id);

                if (orderDownload == null)
                {
                    _logger.LogWarning("Cannot found the order {OrderId} for the download", id);
                    throw new KeyNotFoundException($"Order {id} not found");
                }


                // Return serialized JSON string of order
                var orderJson = System.Text.Json.JsonSerializer.Serialize(orderDownload);

                _logger.LogInformation("order {OrderId} downloaded in JSON format", id);

                return orderJson;

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unhandled error while downloading Order {OrderId}", id);
                throw;

            }
           
        }
    }
}