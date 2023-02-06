namespace LibraryManagement.DTO
{
    public class OderDto
    {
        public int OrderId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime DueDate { get; set;}

        public bool Issubmitted { get; set; } = false;

        public int Quantity { get; set; }

        public int Penalty { get; set; } 

        public int UserMasterId { get; set; }

        public int BookId { get; set; }
    }
}
