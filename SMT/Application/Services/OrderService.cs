using Application.DTOs.CreateDtos;
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

        
            var order = new Order(dto.Name, dto.Description, dto.OrderDate);
           

            foreach (var boardDto in dto.Boards)
            {
                // Fetch full board entity from DB
                var board = await _boardRepository.GetByIdAsync(boardDto.BoardId);
                if (board == null) throw new Exception($"Board {boardDto.BoardId} not found");
                var orderBoard = new OrderBoard(order, board);
                // Add board to order
                order.AddOrderBoard(orderBoard);

                // Add components
                foreach (var compDto in boardDto.Components)
                {
                    var component = await _componentRepository.GetByIdAsync(compDto.ComponentId);
                    if (component == null) throw new Exception($"Component {compDto.ComponentId} not found");

                    var existingBoardComponent = board.BoardComponents.FirstOrDefault(bc => bc.ComponentId == component.Id);

                    if (existingBoardComponent != null)
                    {
                        existingBoardComponent.Quantity = compDto.Quantity;
                    }
                    else
                    {
                        var boardComponent = new BoardComponent(board, component, compDto.Quantity);
                        board.AddBoardComponent(boardComponent);
                    }
                }
            }

            await _orderRepository.AddAsync(order);
            _logger.LogInformation("order created successfully");
            return order.Id;
        }







        public async Task<bool> UpdateOrderWithDetailsAsync(Guid orderId, OrderCreateDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                //Get existing order
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                    return false;

                //Update order core fields
               
                order.Update(dto.Name, dto.Description, dto.OrderDate);

                await _orderRepository.UpdateAsync(order);

                // Collect board IDs from request
                var boardIds = dto.Boards
                    .Select(b => b.BoardId)
                    .Distinct()
                    .ToList();

                // Remove existing relations
                await _orderRepository.DeleteOrderBoardAsync(orderId);
                await _orderRepository.DeleteBoardComponentsByBoardIdsAsync(boardIds);


                // Re-create relations
                foreach (var boardDto in dto.Boards)
                {
                    var orderBoard = order.OrderBoards.FirstOrDefault(ob => ob.BoardId == boardDto.BoardId);
                    if (orderBoard == null)
                    {
                        // Add new board to order
                        var board = await _boardRepository.GetByIdAsync(boardDto.BoardId);
                        if (board == null) throw new Exception($"Board {boardDto.BoardId} not found");

                        orderBoard = new OrderBoard(order, board);
                        order.AddOrderBoard(orderBoard);
                    }
                   
                        // 4. Loop through components
                        foreach (var compDto in boardDto.Components)
                        {
                            var existingBoardComponent = orderBoard.Board.BoardComponents
                                .FirstOrDefault(bc => bc.ComponentId == compDto.ComponentId);

                            if (existingBoardComponent != null)
                            {
                                // Update quantity
                                existingBoardComponent.Quantity = compDto.Quantity;
                            }
                            else
                            {
                                // Add new component to board
                                var component = await _componentRepository.GetByIdAsync(compDto.ComponentId);
                                if (component == null) throw new Exception($"Component {compDto.ComponentId} not found");

                                var boardComponent = new BoardComponent(orderBoard.Board, component, compDto.Quantity);
                                orderBoard.Board.AddBoardComponent(boardComponent);
                            }
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