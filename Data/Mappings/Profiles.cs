using AutoMapper;
using ShoppingAPI.Domain;

namespace ShoppingAPI.Data.Mappings
{
    public class Profiles : Profile
    {
        public Profiles() 
        {
            // Source -> Destination
            CreateMap<AppUser, AppUserReadDTO>();
            CreateMap<Product, ProductReadDTO>();
        }
    }
}