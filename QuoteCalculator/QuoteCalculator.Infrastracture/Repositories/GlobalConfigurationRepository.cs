using Microsoft.EntityFrameworkCore;
using QuoteCalculator.Domain.Entities;
using QuoteCalculator.Domain.Repositories;

namespace QuoteCalculator.Infrastracture.Repositories
{
    public class GlobalConfigurationRepository : IGlobalConfigurationRepository
    {
        private readonly QuoteCalculatorDbContext _context;

        public GlobalConfigurationRepository(QuoteCalculatorDbContext context) => _context = context;

        public async Task<GlobalConfiguration?> GetByKeyAsync(string key) =>
            await _context.GlobalConfigurations.AsNoTracking().FirstOrDefaultAsync();
    }
}
