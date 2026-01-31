using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuoteCalculator.Application.DTOs.Loan.Request
{
    public class CalculateLoanRequestDto
    {
        [Required]
        public int ProductTypeId { get; set; }

        [Required]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Amount cannot be zero.")]
        public decimal Amount { get; set; }

        [Required]
        public int Term { get; set; }
    }
}
