using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Entities;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class ProductController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IService<Product> _service;

        public ProductController(
            IMapper mapper,
            IService<Product> service)
        {
            _mapper = mapper;
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllAsync();
            var productsDto = _mapper.Map<List<ProductDto>>(products.ToList());
            return CreateActionResult(CustomResponseDTO<List<ProductDto>>.Success(200, productsDto));
        }

        //www.mysite.com/api/products/5 anlamında kullanılır
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var product = await _service.GetByIdAsync(Id);
            var productDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDTO<ProductDto>.Success(200, productDto));
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
            var responseDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDTO<ProductDto>.Success(201, responseDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productDto)
        {
            await _service.UpdateAsync(_mapper.Map<Product>(productDto));
            return CreateActionResult(CustomResponseDTO<NoContentDto>.Success(204));
        }

        //www.mysite.com/api/products/5
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Remove(int Id)
        {
            var product = await _service.GetByIdAsync(Id);
            await _service.RemoveAync(product);          
            return CreateActionResult(CustomResponseDTO<NoContentDto>.Success(204));
        }
    }
}
