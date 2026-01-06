using SMT.Application.DTOs;
using SMT.Application.Interfaces;
using SMT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public async Task<Board> CreateBoardAsync(Board board)
        {
            if(board == null)
            {
                throw new ArgumentNullException(nameof(board));
            }
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

        public async Task<bool> UpdateBoardAsync(BoardDto board)
        {
            try
            {
                if (board == null)
                {
                    return false;
                }
                var existingBoard = await _boardRepository.GetByIdAsync(board.Id);
                if (existingBoard == null)
                {
                    return false;
                }
                existingBoard.Update(board.Name, board.Description, board.Width, board.Length);
               

                await _boardRepository.UpdateAsync(existingBoard);
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception();
                

            }
        }
        public async Task<bool> DeleteBoardAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return false;
            }
            await _boardRepository.DeleteAsync(id);
            return true;
        }


    }
}