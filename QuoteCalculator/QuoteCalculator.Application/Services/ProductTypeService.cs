using AutoMapper;
using QuoteCalculator.Application.DTOs.ProductType.Response;
using QuoteCalculator.Application.Interfaces;
using QuoteCalculator.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuoteCalculator.Application.Services
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IMapper _mapper;

        public ProductTypeService(IProductTypeRepository productTypeRepository, IMapper mapper)
        {
            _productTypeRepository = productTypeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductTypeResponseDto>> GetAllAsync()
        {
            var productTypes = await _productTypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductTypeResponseDto>>(productTypes);
        }
    }
}
