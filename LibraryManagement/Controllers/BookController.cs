using LibraryManagement.Data;
using LibraryManagement.DTO;
using LibraryManagement.Models;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace LibraryManagement.Controllers
{
    //[Authorize (Roles = "Admin")]
    [ApiController]
    [Route("api/controller")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;
        private readonly BookDbContext _context;
        readonly string coverImageFolderPath = string.Empty;

        public BookController(IBookService bookService, IWebHostEnvironment hostingEnvironment, IConfiguration config, BookDbContext context)
        {
            _bookService = bookService;
            _hostingEnvironment = hostingEnvironment;
            _config = config;
            _context = context;
            coverImageFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "BookPic");
            if (!Directory.Exists(coverImageFolderPath))
            {
                Directory.CreateDirectory(coverImageFolderPath);
            }
        }

       
        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetBookDto>>>> GetAllBook()
        {
            var response = await _bookService.GetAllBooks();
            if(response == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetBookDto>>> GetSingle(int id)
        {
            var response = await _bookService.GetBookbyId(id);
            if (response == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("AddBook")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<GetBookDto>>>> AddBook(AddBookDto book)
        {
            var response = await _bookService.AddBook(book);
            if (response == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


        [HttpPut("Update Book")]
        public async Task<ActionResult<ServiceResponse<GetBookDto>>> UpdateBook(UpdateBookDto book)
        {
            var response = await _bookService.UpdateBook(book);
            if (response == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetBookDto>>>> DeleteBook(int id)
        {
            var response = await _bookService.DeleteBook(id);
            if (response == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

       
        [HttpGet("Search by BookName,Author,Category")]
        public async Task<ActionResult<ServiceResponse<List<FilterBookDto>>>> FilterBook(string bookname, string authorname, string category)
        {            
            var response = await _bookService.FilterBook(bookname, authorname, category);
            if(response == null)
            {
                return NotFound(response);
            }
            return Ok(response); 
        }

        [HttpGet("Search by Price")]
        public async Task<ActionResult<ServiceResponse<List<FilterBookDto>>>> Searchbook(decimal price)
        {          
            var response = await _bookService.Searchbook(price);
            if (response == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("Sort by Price in Ascending Order")]
        public async Task<ActionResult<ServiceResponse<List<FilterBookDto>>>> SortByPrice()
        {
            var response = await _bookService.SortingbyPrice();
            if (response == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("Sort by Price in Descending Order")]
        public async Task<ActionResult<ServiceResponse<List<FilterBookDto>>>> SortByPriceDesc()
        {
            var response = await _bookService.SortingbyPriceDecs();
            if (response == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("Image Upload for Book")]
        public async Task<ActionResult<ServiceResponse<string>>> PostImage([FromForm] Image user)
        {
            //var reponse = await _authrepo.UploadImage1(user.)
            string path = await _bookService.UploadImage(user.FileUrl);
            var u = user.FileUrl.ToString();
            //await _context.UserMaster.AddAsync();
            await _context.SaveChangesAsync();
            return Ok(path);
        }
    }
}
