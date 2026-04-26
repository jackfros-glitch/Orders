using AutoMapper;
using Orders.Entities.Models;
using Orders.Shared.DataTransferObjects;
using Orders.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace Orders.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Maps a Product to a productDto
            CreateMap<Product, ProductDto>();

            CreateMap<ProductDto, Product>();
            

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderItems,
                    opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<OrderItem, OrderItemDto>();


            CreateMap<CreateOrderRequestDto, Order>();
            CreateMap<ProductRequestDto, OrderItem>();
            
            CreateMap<PaymentReference, PaymentReferenceDto>();

            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));
            
        }
    }
}
