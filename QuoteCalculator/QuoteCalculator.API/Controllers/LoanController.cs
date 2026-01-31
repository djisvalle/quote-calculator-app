using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuoteCalculator.Application.DTOs.Loan.Request;
using QuoteCalculator.Application.DTOs.Loan.Response;
using QuoteCalculator.Application.Interfaces;

namespace QuoteCalculator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService) => _loanService = loanService;

        [HttpPost]
        public async Task<ActionResult<string>> ProcessLoan(CreateLoanRequestDto dto) => 
            Ok(await _loanService.ProcessLoanAsync(dto));

        [HttpGet("{publicId:guid}")]
        public async Task<IActionResult> GetApplicationDetails(Guid publicId)
        {
            var result = await _loanService.GetLoanApplicationDetailsAsync(publicId);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPatch("save-draft/{publicId:guid}")]
        public async Task<IActionResult> SaveDraftApplication(Guid publicId, [FromBody] SaveDraftLoanRequestDto dto)
        {
            if (publicId != dto.LoanApplicationPublicId) 
                return BadRequest("Loan Application ID does not match Loan Application ID in the body.");

            await _loanService.SaveDraftLoanApplicationAsync(dto);
            return NoContent();
        }

        [HttpPost("calculate")]
        public async Task<ActionResult<CalculateLoanResponseDto>> CalculateLoanDetails(CalculateLoanRequestDto dto) =>
            Ok(await _loanService.CalculateLoanDetailsAsync(dto));

        [HttpPut("{publicId:guid}")]
        public async Task<IActionResult> FinalizeLoan(Guid publicId, [FromBody] FinalizeLoanRequestDto dto)
        {
            if (publicId != dto.LoanApplicationPublicId) 
                return BadRequest("Loan Application ID does not match Loan Application ID in the body.");

            await _loanService.FinalizeLoanAsync(dto);
            return NoContent();
        }

        [HttpGet("receipt-details/{publicId:guid}")]
        public async Task<IActionResult> GetReceiptDetails(Guid publicId)
        {
            var result = await _loanService.GetReceiptDetailsAsync(publicId);
            if (result == null) return NotFound();

            return Ok(result);
        }
    }
}
