﻿namespace LibraryManagement.DTO
{
    public class UpdateBookDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Author { get; set; }

        public int Quantity { get; set; }

        public string Category { get; set; }

        public string CoverFileName { get; set; }
    }
}
