using Microsoft.EntityFrameworkCore;
using QuoteCalculator.Domain.Entities;
using QuoteCalculator.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Infrastracture.Repositories
{
    public class LoanApplicationRepository : ILoanApplicationRepository
    {
        private readonly QuoteCalculatorDbContext _context;

        public LoanApplicationRepository(QuoteCalculatorDbContext context) => _context = context;

        public async Task AddAsync(LoanApplication loanApplication) =>
            await _context.LoanApplications.AddAsync(loanApplication);

        public async Task<LoanApplication?> GetByIdAsync(int loanApplicationId) =>
            await _context.LoanApplications.FirstOrDefaultAsync(la => la.LoanApplicationId == loanApplicationId);

        public async Task<LoanApplication?> GetByPublicIdAsync(Guid publicId) =>
            await _context.LoanApplications.Include(la => la.Applicant).FirstOrDefaultAsync(la => la.LoanApplicationPublicId == publicId);

        public async Task<LoanApplication?> GetByApplicantIdAsync(int applicantId) =>
            await _context.LoanApplications
                    .Where(la => la.ApplicantId == applicantId)
                    .OrderByDescending(la => la.CreatedAt)
                    .FirstOrDefaultAsync();

        public void Update(LoanApplication loanApplication) =>
            _context.LoanApplications.Update(loanApplication);
            
        public async Task<bool> SaveChangesAsync() =>
            await _context.SaveChangesAsync() > 0;
    }
}
