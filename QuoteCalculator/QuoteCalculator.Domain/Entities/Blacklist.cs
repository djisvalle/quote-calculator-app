using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Domain.Entities
{
    public class Blacklist
    {
        public int BlacklistId { get; set; }
        public int BlacklistType { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
