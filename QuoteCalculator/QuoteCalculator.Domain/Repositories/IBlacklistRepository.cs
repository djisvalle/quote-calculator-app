using QuoteCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Domain.Repositories
{
    public interface IBlacklistRepository
    {
        Task<bool> IsMobileNumberBlacklistedAsync(string mobileNumber);
        Task<bool> IsEmailDomainBlacklistedAsync(string email);
        Task<bool> SaveChangesAsync();
    }
}
