using Application.DTOs.CreateDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTOs;
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
       
        public async Task<IActionResult> Get(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound("Order not dound");
            }

            var model = new Application.DTOs.OrderDto
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

      

        [HttpPost("full")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateFullOrder(
         [FromBody] OrderCreateDto dto)
        {
            _logger.LogInformation("Creating full order {OrderName}", dto.Name);

            var orderId = await _orderService.CreateOrderWithDetailsAsync(dto);

            return CreatedAtAction(nameof(Get), new { id = orderId }, new { id = orderId });
        }

        [HttpPut("{id}/full")]
        public async Task<IActionResult> UpdateFullOrder(Guid id, [FromBody] OrderCreateDto dto)
        {
            var updatedOrderId = await _orderService.UpdateOrderWithDetailsAsync(id, dto);

            if (updatedOrderId == null)
                return NotFound("Order not found");

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

