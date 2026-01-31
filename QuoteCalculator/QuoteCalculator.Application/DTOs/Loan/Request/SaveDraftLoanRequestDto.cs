using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuoteCalculator.Application.DTOs.Loan.Request
{
    public class SaveDraftLoanRequestDto
    {
        [Required]
        public Guid LoanApplicationPublicId { get; set; }

        [Required]
        public int ApplicantId { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required string MobileNumber { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public int ProductTypeId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int Term { get; set; }
    }
}
