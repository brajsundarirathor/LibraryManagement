using LibraryManagement.Models;
using LibraryManagement.DTO;

namespace LibraryManagement.Services
{
    public interface IOrderService
    {
        Task<ServiceResponse<OderDto>> GetOrderList(int userId);

        Task<ServiceResponse<List<OderDto>>> AddOrderList(AddOrderDto addOrder);

        Task<ServiceResponse<OderDto>> SubmitBook(SubmitOrderDto submitbook);
    }
}
