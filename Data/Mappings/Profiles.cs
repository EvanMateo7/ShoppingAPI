using AutoMapper;
using ShoppingAPI.Domain;

namespace ShoppingAPI.Data.Mappings
{
    public class Profiles : Profile
    {
        public Profiles() 
        {
            // Source -> Destination

            // Domain -> DTO
            CreateMap<AppUser, AppUserReadDTO>();
            CreateMap<Product, ProductReadDTO>();

            // DTO -> Domain
            CreateMap<ProductCreateDTO, Product>()
              .ForMember(p => p.User, opt => opt.Ignore())
              .ForMember(p => p.OrderProducts, opt => opt.Ignore());
        }
    }
}