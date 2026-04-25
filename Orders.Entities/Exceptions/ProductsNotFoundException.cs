using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Entities.Exceptions
{
    public class ProductsNotFoundException : NotFoundException
    {
        public ProductsNotFoundException(List<Guid> ids) : base($"The following product IDs were not found: {string.Join(", ", ids)}")
        {
        }
    }
}