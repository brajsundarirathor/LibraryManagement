using AutoMapper;
using LibraryManagement.Data;
using LibraryManagement.DTO;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace LibraryManagement.Services
{
    public class OrderService : IOrderService
    {
        private readonly BookDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(BookDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<OderDto>>> AddOrderList(AddOrderDto addOrder)
        {
            var response = new ServiceResponse<List<OderDto>>();
            try
            {
                UserOrder b = _mapper.Map<UserOrder>(addOrder);
                b.DueDate = b.CreatedDate.AddDays(10);
                b.IsSubmitted = false;
                //UserOrder userOrder = _context.UserOrders.FirstOrDefault(c => c.BookId == addOrder.BookId || c.UserMasterId == addOrder.UserMasterId);
                if (b.IsSubmitted == false)
                {
                    Book db = await _context.Books.FirstOrDefaultAsync(c => c.BookId == b.BookId);
                    if (db != null)
                    {
                        db.Quantity -= b.Quantity;
                        _context.Entry(db).State = EntityState.Modified;
                        _context.UserOrders.Add(b);
                        await _context.SaveChangesAsync();
                        response.Data = await _context.UserOrders.Select(c => _mapper.Map<OderDto>(c)).ToListAsync();
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "invalid BookId";
                    }
                }
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                
            }
           
            return response;

        }

        public async Task<ServiceResponse<OderDto>> GetOrderList(int userId)
        {
            var response = new ServiceResponse<OderDto>();
            var dbbook = await _context.UserOrders.FirstOrDefaultAsync(x => x.OrderId == userId);
            //if(dbbook.DueDate > dbbook.CreatedDate.AddDays(10) && dbbook.IsSubmitted == false) 
            //{ 
            //    var dayofpenalty = dbbook.DueDate - dbbook.CreatedDate.AddDays(10);
            //    int n = int.Parse(dayofpenalty.ToString("yyyyMMddHHmmss"));
            //    dbbook.Penalty = 5 * n;
            //}
            response.Data = _mapper.Map<OderDto>(dbbook);
            return response;
        }

        public async Task<ServiceResponse<OderDto>> SubmitBook(SubmitOrderDto submitbook)
        {
            ServiceResponse<OderDto> response = new ServiceResponse<OderDto>();
            UserOrder b = await _context.UserOrders.FirstOrDefaultAsync(c => c.OrderId == submitbook.OrderId);
            b.IsSubmitted = true;
            try
            {
                Book db = await _context.Books.FirstOrDefaultAsync(c => c.BookId == submitbook.BookId);

                if (db != null)
                {
                    if (b.IsSubmitted == true)
                    {
                        //UserOrder bo = await _context.Books.FirstOrDefaultAsync(x => x.Quantity == submitbook.Quantity);
                        db.Quantity += b.Quantity;
                        b.Quantity -= b.Quantity;
                        
                        //await _context.SaveChangesAsync();
                       // _context.Entry(db).State = EntityState.Modified;
                       // _context.Entry(submitbook).State = EntityState.Modified;
                    }

                    UserOrder userOrder = await _context.UserOrders.FindAsync(submitbook.OrderId);
                    if (submitbook.DueDate > userOrder.CreatedDate.AddDays(10))
                    {
                        var dayofpenalty = userOrder.DueDate - userOrder.CreatedDate.AddDays(10);
                        int n = int.Parse(dayofpenalty.ToString("yyyyMMddHHmmss"));
                        userOrder.Penalty = 5 * n;
                        _context.Entry(n).State = EntityState.Modified;
                    }
                }
                response.IsSubmitted = true;

                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<OderDto>(submitbook);

            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
