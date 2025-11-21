using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public class GetSalesProfiler : Profile
    {
        public GetSalesProfiler()
        {
            CreateMap<(List<Sale> Sales, int TotalCount), PagedSalesResult>()
                .ForMember(dest => dest.Sales, opt => opt.MapFrom(src => src.Sales))
                .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount));

            CreateMap<Sale, GetSaleResult>()
                .ForMember(dest => dest.SaleProducts, opt => opt.MapFrom(src => src.SaleProducts))
                .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<Sale, PagedSalesResult>()
                .ForMember(dest => dest.Sales, opt => opt.Ignore()) // Ignorar a propriedade Sales, pois ela será mapeada separadamente
                .ForMember(dest => dest.TotalCount, opt => opt.Ignore()) // Ignorar TotalCount, pois será preenchido no handler
                .ForMember(dest => dest.PageNumber, opt => opt.Ignore()) // Ignorar PageNumber, pois será preenchido no handler
                .ForMember(dest => dest.PageSize, opt => opt.Ignore()); // Ignorar PageSize, pois será preenchido no handler
        }
    }
}
