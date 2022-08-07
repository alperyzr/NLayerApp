using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.DTOs;
using NLayer.Core.Entities;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Caching
{
    public class ProductServiceWithCaching : IProductService
    {
        private const string CacheProductKey = "productCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWorkService _unitOfWorkService;

        public ProductServiceWithCaching(IMapper mapper,
            IMemoryCache memoryCache,
            IProductRepository productRepository,
            IUnitOfWorkService unitOfWorkService)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _productRepository = productRepository;
            _unitOfWorkService = unitOfWorkService;

            if (!_memoryCache.TryGetValue(CacheProductKey, out _))
            {
                //constracture içerisinde await kullanılmadığı için .Result kullanarak senkron hale çevirdik
                _memoryCache.Set(CacheProductKey, _productRepository.GetProductsWitCategoryAsync().Result);
            }
        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _productRepository.AddAsync(entity);
            await _unitOfWorkService.CommitAsync();
            await CacheAllProductsAsync();
            return entity;

        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _productRepository.AddRangeAsync(entities);
            await _unitOfWorkService.CommitAsync();
            await CacheAllProductsAsync();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {      
            return Task.FromResult(_memoryCache.Get<IEnumerable<Product>>(CacheProductKey));
        }

        public Task<Product> GetByIdAsync(int Id)
        {
            var product = _memoryCache.Get<List<Product>>(CacheProductKey).FirstOrDefault(x => x.Id == Id);
            if (product == null)
            {
                throw new NotFoundException($"{typeof(Product)}({Id}) not found");
            }

            //await ve ya asnc kullanılmadığı için Task ile dönmenin başka bir yolu
            return Task.FromResult(product);
        }

        public Task<CustomResponseDTO<List<ProductWithCategoryDto>>> GetProductsWitCategoryAsync()
        {
            var products = _memoryCache.Get<IEnumerable<Product>>(CacheProductKey);
            var productsWithCategoryDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
            return Task.FromResult(CustomResponseDTO<List<ProductWithCategoryDto>>.Success(200,productsWithCategoryDto));

        }

        public async Task RemoveAync(Product entity)
        {
            _productRepository.Remove(entity);
            await _unitOfWorkService.CommitAsync();
            await CacheAllProductsAsync();

        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            _productRepository.RemoveRange(entities);
            await _unitOfWorkService.CommitAsync();
            await CacheAllProductsAsync();

        }

        public async Task<Product> UpdateAsync(Product entity)
        {
            _productRepository.Update(entity);
            await _unitOfWorkService.CommitAsync();
            await CacheAllProductsAsync();
            return entity;
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            return _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
        }

        public async Task CacheAllProductsAsync()
        {
            _memoryCache.Set(CacheProductKey, await _productRepository.GetAll().ToListAsync());
        }
    }
}
