using Application.DTOs.DownloadDtos;



namespace SMT.Application.Interfaces
{
    public interface IOrderReadRepository
    {
        Task<OrderDownloadDto?> GetOrderForDownloadAsync(Guid orderId);
    }
}
