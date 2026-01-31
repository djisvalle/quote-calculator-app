using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace QuoteCalculator.Application.DTOs.Loan.Response
{
    public class CalculateLoanResponseDto
    {
        public decimal Amount { get; set; }
        public int Term { get; set; }
        public decimal EstablishmentFee { get; set; }
        public decimal InterestFee { get; set; }
        public decimal WeeklyRepayment { get; set; }
        public decimal MonthlyRepayment { get; set; }
        public decimal TotalRepayment { get; set; }
    }
}
