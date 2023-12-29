
using AutoMapper;
using Vb.Business;
using Vb.Data.Entity;

namespace Vb.Business.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<Address, AddressDto>().ReverseMap();
        CreateMap<Contact, ContactDto>().ReverseMap();
        CreateMap<Account, AccountDto>().ReverseMap();
        CreateMap<AccountTransaction, AccountTransactionDto>().ReverseMap();
        CreateMap<EftTransaction, EftTransactionDto>().ReverseMap();
    }
}
