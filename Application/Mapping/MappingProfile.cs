using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Application.Mapping;
internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CustomerModel, CustomerDTO>().ReverseMap();
        CreateMap<SupplierModel, SupplierDto>().ReverseMap();
        CreateMap<UserRegistrationDTO, UserModel>();
    }
}
