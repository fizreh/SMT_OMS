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
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
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
            var order = await _orderService.CreateOrderAsync(
                orderCreateDto.Name,
                orderCreateDto.Description,
                orderCreateDto.OrderDate
            );

            if (order == null)
            {
                return NotFound("Order not dound");
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

            return CreatedAtAction(nameof(Get), new { id = order.Id }, orderDto);
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
            var json = await _orderService.DownloadOrderAsync(id);

            if(json == null)
            {
                return NotFound("Order not found");
            }

            return Ok(new { OrderJson = json });
        }
    }
}

