
using AutoMapper;
using Utilities.DataTransferObjects;
using Utilities.Models;
using Utilities.ViewModels;

namespace Utilities.Infrastructure
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile(IMapper mapper)
        {
            _mapper = mapper;
        }
        private readonly IMapper _mapper;


        public AutoMapperProfile()
        {

            //mappa från accountVM till accounts och reversed
            CreateMap<AccountViewModel, Account>()
                .ReverseMap();

            CreateMap<CustomerViewModel, Customer>()
                .ReverseMap();

            CreateMap<CreateCustomerViewModel, Customer>()
                .ReverseMap();

            CreateMap<UpdateCustomerViewModel, Customer>()
                .ReverseMap();

            CreateMap<CustomerDto, Customer>()
                .ReverseMap();

            CreateMap<Transaction, TransactionDto>();

            CreateMap<List<Transaction>, List<TransactionDto>>()
                .ConvertUsing(src => src.Select(x => _mapper.Map<TransactionDto>(x)).ToList());

            CreateMap<List<Account>, List<AccountViewModel>>()
                .ConvertUsing(src => src.Select(x => _mapper.Map<AccountViewModel>(x)).ToList());

        }
    }
}