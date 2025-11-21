using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleProfiler : Profile
    {
        public GetSaleProfiler()
        {
            CreateMap<Sale, GetSaleResult>()
                .ForMember(dest => dest.SaleProducts, opt => opt.MapFrom(src => src.SaleProducts))
                .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<User, SaleUserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<Branch, BranchDto>();
            CreateMap<Product, ProductDto>();

            CreateMap<SaleProduct, SaleProductsDto>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.UnitPrice * (1 - src.DiscountPercent / 100)))
                .ForMember(dest => dest.DiscountPercent, opt => opt.MapFrom(src => src.DiscountPercent));
        }
    }
}
