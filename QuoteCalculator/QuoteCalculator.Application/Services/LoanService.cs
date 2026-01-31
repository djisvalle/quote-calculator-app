using AutoMapper;
using Excel.FinancialFunctions;
using Microsoft.Extensions.Configuration;
using QuoteCalculator.Application.DTOs.Loan.Request;
using QuoteCalculator.Application.DTOs.Loan.Response;
using QuoteCalculator.Application.Enums;
using QuoteCalculator.Application.Interfaces;
using QuoteCalculator.Domain.Entities;
using QuoteCalculator.Domain.Repositories;
using System.Transactions;

namespace QuoteCalculator.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly ILoanApplicationRepository _loanApplicationRepository;
        private readonly IBlacklistRepository _blacklistRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IGlobalConfigurationRepository _globalConfigurationRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public LoanService(IApplicantRepository applicantRepository, ILoanApplicationRepository loanApplicationRepository, IBlacklistRepository blacklistRepository
            , IProductTypeRepository productTypeRepository, IGlobalConfigurationRepository globalConfigurationRepository, IMapper mapper, IConfiguration configuration)
        {
            _applicantRepository = applicantRepository;
            _loanApplicationRepository = loanApplicationRepository;
            _blacklistRepository = blacklistRepository;
            _productTypeRepository = productTypeRepository;
            _globalConfigurationRepository = globalConfigurationRepository;
            _mapper = mapper;
            _configuration = configuration;
        }


        public async Task<string> ProcessLoanAsync(CreateLoanRequestDto dto)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // Idempotency Check
                    var applicant = await _applicantRepository.GetByNameAndDOBAsync(dto.FirstName, dto.LastName, dto.DateOfBirth);

                    if (applicant == null)
                    {
                        applicant = _mapper.Map<Applicant>(dto);
                        await _applicantRepository.AddAsync(applicant);
                        await _applicantRepository.SaveChangesAsync();

                    }
                    else
                    {
                        var existingLoan = await _loanApplicationRepository.GetByApplicantIdAsync(applicant.ApplicantId);
                        if (existingLoan != null && existingLoan.Status == (int)ApplicationStatus.Pending)
                        {
                            return GenerateUserUrl(existingLoan.LoanApplicationPublicId);
                        }
                    }

                    var loanApplication = _mapper.Map<LoanApplication>(dto);
                    loanApplication.ApplicantId = applicant.ApplicantId;
                    loanApplication.Status = (int)ApplicationStatus.Pending;

                    await _loanApplicationRepository.AddAsync(loanApplication);
                    await _loanApplicationRepository.SaveChangesAsync();

                    scope.Complete();

                    return GenerateUserUrl(loanApplication.LoanApplicationPublicId);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<LoanApplicationDetailsResponseDto?> GetLoanApplicationDetailsAsync(Guid publicId)
        {
            var existingApplication = await _loanApplicationRepository.GetByPublicIdAsync(publicId);
            if (existingApplication == null)
                throw new KeyNotFoundException("Loan Application does not exist.");

            var responseDto = _mapper.Map<LoanApplicationDetailsResponseDto>(existingApplication);
            return responseDto;
        }

        public async Task SaveDraftLoanApplicationAsync(SaveDraftLoanRequestDto dto)
        {
            var existingApplication = await _loanApplicationRepository.GetByPublicIdAsync(dto.LoanApplicationPublicId);
            if (existingApplication == null)
                throw new KeyNotFoundException("Loan Application does not exist anymore.");

            var existingApplicant = await _applicantRepository.GetByIdAsync(dto.ApplicantId);
            if (existingApplicant == null)
                throw new KeyNotFoundException("Applicant does not exist anymore.");

            if (existingApplication.ApplicantId != dto.ApplicantId)
                throw new UnauthorizedAccessException("You are not authorized to update this loan application.");

            if (existingApplication.Status != (int)ApplicationStatus.Pending)
                throw new InvalidOperationException("Only pending loan applications can be updated.");

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    existingApplicant.FirstName = dto.FirstName;
                    existingApplicant.LastName = dto.LastName;
                    existingApplicant.MobileNumber = dto.MobileNumber;
                    existingApplicant.Email = dto.Email;
                    _applicantRepository.Update(existingApplicant);
                    await _applicantRepository.SaveChangesAsync();
                    

                    existingApplication.Amount = (decimal)dto.Amount;
                    existingApplication.Term = dto.Term;
                    existingApplication.ProductType = dto.ProductTypeId;
                    _loanApplicationRepository.Update(existingApplication);
                    await _loanApplicationRepository.SaveChangesAsync();

                    scope.Complete();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<CalculateLoanResponseDto> CalculateLoanDetailsAsync(CalculateLoanRequestDto dto)
        {
            var productType = await _productTypeRepository.GetByIdAsync(dto.ProductTypeId);
            if (productType == null)
                throw new KeyNotFoundException("Product Type does not exist.");

            if (dto.Term < productType.MinimumTerm || dto.Term > productType.MaximumTerm)
                throw new ArgumentOutOfRangeException($"Term must be between {productType.MinimumTerm} and {productType.MaximumTerm} months for the selected product type.");

            if (dto.Amount < productType.MinimumAmount || dto.Amount > productType.MaximumAmount)
                throw new ArgumentOutOfRangeException($"Amount must be between {productType.MinimumAmount} and {productType.MaximumAmount} for the selected product type.");

            decimal establishmentFee;
            var globalConfigEstablishmentFee = await _globalConfigurationRepository.GetByKeyAsync(ConfigurationKeys.EstablishmentFee.ToString());
            if (globalConfigEstablishmentFee == null)
                establishmentFee = 0;
            else
                establishmentFee = Convert.ToDecimal(globalConfigEstablishmentFee.Value);

            decimal rate = productType.InterestType == (int)InterestTypes.FixedFree ? 0 : productType.InterestRate / 100m / 12m;
            int nper = dto.Term;
            decimal pv = dto.Amount + establishmentFee;

            decimal weeksInMonth = 52m/12m;

            decimal PMT = 0m;
            if (productType.InterestType == (int)InterestTypes.Deferred)
            {
                int interestFreeMonths = productType.InterestFreeMonths;
                decimal interestFreeMonthlyRepayment = pv / nper;
                decimal interestFreeTotalRepayment = interestFreeMonthlyRepayment * interestFreeMonths;

                decimal balance = pv - interestFreeTotalRepayment;

                int withInterestMonths = nper - interestFreeMonths;
                PMT = (decimal)Math.Abs(Financial.Pmt((double)rate, withInterestMonths, (double)balance, 0, 0));
            }
            else PMT = (decimal)Math.Abs(Financial.Pmt((double)rate, nper, (double)pv, 0, 0));

            decimal monthlyPayment = Math.Round(PMT, 4, MidpointRounding.ToEven);
            decimal totalRepayment = monthlyPayment * nper;
            decimal weeklyPayment = Math.Round(totalRepayment / (weeksInMonth * nper), 4, MidpointRounding.AwayFromZero);
            decimal interestFee = totalRepayment - pv;

            return new CalculateLoanResponseDto
            {
                Amount = dto.Amount,
                Term = dto.Term,
                EstablishmentFee = establishmentFee,
                InterestFee = interestFee,
                WeeklyRepayment = weeklyPayment,
                MonthlyRepayment = monthlyPayment,
                TotalRepayment = totalRepayment
            };         
        }

        public async Task FinalizeLoanAsync(FinalizeLoanRequestDto dto)
        {
            var existingApplication = await _loanApplicationRepository.GetByPublicIdAsync(dto.LoanApplicationPublicId);
            if (existingApplication == null) 
                throw new KeyNotFoundException("Loan Application does not exist anymore."); 

            if (existingApplication.Status != (int)ApplicationStatus.Pending) 
                throw new InvalidOperationException("Only pending loan applications can be finalized.");

            var existingApplicant = await _applicantRepository.GetByIdAsync(dto.ApplicantId);
            if (existingApplicant == null) 
                throw new KeyNotFoundException("Applicant does not exist anymore.");

            bool isUnderage = existingApplicant.DateOfBirth.AddYears(18) > DateOnly.FromDateTime(DateTime.UtcNow);
            bool isMobileBlacklisted = await _blacklistRepository.IsMobileNumberBlacklistedAsync(dto.MobileNumber);
            bool isEmailDomainBlacklisted = await _blacklistRepository.IsEmailDomainBlacklistedAsync(dto.Email);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (isUnderage || isMobileBlacklisted || isEmailDomainBlacklisted)
                    {
                        existingApplication.Status = (int)ApplicationStatus.Rejected;
                        existingApplication.Remarks = isUnderage ? "Applicant is underage." :
                            isMobileBlacklisted ? "Applicant's mobile number is blacklisted." : "Applicant's email domain is blacklisted.";
                    }
                    else
                    {
                        existingApplication.Status = (int)ApplicationStatus.Submitted;
                        existingApplication.ReferenceNumber = (existingApplication.ProductType == (int)InterestTypes.Standard ? "SL"
                        : existingApplication.ProductType == (int)InterestTypes.FixedFree ? "FFL" : "DL")
                        + "-" + DateTime.UtcNow.Ticks.ToString().Substring(8);
                    }
                        
                    // Update Applicant Details (regardless of approval or rejection)
                    existingApplicant.FirstName = dto.FirstName;
                    existingApplicant.LastName = dto.LastName;
                    existingApplicant.MobileNumber = dto.MobileNumber;
                    existingApplicant.Email = dto.Email;
                    _applicantRepository.Update(existingApplicant);
                    await _applicantRepository.SaveChangesAsync();

                    existingApplication.Amount = dto.Amount;
                    existingApplication.Term = dto.Term;
                    existingApplication.InterestFee = dto.InterestFee;
                    existingApplication.WeeklyRepayment = dto.WeeklyRepayment;
                    existingApplication.MonthlyRepayment = dto.MonthlyRepayment;
                    existingApplication.TotalRepayment = dto.TotalRepayment;
                    existingApplication.UpdatedAt = DateTime.UtcNow;

                    _loanApplicationRepository.Update(existingApplication);
                    await _loanApplicationRepository.SaveChangesAsync();

                    scope.Complete();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<ApplicationDetailsResponseDto> GetReceiptDetailsAsync(Guid publicId)
        {
            var existingApplication = await _loanApplicationRepository.GetByPublicIdAsync(publicId);
            if (existingApplication == null)
                throw new KeyNotFoundException("Loan Application does not exist.");

            var responseDto = _mapper.Map<ApplicationDetailsResponseDto>(existingApplication);
            return responseDto;
        }

        private string GenerateUserUrl(Guid publicId) =>
            _configuration.GetValue<string>("ClientBaseUrl") + $"/quote-calculator/initial-quote?loans={publicId}";
        
    }
}
