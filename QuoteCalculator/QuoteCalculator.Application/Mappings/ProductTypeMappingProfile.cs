using AutoMapper;
using QuoteCalculator.Application.DTOs.Loan.Request;
using QuoteCalculator.Application.DTOs.ProductType.Response;
using QuoteCalculator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Application.Mappings
{
    public class ProductTypeMappingProfile : Profile
    {
        public ProductTypeMappingProfile()
        {
            CreateMap<ProductType, ProductTypeResponseDto>();
        }
    }
}
