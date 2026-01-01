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
    [Authorize(AuthenticationSchemes = "Firebase")]
    public class BoardsController : ControllerBase
    {
        private readonly BoardService _boardService;
        private readonly ILogger<OrdersController> _logger;


        public BoardsController(
         BoardService boardService,
         ILogger<OrdersController> logger
         )
        {
            _boardService = boardService;
            _logger = logger;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var boards = await _boardService.GetAllBoardsAsync();

            if (boards == null)
            {
                return NotFound("No boards found!");
            }
            // Map entities to DTOs
            var result = boards.Select(b => new BoardDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                Length = b.Length,
                Width = b.Width,
            }).ToList();

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BoardDto boardDto)
        {
            _logger.LogInformation("Create order request started for Order {BoardName}", boardDto.Name);
            var board = await _boardService.CreateBoardAsync(

                boardDto.Name,
                boardDto.Description,
                boardDto.Length,
                boardDto.Width
            );

            if (board== null)
            {
                _logger.LogError("Board request failed for Board {OrderName}", boardDto.Name);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Map to DTO
            var createdBoard = new BoardDto
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                Length = board.Length,
                Width = board.Width
            };

            _logger.LogInformation("Create order request completed for Order {BoardName}", board.Name);

            return CreatedAtAction(nameof(GetAll), new { id = board.Id }, boardDto);
        }
    }
}

