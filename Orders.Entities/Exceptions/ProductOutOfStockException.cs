using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Entities.Exceptions
{
    public class ProductOutOfStockException : BadRequestException
    {
        public ProductOutOfStockException(Guid id) : base($"The Product with id: {id} out of Stock")
        {
        }
    }
}