using QuoteCalculator.Application.DTOs.ProductType.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Application.Interfaces
{
    public interface IProductTypeService
    {
        Task<IEnumerable<ProductTypeResponseDto>> GetAllAsync();
    }
}
