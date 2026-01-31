using Microsoft.EntityFrameworkCore;
using QuoteCalculator.Domain.Entities;
using QuoteCalculator.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Infrastracture.Repositories
{
    public class ApplicantRepository : IApplicantRepository
    {
        private readonly QuoteCalculatorDbContext _context;

        public ApplicantRepository(QuoteCalculatorDbContext context) => _context = context;

        public async Task AddAsync(Applicant applicant) =>
            await _context.Applicants.AddAsync(applicant);

        public async Task<Applicant?> GetByIdAsync(int applicantId) =>
            await _context.Applicants.FirstOrDefaultAsync(a => a.ApplicantId == applicantId);

        public async Task<Applicant?> GetByNameAndDOBAsync(string firstName, string lastName, DateOnly dateOfBirth) =>
            await _context.Applicants.AsNoTracking().FirstOrDefaultAsync(a =>
                EF.Functions.ILike(a.FirstName, firstName) &&
                EF.Functions.ILike(a.LastName, lastName) &&
                a.DateOfBirth == dateOfBirth);

        public void Update(Applicant applicant) =>
            _context.Applicants.Update(applicant);

        public async Task<bool> SaveChangesAsync() =>
            await _context.SaveChangesAsync() > 0;
    }
}
