using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace ShoppingAPI.Domain
{
    public class AppUser : IdentityUser, IDomainEntity
    {
        public string First_Name { get; private set; }

        public string Last_Name { get; private set; }

        public ICollection<Order> Orders { get; private set; }

        public ICollection<Product> Products { get; private set; }

        public ICollection<Cart> CartProducts { get; private set; }
    }
}
