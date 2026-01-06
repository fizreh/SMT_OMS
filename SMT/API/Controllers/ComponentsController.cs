using Application.DTOs.CreateDtos;
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



        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateComponent(Guid id, [FromBody] ComponentDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Component cannot be null.");
            }
            
            var isUpdated = await _componentService.UpdateComponentAsync(dto);

            if (!isUpdated)
                return NotFound("Component not updated");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _componentService.DeleteComponentAsync(id);
            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ComponentDto componentDto)
        {
            _logger.LogInformation("Create component request started for component {ComponentName}", componentDto.Name);
            var newComponent = new Component(componentDto.Name, componentDto.Description);
            var component = await _componentService.CreateComponentAsync(newComponent);

            if (component == null)
            {
                _logger.LogError("Component request failed for Component {ComponentName}", componentDto.Name);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // Map to DTO
            var createdComponent = new BoardDto
            {
                Id = component.Id,
                Name = component.Name,
                Description = component.Description
            };

            _logger.LogInformation("Create component request completed forComponent {ComponentName}", createdComponent.Name);

            return CreatedAtAction(nameof(GetAll), new { id = component.Id }, componentDto);
        }
    }
}

