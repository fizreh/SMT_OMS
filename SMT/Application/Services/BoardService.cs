using SMT.Application.Interfaces;
using SMT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Application.Services
{
    public class BoardService
    {
        private readonly IBoardRepository _boardRepository;

        public BoardService(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        public async Task<Board> CreateBoardAsync(string name, string description, double length, double width)
        {
            var board = new Board(name, description,length,width);
            await _boardRepository.AddAsync(board);
            return board;
        }

        public async Task<Board> GetBoardByIdAsync(Guid id)
        {
            return await _boardRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Board>> GetAllBoardsAsync()
        {
            return await _boardRepository.GetAllAsync();
        }

        public async Task UpdateBoardAsync(Board board)
        {
            await _boardRepository.UpdateAsync(board);
        }

        public async Task DeleteBoardAsync(Guid id)
        {
            await _boardRepository.DeleteAsync(id);
        }

        // Simulate download to production line
        public async Task<string> DownloadBoardAsync(Guid id)
        {
            var board = await _boardRepository.GetByIdAsync(id);
            if (board == null) throw new Exception("Board not found");

            // Return serialized JSON string of board
            var boardJson = System.Text.Json.JsonSerializer.Serialize(board);
            return boardJson;
        }
    }
}