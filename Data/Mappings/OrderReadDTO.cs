using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ShoppingAPI.Domain;

namespace ShoppingAPI.Data.Mappings
{
    public class OrderReadDTO
    {
        public int Id { get; private set; }

        public Guid OrderId { get; private set; }

        public AppUserReadDTO User { get; private set; }

#nullable enable
        public DateTime? FullfilledAt { get; private set; }
#nullable disable

        public DateTime CreatedAt { get; private set; }
    }
}
