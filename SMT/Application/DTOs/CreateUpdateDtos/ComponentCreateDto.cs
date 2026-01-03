using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.CreateDtos
{
    public class ComponentCreateDto
    {
        public Guid ComponentId { get; set; }
        public int Quantity { get; set; }
    }
}
