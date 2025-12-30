using System;

namespace SMT.Application.Models
{
    public class OrderCreateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime OrderDate { get; set; }
    }
}