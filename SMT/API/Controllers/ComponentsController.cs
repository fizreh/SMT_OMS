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
    public class ComponentsController : ControllerBase
    {
        private readonly ComponentService _componentService;
        private readonly ILogger<OrdersController> _logger;


        public ComponentsController(
         ComponentService componentService,
         ILogger<OrdersController> logger
         )
        {
            _componentService = componentService;
            _logger = logger;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var components = await _componentService.GetAllComponentsAsync();

            if (components == null)
            {
                return NotFound("No component found!");
            }
            // Map entities to DTOs
            var result = components.Select(c => new ComponentDto
            {
               Id = c.Id,
               Name = c.Name,
               Description = c.Description
            }).ToList();

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ComponentDto componentDto)
        {
            _logger.LogInformation("Create order request started for Order {BoardName}", componentDto.Name);
            var component = await _componentService.CreateComponentAsync(

                componentDto.Name,
                componentDto.Description
            );

            if (component == null)
            {
                _logger.LogError("Board request failed for Board {OrderName}", componentDto.Name);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Map to DTO
            var createdComponent = new BoardDto
            {
                Id = component.Id,
                Name = component.Name,
                Description = component.Description
            };

            _logger.LogInformation("Create component request completed for Order {ComponentName}", createdComponent.Name);

            return CreatedAtAction(nameof(GetAll), new { id = component.Id }, componentDto);
        }
    }
}

