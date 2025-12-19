namespace SMT.Application.DTOs
{
    public class BoardDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }

        // Components on this board
        public List<ComponentDto> Components { get; set; } = new();
    }
}
