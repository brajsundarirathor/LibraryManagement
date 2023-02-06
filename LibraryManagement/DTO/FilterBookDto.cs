using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace LibraryManagement.DTO
{
    public class FilterBookDto
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Author { get; set; }

        public string Category { get; set; }

        public int Quantity { get; set; }

    }
}
