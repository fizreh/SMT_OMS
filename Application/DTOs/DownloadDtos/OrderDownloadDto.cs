using SMT.Application.DTOs;
using System;

namespace Application.DTOs.DownloadDtos
{
    public class OrderDownloadDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime OrderDate { get; set; }

        // Boards included in this order
        public List<BoardDownloadDto> Boards { get; set; } = new();
    }
}
