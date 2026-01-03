using System.Collections.Generic;

namespace SMT.Domain.Entities
{
    public class Board : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public double Length { get; private set; }
        public double Width { get; private set; }

        private readonly List<OrderBoard> _orderBoards = new();
        public IReadOnlyCollection<OrderBoard> OrderBoards => _orderBoards.AsReadOnly();

        private readonly List<BoardComponent> _boardComponents = new();
        public IReadOnlyCollection<BoardComponent> BoardComponents => _boardComponents.AsReadOnly();

        public void AddBoardComponent(BoardComponent bc)
        {
            _boardComponents.Add(bc);
        }

        private Board() { }

        public Board(string name, string description, double length, double width)
        {
            Name = name;
            Description = description;
            Length = length;
            Width = width;
        }
    }
}