using SMT.Application.Interfaces;
using SMT.Domain.Entities;
using SMT.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SMTDbContext _context;

        public OrderRepository(SMTDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
            {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }


        public async Task AddOrderBoardAsync(OrderBoard orderBoard)
        {
            _context.OrderBoards.Add(orderBoard);
            await _context.SaveChangesAsync();

        }

        public  Task DeleteOrderBoardAsync(Guid orderId)
        {
            var orderBoards = _context.OrderBoards.Where(ob => ob.OrderId == orderId);
            _context.OrderBoards.RemoveRange(orderBoards);
            return Task.CompletedTask;

        }


        public Task DeleteBoardComponentsByBoardIdsAsync(IEnumerable<Guid> boardIds)
        {
          
            var boardComponents = _context.BoardComponents.Where(bc => boardIds.Contains(bc.BoardId));
            _context.BoardComponents.RemoveRange(boardComponents);
            return Task.CompletedTask;

        }

        public async Task AddBoardComponentAsync(BoardComponent boardComponent)
        {
            _context.BoardComponents.Add(boardComponent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderBoards)
                .ThenInclude(ob => ob.Board)
                .ThenInclude(o => o.BoardComponents)
                .ThenInclude(ob => ob.Component)
                .ToListAsync();
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            return await _context.Orders
                .Include(o => o.OrderBoards)
                .ThenInclude(ob => ob.Board)
                .ThenInclude(o => o.BoardComponents)
                .ThenInclude(ob => ob.Component)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

       

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        
    }
}
