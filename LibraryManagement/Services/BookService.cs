using AutoMapper;
using LibraryManagement.Data;
using LibraryManagement.DTO;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace LibraryManagement.Services
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;
        private readonly BookDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContext;
        readonly string coverImageFolderPath = string.Empty;

        public BookService(IMapper mapper, BookDbContext context, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContext)
        {
            _mapper = mapper;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _httpContext = httpContext;
            coverImageFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "BookPic/");
            if (!Directory.Exists(coverImageFolderPath))
            {
                Directory.CreateDirectory(coverImageFolderPath);
            }
        }

        private int UserID() => int.Parse(_httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        private string GetUserRole() => _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        public async Task<ServiceResponse<List<GetBookDto>>> AddBook(AddBookDto book)
        {
            var response = new ServiceResponse<List<GetBookDto>>();
            Book b = _mapper.Map<Book>(book);
            _context.Books.Add(b);
            await _context.SaveChangesAsync();
            response.Data = await _context.Books.Select(c => _mapper.Map<GetBookDto>(c)).ToListAsync();
            return response;
        }

        public async Task<ServiceResponse<List<GetBookDto>>> DeleteBook(int id)
        {
            ServiceResponse<List<GetBookDto>> response = new ServiceResponse<List<GetBookDto>>();

            try
            {
                Book b = await _context.Books.FirstAsync(c => c.BookId == id);
                if (b != null)
                {
                    _context.Books.Remove(b);
                    await _context.SaveChangesAsync();

                    response.Data = await _context.Books.Select(c => _mapper.Map<GetBookDto>(c)).ToListAsync();
                }
                else
                {
                    response.Message = "Not Found";
                }
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<FilterBookDto>>> FilterBook(string bookname, string authorname, string category)
        {
            var book = await _context.Books
                    .Where(c => c.Category == category || c.Author == authorname || c.Title == bookname)
                    .OrderBy(c => c.Title)
                    .ThenBy(c => c.Author)
                    .ThenBy(c => c.Category)
                    .ToListAsync();

            var response = new ServiceResponse<List<FilterBookDto>>
            {
                Data = book.Select(c => _mapper.Map<FilterBookDto>(c)).ToList()
            };
            return response;
        }

        public async Task<ServiceResponse<List<GetBookDto>>> GetAllBooks()
        {
            var response = new ServiceResponse<List<GetBookDto>>();
            //var dbbook = GetUserRole().Equals("Admin") ?
            //    await _context.Books.ToListAsync() :
            //    await _context.Books.Where(c => c.BookId == UserID()).ToListAsync();
            var dbbook = await _context.Books.ToListAsync();
            response.Data = await _context.Books.Select(c => _mapper.Map<GetBookDto>(c)).ToListAsync();
            return response;
        }

        public async Task<ServiceResponse<GetBookDto>> GetBookbyId(int id)
        {
            var response = new ServiceResponse<GetBookDto>();
            var dbbook = await _context.Books.FirstOrDefaultAsync(x => x.BookId == id);
            response.Data = _mapper.Map<GetBookDto>(dbbook);
            return response;
        }

        public async Task<ServiceResponse<List<FilterBookDto>>> Searchbook(decimal price)
        {
            var book = await _context.Books
                .Where(p => p.Price == price)
                .OrderBy(p => p.Price)
                .ToListAsync();

            var response = new ServiceResponse<List<FilterBookDto>>
            {
                Data = book.Select(c => _mapper.Map<FilterBookDto>(c)).ToList()
            };
            return response;
        }

        public async Task<ServiceResponse<List<FilterBookDto>>> SortingbyPrice()
        {
            var book = await _context.Books
                .Where(p => p.Price>0)
                .OrderBy(p => p.Price)
                .ToListAsync();
            
            var response = new ServiceResponse<List<FilterBookDto>>
            {
                Data = book.Select(c => _mapper.Map<FilterBookDto>(c)).ToList()
            };
            return response;
        }

        public async Task<ServiceResponse<List<FilterBookDto>>> SortingbyPriceDecs()
        {
            var book = await _context.Books
                .Where(p => p.Price > 0)
                .OrderByDescending(p => p.Price)
                .ToListAsync();

            var response = new ServiceResponse<List<FilterBookDto>>
            {
                Data = book.Select(c => _mapper.Map<FilterBookDto>(c)).ToList()
            };
            return response;
        }

        public async Task<ServiceResponse<GetBookDto>> UpdateBook(UpdateBookDto book)
        {
            ServiceResponse<GetBookDto> response = new ServiceResponse<GetBookDto>();
            try
            {
                Book b = await _context.Books.FirstOrDefaultAsync(c => c.BookId == book.Id);

                //    if (book.CoverFileName == null)
                //    {
                //        book.CoverFileName = b.CoverFileName;
                //    }
                //}


                b.Title = book.Title;
                b.Author = book.Author;
                b.Quantity = book.Quantity;
                b.Category = book.Category;
                b.Price = book.Price;
                b.CoverFileName = book.CoverFileName;

                // _context.Entry(book).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetBookDto>(b);
 
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            var special = Guid.NewGuid().ToString();
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\BookImg", special + "-" + file.FileName);
            using (FileStream ms = new FileStream(filepath, FileMode.Create))
            {
                await file.CopyToAsync(ms);
            }
            var filename = special + "-" + file.FileName;
            return filepath;
        }

    }
}
