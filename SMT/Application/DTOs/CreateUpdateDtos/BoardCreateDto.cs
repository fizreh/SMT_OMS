using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.CreateDtos
{
    public class BoardCreateDto
    {
        public Guid BoardId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public List<ComponentCreateDto> Components { get; set; }
    }
}
