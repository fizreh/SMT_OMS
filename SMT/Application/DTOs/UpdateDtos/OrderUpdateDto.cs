using System;

namespace Application.DTOs.CreateDtos
{
    public class OrderUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public List<BoardCreateDto> Boards { get; set; }
    }


}