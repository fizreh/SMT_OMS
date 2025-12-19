using SMT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces
{
    public interface IComponentRepository
    {
        Task<Component> GetByIdAsync(Guid id);
        Task<IEnumerable<Component>> GetAllAsync();
        Task AddAsync(Component component);
        Task UpdateAsync(Component component);
        Task DeleteAsync(Guid id);
    }
}
