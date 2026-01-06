using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.CreateDtos
{
    public class ComponentViewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Quantity { get; set; }
       
       
    }
}
