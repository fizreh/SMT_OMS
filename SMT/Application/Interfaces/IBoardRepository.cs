using SMT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces
{
    public interface IBoardRepository
    {
        Task<Board> GetByIdAsync(Guid id);
        Task<IEnumerable<Board>> GetAllAsync();
        Task AddAsync(Board board);
        Task UpdateAsync(Board board);
        Task DeleteAsync(Guid id);
    }
}
