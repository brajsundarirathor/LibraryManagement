using LibraryManagement.Data;
using LibraryManagement.DTO;
using LibraryManagement.Models;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class UserOrderController : ControllerBase
    {
        private readonly BookDbContext _context;
        private readonly IOrderService _orderService;

        public UserOrderController(BookDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        [HttpGet("GetOrder")]

        public async Task<ActionResult<ServiceResponse<OderDto>>> GetOrder(int userId)
        {
            var response = await _orderService.GetOrderList(userId);
            if(response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost("AddOrder")]

        public async Task<ActionResult<ServiceResponse<List<OderDto>>>> AddOrder(AddOrderDto addOrder)
        {
            var response = await _orderService.AddOrderList(addOrder);
            if (response == null) 
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("SubmitBook")]
        public async Task<ActionResult<ServiceResponse<GetBookDto>>> SubmitBook(SubmitOrderDto submitOrder)
        {
            var response = await _orderService.SubmitBook(submitOrder);
            if (response == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


    }
}
