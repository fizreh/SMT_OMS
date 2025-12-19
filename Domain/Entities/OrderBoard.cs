namespace SMT.Domain.Entities
{
    public class OrderBoard
    {
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; }

        public Guid BoardId { get; private set; }
        public Board Board { get; private set; }

        private OrderBoard() { }

        public OrderBoard(Order order, Board board)
        {
            Order = order;
            Board = board;
            OrderId = order.Id;
            BoardId = board.Id;
        }
    }
}