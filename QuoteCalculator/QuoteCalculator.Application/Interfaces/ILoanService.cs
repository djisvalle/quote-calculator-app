using QuoteCalculator.Application.DTOs.Loan.Request;
using QuoteCalculator.Application.DTOs.Loan.Response;

namespace QuoteCalculator.Application.Interfaces
{
    public interface ILoanService
    {
        Task<string> ProcessLoanAsync(CreateLoanRequestDto dto);
        Task<LoanApplicationDetailsResponseDto?> GetLoanApplicationDetailsAsync(Guid publicId);
        Task SaveDraftLoanApplicationAsync(SaveDraftLoanRequestDto dto);
        Task<CalculateLoanResponseDto> CalculateLoanDetailsAsync(CalculateLoanRequestDto dto);
        Task FinalizeLoanAsync(FinalizeLoanRequestDto dto);
        Task<ApplicationDetailsResponseDto> GetReceiptDetailsAsync(Guid publicId);
    }
}
