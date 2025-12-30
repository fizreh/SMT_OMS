using Application.DTOs.DownloadDtos;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SMT.Application.DTOs;
using SMT.Application.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

public class OrderServiceUnitTests
{
    [Fact]
    public async Task DownloadOrderAsyncTest()
    {
        // Arrange
        var fakeOrder = new OrderDownloadDto
        {
            Name = "UnitTest Order",
            Description = "Test Order",
            OrderDate = DateTime.UtcNow,
            Boards = new System.Collections.Generic.List<BoardDownloadDto>
            {
                new BoardDownloadDto
                {
                    Name = "Test Board",
                    Description = "Board Description",
                    Length = 100,
                    Width = 80,
                    Components = new System.Collections.Generic.List<ComponentDownloadDto>
                    {
                        new ComponentDownloadDto { Name = "Resistor", Description = "10k Ohm", Quantity = 10 }
                    }
                }
            }
        };

        var mockOrderRepo = new Mock<SMT.Application.Interfaces.IOrderRepository>();
        var mockBoardRepo = new Mock<SMT.Application.Interfaces.IBoardRepository>();
        var mockComponentRepo = new Mock<SMT.Application.Interfaces.IComponentRepository>();
        var mockOrderReadRepo = new Mock<SMT.Application.Interfaces.IOrderReadRepository>();
        var logger = Mock.Of<ILogger<OrderService>>();
        mockOrderReadRepo.Setup(r => r.GetOrderForDownloadAsync(It.IsAny<Guid>()))
                         .ReturnsAsync(fakeOrder);

        var service = new OrderService(
            mockOrderRepo.Object, 
            mockOrderReadRepo.Object,
            mockBoardRepo.Object,
            mockComponentRepo .Object,
            logger);

        // Act
        var json = await service.DownloadOrderAsync(Guid.NewGuid());

        // Assert
        json.Should().NotBeNullOrEmpty();
        var orderDto = JsonSerializer.Deserialize<OrderDownloadDto>(json);
        orderDto.Name.Should().Be("UnitTest Order");
        orderDto.Boards[0].Components[0].Quantity.Should().Be(10);
    }
}