using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Domain.Entities
{
    public class GlobalConfiguration
    {
        public int GlobalConfigurationId { get; set; }
        public required string Key { get; set; }
        public required string Value { get; set; }
    }
}
