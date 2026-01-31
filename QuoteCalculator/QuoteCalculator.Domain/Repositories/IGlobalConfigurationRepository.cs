
using QuoteCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Domain.Repositories
{
    public interface IGlobalConfigurationRepository
    {
        Task<GlobalConfiguration?> GetByKeyAsync(string key);
    }
}
