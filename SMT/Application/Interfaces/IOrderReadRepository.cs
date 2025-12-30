using Application.DTOs.DownloadDtos;
using SMT.Application.Models;


namespace SMT.Application.Interfaces
{
    public interface IOrderReadRepository
    {
        Task<OrderDownloadDto?> GetOrderForDownloadAsync(Guid orderId);
    }
}
