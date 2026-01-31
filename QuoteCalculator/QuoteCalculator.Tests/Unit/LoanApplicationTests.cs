using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using QuoteCalculator.Application.DTOs.Loan.Request;
using QuoteCalculator.Application.Enums;
using QuoteCalculator.Application.Services;
using QuoteCalculator.Domain.Entities;
using QuoteCalculator.Domain.Repositories;
using Xunit;

namespace QuoteCalculator.Tests.Unit
{
    public class LoanApplicationTests
    {
        private readonly IApplicantRepository _applicantRepository = Substitute.For<IApplicantRepository>();
        private readonly ILoanApplicationRepository _loanApplicationRepository = Substitute.For<ILoanApplicationRepository>();
        private readonly IBlacklistRepository _blacklistRepository = Substitute.For<IBlacklistRepository>();
        private readonly IProductTypeRepository _productTypeRepository = Substitute.For<IProductTypeRepository>();
        private readonly IGlobalConfigurationRepository _globalConfigurationRepository = Substitute.For<IGlobalConfigurationRepository>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly IConfiguration _configuration = Substitute.For<IConfiguration>();
        private readonly LoanService _service;

        public LoanApplicationTests()
        {
            _service = new LoanService(_applicantRepository, _loanApplicationRepository, _blacklistRepository
                , _productTypeRepository, _globalConfigurationRepository, _mapper, _configuration);
        }

        [Fact]
        public async Task CalculateLoan_WhenValuesAreValid_ReturnsCalculatedLoan()
        {
            _productTypeRepository.GetByIdAsync(1).Returns(new ProductType
            {
                ProductTypeId = 1,
                ProductName = "Personal Loan",
                InterestType = 0,
                InterestRate = 5.0m,
                InterestFreeMonths = 0,
                MinimumTerm = 1,
                MaximumTerm = 60,
                MinimumAmount = 1000,
                MaximumAmount = 50000
            });

            _globalConfigurationRepository.GetByKeyAsync(ConfigurationKeys.EstablishmentFee.ToString()).Returns(new GlobalConfiguration
            {
                Key = ConfigurationKeys.EstablishmentFee.ToString(),
                Value = "300"
            });

            var dto = new CalculateLoanRequestDto
            {
                Amount = 5000,
                Term = 24,
                ProductTypeId = 1
            };

            var result = await _service.CalculateLoanDetailsAsync(dto);

            result.TotalRepayment.Should().Be(5580.44m);
        }

        [Fact]
        public async Task CalculateLoan_WhenAmountAndTermOutOfRange_ReturnException()
        {
            _productTypeRepository.GetByIdAsync(1).Returns(new ProductType
            {
                ProductTypeId = 1,
                ProductName = "Personal Loan",
                InterestType = 0,
                InterestRate = 5.0m,
                InterestFreeMonths = 0,
                MinimumTerm = 6,
                MaximumTerm = 24,
                MinimumAmount = 1000,
                MaximumAmount = 5000
            });

            _globalConfigurationRepository.GetByKeyAsync(ConfigurationKeys.EstablishmentFee.ToString()).Returns(new GlobalConfiguration
            {
                Key = ConfigurationKeys.EstablishmentFee.ToString(),
                Value = "300"
            });

            var minAmount = new CalculateLoanRequestDto
            {
                Amount = 100m,
                Term = 12,
                ProductTypeId = 1
            };

            var maxAmount = new CalculateLoanRequestDto
            {
                Amount = 10000m,
                Term = 12,
                ProductTypeId = 1
            };

            var minTerm = new CalculateLoanRequestDto
            {
                Amount = 1000,
                Term = 1,
                ProductTypeId = 1
            };

            var maxTerm = new CalculateLoanRequestDto
            {
                Amount = 5000,
                Term = 36,
                ProductTypeId = 1
            };


            await Assert.MultipleAsync(
                async () =>
                {
                    await _service
                        .Invoking(s => s.CalculateLoanDetailsAsync(minAmount))
                        .Should()
                        .ThrowAsync<ArgumentOutOfRangeException>();
                    await _service
                        .Invoking(s => s.CalculateLoanDetailsAsync(maxAmount))
                        .Should()
                        .ThrowAsync<ArgumentOutOfRangeException>();
                    await _service
                        .Invoking(s => s.CalculateLoanDetailsAsync(minTerm))
                        .Should()
                        .ThrowAsync<ArgumentOutOfRangeException>();
                    await _service
                        .Invoking(s => s.CalculateLoanDetailsAsync(maxTerm))
                        .Should()
                        .ThrowAsync<ArgumentOutOfRangeException>();
                }
            );
        }

