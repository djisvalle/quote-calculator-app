
using QuoteCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Domain.Repositories
{
    public interface IProductTypeRepository
    {
        Task<IEnumerable<ProductType>> GetAllAsync();
        Task<ProductType?> GetByIdAsync(int productTypeId);
        Task<bool> SaveChangesAsync();
    }
}
