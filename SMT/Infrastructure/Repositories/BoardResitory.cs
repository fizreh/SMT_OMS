using SMT.Application.Interfaces;
using SMT.Domain.Entities;
using SMT.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly SMTDbContext _context;

        public BoardRepository(SMTDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Board board)
        {
            _context.Boards.Add(board);
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

        public async Task<IEnumerable<Board>> GetAllAsync()
        {
            return await _context.Boards
                .Include(o => o.OrderBoards)
                .ThenInclude(ob => ob.Order)
                .ToListAsync();
        }

        public async Task<Board> GetByIdAsync(Guid id)
        {
            return await _context.Boards
                .Include(o => o.OrderBoards)
                .ThenInclude(ob => ob.Order)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task UpdateAsync(Board board)
        {
            _context.Boards.Update(board);
            await _context.SaveChangesAsync();
        }
    }
}
