using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace ShoppingAPI.Controllers.Mappings
{
    public class AppUserReadDTO
    {
        public string UserName { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }
    }
}
