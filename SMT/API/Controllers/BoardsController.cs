using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMT.Application.DTOs;
using SMT.Application.Services;
using SMT.Domain.Entities;
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _boardService.DeleteBoardAsync(id);
            return NoContent();
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateBoard([FromBody] BoardDto dto)
        {
            if(dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Board cannot be null.");
            }
        
            var isUpdated = await _boardService.UpdateBoardAsync(dto);

            if (!isUpdated)
                return NotFound("Board not updated");

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BoardDto boardDto)
        {
            _logger.LogInformation("Create board request started for Board {BoardName}", boardDto.Name);
            var newBoard = new Board(boardDto.Name, boardDto.Description, boardDto.Length, boardDto.Width);
            var board = await _boardService.CreateBoardAsync(newBoard);

            if (board== null)
            {
                _logger.LogError("Board request failed for Board {BoardName}", boardDto.Name);
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

            _logger.LogInformation("Create board request completed for Board {BoardName}", board.Name);

            return CreatedAtAction(nameof(GetAll), new { id = board.Id }, boardDto);
        }
    }
}

