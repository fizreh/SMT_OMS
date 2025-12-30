using System;

namespace SMT.Application.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime OrderDate { get; set; }

        // Boards included in this order
        public List<BoardDto> Boards { get; set; } = new();
    }
}