using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTOs;
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
                return NotFound("orders not found");
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
            if (order == null) return NotFound();
            return Ok(order);
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
            return Ok(new { OrderJson = json });
        }
    }
}

