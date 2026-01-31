using QuoteCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Domain.Repositories
{
    public interface IApplicantRepository
    {
        Task AddAsync(Applicant applicant);
        Task<Applicant?> GetByIdAsync(int applicantId);
        Task<Applicant?> GetByNameAndDOBAsync(string firstName, string lastName, DateOnly dateOfBirth);
        void Update(Applicant applicant);
        Task<bool> SaveChangesAsync();
    }
}
