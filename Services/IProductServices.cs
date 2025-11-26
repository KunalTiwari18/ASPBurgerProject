using System.Collections.Generic;
using BBURGERClone.Models;

namespace BBURGERClone.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        Product? GetById(int id);
    }
}
