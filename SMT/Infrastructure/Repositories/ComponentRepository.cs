using SMT.Application.Interfaces;
using SMT.Domain.Entities;
using SMT.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories
{
    public class ComponentRepository : IComponentRepository
    {
        private readonly SMTDbContext _context;

        public ComponentRepository(SMTDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Component component)
        {
            _context.Components.Add(component);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var component = await _context.Components.FindAsync(id);
            if (component != null)
            {
                _context.Components.Remove(component);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Component>> GetAllAsync()
        {
            return await _context.Components
                .ToListAsync();
        }

        public async Task<Component> GetByIdAsync(Guid id)
        {
            return await _context.Components
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task UpdateAsync(Component component)
        {
            _context.Components.Update(component);
            await _context.SaveChangesAsync();
        }
    }
}

