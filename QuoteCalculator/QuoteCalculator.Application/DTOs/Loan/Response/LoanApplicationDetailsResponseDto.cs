using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Application.DTOs.Loan.Response
{
    public class LoanApplicationDetailsResponseDto
    {
        public Guid LoanApplicationPublicId { get; set; }
        public int ApplicantId { get; set; }
        public string Title { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public string MobileNumber { get; set; } = default!;
        public string Email { get; set; } = default!;
        public decimal Amount { get; set; }
        public int Term { get; set; }
        public int ProductType { get; set; }
    }
}
