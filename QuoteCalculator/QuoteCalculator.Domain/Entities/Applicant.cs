using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Domain.Entities
{
    public class Applicant
    {
        public int ApplicantId { get; set; }
        public string Title { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public string MobileNumber { get; set; } = default!;
        public string Email { get; set; } = default!;
        public uint RowVersion { get; set; }
    }
}
