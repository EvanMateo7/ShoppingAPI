using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingAPI.Domain
{
    public class Cart : IDomainEntity
    {
        public int Id { get; private set; }

        public string UserId { get; private set; }
        public AppUser User { get; private set; }

        public int ProductId { get; private set; }
        public Product Product { get; private set; }
    }
}
