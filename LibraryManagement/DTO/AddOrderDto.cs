namespace LibraryManagement.DTO
{
    public class AddOrderDto
    {
        public DateTime CreatedDate { get; set; }

        public int Quantity { get; set; }

        public bool Issubmitted { get; set; } = false;

        public int UserMasterId { get; set; }

        public int BookId { get; set; }
    }
}
