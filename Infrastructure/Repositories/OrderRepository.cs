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

        public Task AddBoardComponentAsync(BoardComponent boardComponent)
        {
            throw new NotImplementedException();
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
