using SMT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task AddAsync(Order order);
        Task AddOrderBoardAsync(OrderBoard board);
        Task AddBoardComponentAsync(BoardComponent boardComponent);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Guid id);

        Task DeleteOrderBoardAsync(Guid id);
        Task DeleteBoardComponentsByBoardIdsAsync(IEnumerable<Guid> ids);

    }
}
