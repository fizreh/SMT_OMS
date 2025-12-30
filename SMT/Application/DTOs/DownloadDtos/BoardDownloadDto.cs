namespace SMT.Application.DTOs
{
    public class BoardDownloadDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }

        // Components on this board
        public List<ComponentDownloadDto> Components { get; set; } = new();
    }
}
