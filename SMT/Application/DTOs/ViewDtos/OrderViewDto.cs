using Application.DTOs.CreateDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ViewDtos
{
    public class OrderViewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public List<BoardViewDto> Boards { get; set; }
    }
}
