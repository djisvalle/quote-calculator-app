using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuoteCalculator.Application.DTOs.Loan.Request
{
    public class FinalizeLoanRequestDto
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
        public decimal Amount { get; set; }

        [Required]
        public int Term { get; set; }

        [Required]
        public decimal InterestFee { get; set; }

        [Required]
        public decimal WeeklyRepayment { get; set; }

        [Required]
        public decimal MonthlyRepayment { get; set; }

        [Required]
        public decimal TotalRepayment { get; set; }
    }
}
