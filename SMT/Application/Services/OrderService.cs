using Application.DTOs.CreateDtos;
using Microsoft.Extensions.Logging;
using SMT.Application.DTOs;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository,
        IOrderReadRepository orderReadRepository,
        IBoardRepository boardRepository,
        IComponentRepository componentRepository,
        IUnitOfWork unitOfWork,
        ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _orderReadRepository = orderReadRepository;
            _boardRepository = boardRepository;
            _componentRepository = componentRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }



        public async Task<Guid> CreateOrderWithDetailsAsync(OrderCreateDto dto)
        {

            if(dto == null)
            {
                _logger.LogWarning("Order is emmpty");
                throw new ArgumentNullException(nameof(dto));
            }

            _logger.LogInformation("creating order started");

            // 1. Create Order
            var order = new Order(dto.Name, dto.Description, dto.OrderDate);
            await _orderRepository.AddAsync(order);

            // 2. Attach Boards
            foreach (var boardDto in dto.Boards)
            {
                var board = new Board(boardDto.Name, boardDto.Description, boardDto.Length, boardDto.Width);
                var orderBoard = new OrderBoard(order, board);
                await _orderRepository.AddOrderBoardAsync(orderBoard);

                // 3. Attach Components
                foreach (var componentDto in boardDto.Components)
                {
                    var component = new Component(componentDto.Name, componentDto.Description);
                    var boardComponent = new BoardComponent(board,component,componentDto.Quantity);

                    await _orderRepository.AddBoardComponentAsync(boardComponent);
                }
            }

            _logger.LogInformation("order created successfully");

            return order.Id;
        }


        public async Task<bool> UpdateOrderWithDetailsAsync(Guid orderId, OrderCreateDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 1. Get existing order
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                    return false;

                // 2. Update order core fields
                order.Name = dto.Name;
                order.Description = dto.Description;
                order.OrderDate = dto.OrderDate;

                await _orderRepository.UpdateAsync(order);

                // 3. Collect board IDs from request
                var boardIds = dto.Boards
                    .Select(b => b.BoardId)
                    .Distinct()
                    .ToList();

                // 4. Remove existing relations
                await _orderRepository.DeleteOrderBoardAsync(orderId);
                await _orderRepository.DeleteBoardComponentsByBoardIdsAsync(boardIds);
                

                // 5. Re-create relations
                foreach (var newBoard in dto.Boards)
                {
                    var updatedBoard = new Board(newBoard.Name, newBoard.Description, newBoard.Length, newBoard.Width);
                    var updatedOrder = new Order(dto.Name, dto.Description,dto.OrderDate);
                    var updatedOrderBoard = new OrderBoard(updatedOrder, updatedBoard);
                    await _orderRepository.AddOrderBoardAsync(updatedOrderBoard);

                    foreach (var component in newBoard.Components)
                    {
                        var updatedComponent = new Component(component.Name, component.Description);
                        var updatedBoardComponent= new BoardComponent(updatedBoard,updatedComponent,component.Quantity);
                        await _orderRepository.AddBoardComponentAsync(
                            updatedBoardComponent
                        );
                    }
                }

                // 6. Commit transaction
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
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