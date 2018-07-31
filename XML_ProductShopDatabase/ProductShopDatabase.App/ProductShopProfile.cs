using AutoMapper;
using ProductShop.App.DTOs;
using ProductShop.Models;
using System.Linq;

namespace ProductShop.App
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<CategoryNameDto, Category>();

            CreateMap<Product, ProductsInRangeDto>()
                .ForMember(dto => dto.BuyerName,
                    opt => opt.MapFrom(src => $"{src.Buyer.FirstName} {src.Buyer.LastName}".Trim()));

            CreateMap<User, UserSellerDto>()
                .ForMember(dto => dto.SoldProducts,
                    opt => opt.MapFrom(src => src.ProductsSold));

            CreateMap<Category, CategoryDto>()
                .ForMember(dto => dto.NumberOfProducts,
                    opt => opt.MapFrom(src => src.Products.Count))
                .ForMember(dto => dto.AverageProductPrice,
                    opt => opt.MapFrom(src => src.Products.Average(p => p.Product.Price)))
                .ForMember(dto => dto.TotalRevenue,
                    opt => opt.MapFrom(src => src.Products.Sum(p => p.Product.Price)));
        }
    }
}
