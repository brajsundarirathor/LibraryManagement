using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class UserOrder
    {
        [Key]
        public int OrderId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime DueDate { get; set; }

        public int Quantity { get; set; }

        public bool IsSubmitted { get; set; } = false;

        public int BookId { get; set; }

        public int Penalty { get; set; }

        [ForeignKey("BookId")]
        public Book Book { get; set; }

        public int UserMasterId { get; set; }

        [ForeignKey("UserMasterId")]
        public UserMaster UserMaster { get; set;}

    }
}
