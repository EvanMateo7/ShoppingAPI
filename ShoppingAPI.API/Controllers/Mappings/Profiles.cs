using AutoMapper;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using ShoppingAPI.Domain.AggregateRoots.OrderAggregate;
using ShoppingAPI.Domain.AggregateRoots.ProductAggregate;

namespace ShoppingAPI.API.Controllers.Mappings
{
  public class Profiles : Profile
  {
    public Profiles()
    {
      // Source -> Destination

      // Domain -> DTO
      CreateMap<AppUser, AppUserReadDTO>();

      CreateMap<Product, ProductReadDTO>();
      CreateMap<Product, ProductCreateDTO>();

      CreateMap<Order, OrderReadDTO>();

      // DTO -> Domain
      CreateMap<ProductCreateDTO, Product>()
        .ForMember(p => p.User, opt => opt.Ignore())
        .ForMember(p => p.OrderProducts, opt => opt.Ignore());
    }
  }
}
