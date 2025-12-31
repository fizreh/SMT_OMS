using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTOs;
using SMT.Application.Models;
using SMT.Application.Services;
using System;
using System.Threading.Tasks;

namespace SMT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   // [Authorize(AuthenticationSchemes = "Firebase")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly ILogger<OrdersController> _logger;
        

        public OrdersController(
         OrderService orderService, 
         ILogger<OrdersController> logger
         )
        {
            _orderService = orderService;
            _logger = logger;
            
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            if (orders == null)
            { 
                return NotFound("No orders found!");
            }
            // Map entities to DTOs
            var result = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                Name = o.Name,
                Description = o.Description,
                OrderDate = o.OrderDate,
                Boards = o.OrderBoards.Select(ob => new BoardDto
                {
                    Id = ob.Board.Id,
                    Name = ob.Board.Name,
                    Description = ob.Board.Description,
                    Length = ob.Board.Length,
                    Width = ob.Board.Width,
                    Components = ob.Board.BoardComponents.Select(bc => new ComponentDto
                    {
                        Id = bc.Component.Id,
                        Name = bc.Component.Name,
                        Description = bc.Component.Description,
                        Quantity = bc.Quantity
                    }).ToList()
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound("Order not dound");
            }

            var model = new OrderDto
            {
                Id = order.Id,
                Name = order.Name,
                Description = order.Description,
                OrderDate = order.OrderDate,
                Boards = order.OrderBoards.Select(ob => new BoardDto
                {
                    Id = ob.Board.Id,
                    Name = ob.Board.Name,
                    Description = ob.Board.Description,
                    Length = ob.Board.Length,
                    Width = ob.Board.Width,
                    Components = ob.Board.BoardComponents.Select(bc => new ComponentDto
                    {
                        Id = bc.Component.Id,
                        Name = bc.Component.Name,
                        Description = bc.Component.Description,
                        Quantity = bc.Quantity
                    }).ToList()
                }).ToList()
            };

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto orderCreateDto)
        {

            _logger.LogInformation("Create order request started for Order {OrderName}", orderCreateDto.Name);
            var order = await _orderService.CreateOrderAsync(
                orderCreateDto.Name,
                orderCreateDto.Description,
                orderCreateDto.OrderDate
            );

            if (order == null)
            {
                _logger.LogError("Order request failed for Order {OrderName}", orderCreateDto.Name);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Map to DTO
            var orderDto = new OrderDto
            {
                Id = order.Id,
                Name = order.Name,
                Description = order.Description,
                OrderDate = order.OrderDate,
                Boards = order.OrderBoards.Select(ob => new BoardDto
                {
                    Id = ob.Board.Id,
                    Name = ob.Board.Name,
                    Description = ob.Board.Description,
                    Length = ob.Board.Length,
                    Width = ob.Board.Width,
                    Components = ob.Board.BoardComponents.Select(bc => new ComponentDto
                    {
                        Id = bc.Component.Id,
                        Name = bc.Component.Name,
                        Description = bc.Component.Description,
                        Quantity = bc.Quantity
                    }).ToList()
                }).ToList()
            };

            _logger.LogInformation("Create order request completed for Order {OrderName}", orderCreateDto.Name);

            return CreatedAtAction(nameof(Get), new { id = order.Id }, orderDto);
        }

        [HttpPost("{orderId}/boards")]
        public async Task<IActionResult> AddBoardToOrder(Guid orderId,[FromBody] AddBoardToOrderDto dto)
        {
            await _orderService.AddBoardToOrderAsync(orderId, dto.BoardId);

            _logger.LogInformation("Board {BoardId} added to Order {OrderId}",dto.BoardId, orderId);

            return NoContent();
        }


        [HttpPost("{boardId}/components")]
        public async Task<IActionResult> AddComponentToBoard(Guid boardId, [FromBody] AddComponentToBoardDto dto)
        {
            await _orderService.AddComponentToBoardAsync(boardId, dto.ComponentId, dto.Quantity);

            _logger.LogInformation("Component {ComponentId} added to Board {BoardId}", dto.ComponentId, boardId);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderCreateDto orderCreateDto)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound("Order doesnt exist!");
            }

            // Update entity via service if the order exists
            order = new SMT.Domain.Entities.Order(
                orderCreateDto.Name,
                orderCreateDto.Description,
                orderCreateDto.OrderDate
            );

            await _orderService.UpdateOrderAsync(order);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/download")]
        public async Task<IActionResult> Download(Guid id)
        {
            _logger.LogInformation("API request: Download Order {OrderId}", id);
            
            var jsonOrder = await _orderService.DownloadOrderAsync(id);

            if(jsonOrder == null)
            {
                return NotFound("Order not found");
            }

            return Ok(jsonOrder);
        }
    }
}

