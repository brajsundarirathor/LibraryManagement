using AutoMapper;
using LibraryManagement.DTO;
using LibraryManagement.Models;

namespace LibraryManagement
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, GetBookDto>() 
                .ForMember(x => x.Id , opt => opt.MapFrom(b => b.BookId))  // manually autoincreament of bookid.
                .ReverseMap();
            CreateMap<AddBookDto, Book>().ReverseMap();
            CreateMap<UpdateBookDto, Book>().ReverseMap();
            CreateMap<UserOrder, OderDto>().ReverseMap();
            CreateMap<AddOrderDto, UserOrder>().ReverseMap();
            CreateMap<Book, FilterBookDto>().ReverseMap();
            CreateMap<SubmitOrderDto, OderDto>().ReverseMap();
        }
    }
}
