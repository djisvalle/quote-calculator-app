using QuoteCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Domain.Repositories
{
    public interface ILoanApplicationRepository
    {
        Task AddAsync(LoanApplication loanApplication);
        Task<LoanApplication?> GetByIdAsync(int loanApplicationId);
        Task<LoanApplication?> GetByPublicIdAsync(Guid publicId);
        Task<LoanApplication?> GetByApplicantIdAsync(int applicantId);
        void Update(LoanApplication loanApplication);
        Task<bool> SaveChangesAsync();
    }
}
