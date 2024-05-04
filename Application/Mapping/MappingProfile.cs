using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Application.Mapping;
internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CustomerMD, CustomerDTO>().ReverseMap();
        CreateMap<UserRegistrationDTO, UserMD>();
    }
}
