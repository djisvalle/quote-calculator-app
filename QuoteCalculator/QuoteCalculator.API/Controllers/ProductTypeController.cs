using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuoteCalculator.Application.Interfaces;

namespace QuoteCalculator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService _productTypeService;

        public ProductTypeController(IProductTypeService productTypeService) => _productTypeService = productTypeService;

        [HttpGet]
        public async Task<ActionResult> GetAll() => Ok(await _productTypeService.GetAllAsync());
    }
}
