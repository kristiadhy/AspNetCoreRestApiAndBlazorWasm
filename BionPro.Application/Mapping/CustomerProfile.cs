using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Application.Mapping;
internal class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CustomerMD, CustomerDTO>().ReverseMap();
    }
}
