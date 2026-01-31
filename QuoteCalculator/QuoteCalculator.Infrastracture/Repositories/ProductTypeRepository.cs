using Microsoft.EntityFrameworkCore;
using QuoteCalculator.Domain.Entities;
using QuoteCalculator.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Infrastracture.Repositories
{
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly QuoteCalculatorDbContext _context;

        public ProductTypeRepository(QuoteCalculatorDbContext context) => _context = context;

        public async Task<IEnumerable<ProductType>> GetAllAsync() =>
            await _context.ProductTypes.AsNoTracking().ToListAsync();

        public async Task<ProductType?> GetByIdAsync(int productTypeId) =>
            await _context.ProductTypes.AsNoTracking().FirstOrDefaultAsync(pt => pt.ProductTypeId == productTypeId);

        public async Task<bool> SaveChangesAsync() =>
            await _context.SaveChangesAsync() > 0;
    }
}
