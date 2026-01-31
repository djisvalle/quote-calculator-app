using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Application.DTOs.ProductType.Response
{
    public class ProductTypeResponseDto
    {
        public int ProductTypeId { get; set; }
        public required string ProductName { get; set; }
        public int MinimumTerm { get; set; }
        public int MaximumTerm { get; set; }
        public decimal MinimumAmount { get; set; }
        public decimal MaximumAmount { get; set; }
    }
}
