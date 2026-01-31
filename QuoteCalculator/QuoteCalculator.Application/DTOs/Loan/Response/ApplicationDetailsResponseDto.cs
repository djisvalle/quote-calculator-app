using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Application.DTOs.Loan.Response
{
    public class ApplicationDetailsResponseDto
    {
        public required string FirstName { get; set; }
        public required string Email { get; set; }
        public decimal Amount { get; set; }
        public string? ReferenceNumber { get; set; }
        public int Status { get; set; } = default!;        
    }
}
