using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Domain.Entities
{
    public class LoanApplication
    {
        public int LoanApplicationId { get; set; }
        public Guid LoanApplicationPublicId { get; set; }
        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; } = default!;
        public decimal Amount { get; set; }
        public int Term { get; set; }
        public int ProductType { get; set; }
        public int Status { get; set; }
        public decimal EstablishmentFee { get; set; }
        public decimal InterestFee { get; set; }
        public decimal WeeklyRepayment { get; set; }
        public decimal MonthlyRepayment { get; set; }
        public decimal TotalRepayment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? Remarks { get; set; }
        public uint RowVersion { get; set; }
    }
}
