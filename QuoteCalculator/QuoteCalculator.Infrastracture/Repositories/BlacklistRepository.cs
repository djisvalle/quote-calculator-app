using Microsoft.EntityFrameworkCore;
using QuoteCalculator.Application.Enums;
using QuoteCalculator.Domain.Repositories;

namespace QuoteCalculator.Infrastracture.Repositories
{
    public class BlacklistRepository : IBlacklistRepository
    {
        private readonly QuoteCalculatorDbContext _context;

        public BlacklistRepository(QuoteCalculatorDbContext context) => _context = context;

        public async Task<bool> IsMobileNumberBlacklistedAsync(string mobileNumber) =>
            await _context.Blacklists.AsNoTracking().AnyAsync(b =>
                b.Value == mobileNumber && b.BlacklistType == (int)BlacklistTypes.MobileNumber);

        public async Task<bool> IsEmailDomainBlacklistedAsync(string email)
        {
            var domain = email.Split("@").Last().ToLower();

            return await _context.Blacklists.AsNoTracking().AnyAsync(b =>
                b.Value.ToLower() == domain && b.BlacklistType == (int)BlacklistTypes.EmailDomain);
        }
            
        public async Task<bool> SaveChangesAsync() =>
            await _context.SaveChangesAsync() > 0;
    }
}
