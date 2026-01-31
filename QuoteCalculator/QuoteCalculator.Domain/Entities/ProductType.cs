using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Domain.Entities
{
    public class ProductType
    {
        public int ProductTypeId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int MinimumTerm { get; set; }
        public int MaximumTerm { get; set; }
        public decimal MinimumAmount { get; set; }
        public decimal MaximumAmount { get; set; }
        public int InterestType { get; set; }
        public int InterestFreeMonths { get; set; }
        public decimal InterestRate { get; set; }
    }
}
