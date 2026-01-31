using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace QuoteCalculator.Application.DTOs.Loan.Request
{
    public class CreateLoanRequestDto
    {
        [Required]
        [JsonPropertyName("AmountRequired")]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Amount cannot be zero.")]
        public decimal Amount { get; set; }

        [Required]
        public int Term { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters long.")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Only letters are allowed.")]
        public required string FirstName { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters long.")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Only letters are allowed.")]
        public required string LastName { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [JsonPropertyName("Mobile")]
        public required string MobileNumber { get; set; }

        [Required]
        public required string Email { get; set; }
    }
}
