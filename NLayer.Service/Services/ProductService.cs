using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Entities;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class ProductService : Service<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(
            IGenericRepository<Product> genericRepository,
            IUnitOfWorkService unitOfWorkService,
            IProductRepository productService,
            IMapper mapper) 
            : base(genericRepository, unitOfWorkService)
        {
            _productRepository = productService;
            _mapper = mapper;
        }

        public async Task<CustomResponseDTO<List<ProductWithCategoryDto>>> GetProductsWitCategoryAsync()
        {
            var products = await _productRepository.GetProductsWitCategoryAsync();
            var productsDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
            return CustomResponseDTO<List<ProductWithCategoryDto>>.Success(200,productsDto);
        }
    }
}
