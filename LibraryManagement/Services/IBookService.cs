using LibraryManagement.DTO;
using LibraryManagement.Models;

namespace LibraryManagement.Services
{
    public interface IBookService
    {
        Task<ServiceResponse<List<GetBookDto>>> GetAllBooks();

        Task<ServiceResponse<GetBookDto>> GetBookbyId(int id);

        //Task<ServiceResponse<Book>> GetBookData(int bookId);

        Task<ServiceResponse<List<GetBookDto>>> AddBook(AddBookDto book);

        Task<ServiceResponse<GetBookDto>> UpdateBook(UpdateBookDto book);

        Task<ServiceResponse<List<GetBookDto>>> DeleteBook(int id);

        Task<ServiceResponse<List<FilterBookDto>>> FilterBook(string bookname, string authorname, string category);

        Task<ServiceResponse<List<FilterBookDto>>> Searchbook(decimal price);

        Task<ServiceResponse<List<FilterBookDto>>> SortingbyPrice();

        Task<ServiceResponse<List<FilterBookDto>>> SortingbyPriceDecs();

        Task<string> UploadImage(IFormFile file);
    }
}
