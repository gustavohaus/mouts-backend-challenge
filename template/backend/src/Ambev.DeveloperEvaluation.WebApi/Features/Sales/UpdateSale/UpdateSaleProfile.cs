using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleProfile : Profile
    {
        public UpdateSaleProfile()
        {
            CreateMap<UpdateSaleRequest, UpdateSaleCommand>();
            CreateMap<UpdateSaleProductRequest, UpdateSaleProductsCommand>();
            CreateMap<UpdateSaleResponse, GetSaleResult>();
            CreateMap<UpdateSaleProductsDto, SaleProductsDto>();

            CreateMap<GetSaleResult, UpdateSaleResponse>()
                .ForMember(dest => dest.SaleProducts, opt => opt.MapFrom(src => src.SaleProducts));

            CreateMap<SaleProductsDto, UpdateSaleProductsDto>();
        }
    }
}
