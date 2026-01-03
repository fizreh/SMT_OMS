using SMT.Domain.Entities;

namespace SMT.Domain.Entities
{
    public class BoardComponent
{
    public Guid BoardId { get; private set; }
    public Board Board { get; private set; }

    public Guid ComponentId { get; private set; }
    public Component Component { get; private set; }

    public int Quantity { get;  set; }

    private BoardComponent() { }

    public BoardComponent(Board board, Component component, int quantity)
    {
        Board = board;
        Component = component;
        Quantity = quantity;

        BoardId = board.Id;
        ComponentId = component.Id;
    }
}
}
