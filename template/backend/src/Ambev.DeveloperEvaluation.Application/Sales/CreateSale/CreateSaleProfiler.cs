using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleProfiler : Profile
    {
        public CreateSaleProfiler()
        {
            CreateMap<Sale, CreateSaleResult>()
                       .ForMember(dest => dest.SaleProducts, opt => opt.MapFrom(src => src.SaleProducts))
                       .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<SaleProduct, CreateSaleProductResult>()
          .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => Math.Round(src.Product.UnitPrice, 2)))
          .ForMember(dest => dest.DiscountPercent, opt => opt.MapFrom(src => Math.Round(src.DiscountPercent, 2)))
          .ForMember(dest => dest.DiscountedPrice, opt => opt.MapFrom(src => Math.Round(src.Product.UnitPrice - (src.Product.UnitPrice * (src.DiscountPercent / 100)), 2)));
        }
    }
}