        [Fact]
        public async Task CalculateLoan_WhenInterestTypeIsInterestFree_ReturnsCalculatedLoan()
        {
            _productTypeRepository.GetByIdAsync(2).Returns(new ProductType
            {
                ProductTypeId = 2,
                ProductName = "Personal Loan",
                InterestType = 1,
                InterestRate = 0.0m,
                InterestFreeMonths = 6,
                MinimumTerm = 6,
                MaximumTerm = 24,
                MinimumAmount = 2000,
                MaximumAmount = 25000
            });
            _globalConfigurationRepository.GetByKeyAsync(ConfigurationKeys.EstablishmentFee.ToString()).Returns(new GlobalConfiguration
            {
                Key = ConfigurationKeys.EstablishmentFee.ToString(),
                Value = "300"
            });
            var dto = new CalculateLoanRequestDto
            {
                Amount = 10000,
                Term = 12,
                ProductTypeId = 2
            };
            var result = await _service.CalculateLoanDetailsAsync(dto);

            Assert.Multiple(() =>
                {
                    result.EstablishmentFee.Should().Be(300.00m);
                    result.InterestFee.Should().Be(0.0m);
                    result.WeeklyRepayment.Should().Be(198.08m);
                    result.MonthlyRepayment.Should().Be(858.33m);
                    result.TotalRepayment.Should().Be(10300.00m);
                });
        }

        [Fact]
        public async Task CalculateLoan_WhenAmountAndTermIsMinimum_ReturnsCalculatedLoan()
        {
            _productTypeRepository.GetByIdAsync(1).Returns(new ProductType
            {
                ProductTypeId = 1,
                ProductName = "Personal Loan",
                InterestType = 0,
                InterestRate = 7.0m,
                InterestFreeMonths = 0,
                MinimumTerm = 1,
                MaximumTerm = 24,
                MinimumAmount = 1,
                MaximumAmount = 25000
            });
            _globalConfigurationRepository.GetByKeyAsync(ConfigurationKeys.EstablishmentFee.ToString()).Returns(new GlobalConfiguration
            {
                Key = ConfigurationKeys.EstablishmentFee.ToString(),
                Value = "0"
            });
            var dto = new CalculateLoanRequestDto
            {
                Amount = 1,
                Term = 1,
                ProductTypeId = 1
            };
            var result = await _service.CalculateLoanDetailsAsync(dto);

            Assert.Multiple(() =>
            {
                result.MonthlyRepayment.Should().Be(1.0058m);
                result.TotalRepayment.Should().Be(1.0058m);
                result.InterestFee.Should().Be(0.0058m);
                result.WeeklyRepayment.Should().Be(0.2321m);
            });
        }

        [Fact]
        public async Task CalculateLoan_TotalShouldMatchMonthlyTimesTerm()
        {
            _productTypeRepository.GetByIdAsync(1).Returns(new ProductType
            {
                ProductTypeId = 1,
                ProductName = "Personal Loan",
                InterestType = 0,
                InterestRate = 10.0m,
                InterestFreeMonths = 0,
                MinimumTerm = 1,
                MaximumTerm = 24,
                MinimumAmount = 1000,
                MaximumAmount = 25000
            });
            _globalConfigurationRepository.GetByKeyAsync(ConfigurationKeys.EstablishmentFee.ToString()).Returns(new GlobalConfiguration
            {
                Key = ConfigurationKeys.EstablishmentFee.ToString(),
                Value = "150"
            });
            var dto = new CalculateLoanRequestDto
            {
                Amount = 15000,
                Term = 12,
                ProductTypeId = 1
            };
            var result = await _service.CalculateLoanDetailsAsync(dto);
            var totalFromMonthly = Math.Round(result.MonthlyRepayment * dto.Term, 4, MidpointRounding.AwayFromZero);
            result.TotalRepayment.Should().Be(totalFromMonthly);
        }

        [Theory]
        [InlineData(1.225, 1.22, MidpointRounding.ToEven)]
        [InlineData(1.235, 1.24, MidpointRounding.ToEven)]
        [InlineData(1.225, 1.23, MidpointRounding.AwayFromZero)]
        public void Rounding_ShouldFollowFinancialRules(decimal input, decimal expected, MidpointRounding mode)
        {
            var result = Math.Round(input, 2, mode);

            result.Should().Be(expected);
        }
    }
}
