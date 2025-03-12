using AutoMapper;
using PayPridge.Application.DTOs;
using PayPridge.Domain.Domain;
using PayPridge.Domain.Entities;

namespace PayPridge.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<ProductResponse, ProductDto>();
            CreateMap<List<Product>, ProductResponse>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src));

            CreateMap<List<OrderItemDto>, Order>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src));
        }
    }

}