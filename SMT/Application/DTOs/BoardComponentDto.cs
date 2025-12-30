namespace SMT.Application.DTOs
{
    public class BoardComponentDto
    {
        public Guid ComponentId { get; set; }        // Reference to Component
        public string ComponentName { get; set; }    // Optional, for convenience
        public string ComponentDescription { get; set; } // Optional
        public int Quantity { get; set; }            // Quantity on this board
    }
}
