using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Entities;
using NLayer.Core.Services;
//using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class ProductController : _CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductController(
            IMapper mapper,
            IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        // api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            var productsDto = _mapper.Map<List<ProductDto>>(products.ToList());
            return CreateActionResult(CustomResponseDTO<List<ProductDto>>.Success(200, productsDto));
        }

        //www.mysite.com/api/products/5 anlamında kullanılır
        //Daha kod bloğu başlamadan NotFountFilter classına Id parametresi ile giderek olup olmadığını sorgular. 
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var product = await _productService.GetByIdAsync(Id);
            var productDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDTO<ProductDto>.Success(200, productDto));
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            var product = await _productService.AddAsync(_mapper.Map<Product>(productDto));
            var responseDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDTO<ProductDto>.Success(201, responseDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productDto)
        {
            await _productService.UpdateAsync(_mapper.Map<Product>(productDto));
            return CreateActionResult(CustomResponseDTO<NoContentDto>.Success(204));
        }

        //www.mysite.com/api/products/5
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Remove(int Id)
        {
            var product = await _productService.GetByIdAsync(Id);
            await _productService.RemoveAync(product);
            return CreateActionResult(CustomResponseDTO<NoContentDto>.Success(204));
        }


        // api/products/GetProductsWithCategory
        //otomatik olarak Action ın adını alır
        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            return CreateActionResult(await _productService.GetProductsWitCategoryAsync());
        }
    }
}
