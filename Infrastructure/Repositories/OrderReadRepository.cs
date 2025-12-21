using Application.DTOs.DownloadDtos;
using Microsoft.EntityFrameworkCore;
using SMT.Application.DTOs;
using SMT.Application.Interfaces;
using SMT.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories
{
    public class OrderReadRepository : IOrderReadRepository
    {
        public readonly SMTDbContext _context;
        public OrderReadRepository(SMTDbContext context) 
        {
            _context = context;
        
        }
        public async Task<OrderDownloadDto?> GetOrderForDownloadAsync(Guid orderId)
        {
            if(orderId == Guid.Empty)
            {  
                throw new ArgumentNullException(nameof(orderId));
            }

            return await _context.Orders
                .Where(o => o.Id == orderId)
                .Select(o => new OrderDownloadDto
                {
                    Name = o.Name,
                    Description = o.Description,
                    OrderDate = o.OrderDate,
                    Boards = o.OrderBoards.Select(ob => new BoardDownloadDto{
                      Name = ob.Board.Name,
                      Description=ob.Board.Description,
                      Length = ob.Board.Length,
                      Width = ob.Board.Width,
                      Components = ob.Board.BoardComponents.Select(bc => new ComponentDownloadDto
                      {
                         Name = bc.Component.Name,
                         Description = bc.Component.Description,
                         Quantity = bc.Quantity
                      }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}
