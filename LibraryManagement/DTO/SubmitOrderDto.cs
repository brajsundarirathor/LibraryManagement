namespace LibraryManagement.DTO
{
    public class SubmitOrderDto
    {
        public int OrderId { get; set; }

        public DateTime DueDate { get; set; }

        public int Quantity { get; set; }

        public bool Issubmitted { get; set; } = false;

        public int Penalty { get; set; }

        public int UserMasterId { get; set; }

        public int BookId { get; set; }
    }
}
