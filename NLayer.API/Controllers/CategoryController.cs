using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Entities;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class CategoryController : _CustomBaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService,
            IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [ServiceFilter(typeof(NotFoundFilter<Category>))]
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var response = await _categoryService.GetByIdAsync(Id);
            var responseDto = _mapper.Map<CategoryDto>(response);
            return CreateActionResult(CustomResponseDTO<CategoryDto>.Success(200, responseDto));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            
            //Mapleyerek CategoryDTO nesnesine çevrilmesi için kullanılır.
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            return CreateActionResult(CustomResponseDTO<List<CategoryDto>>.Success(200, categoriesDto));
        }

        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetSingleCategoryByIdWithProducts(int categoryId)
        {
            return CreateActionResult(await _categoryService.GetSingleCategoryByIdWithProductsAsync(categoryId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDto request)
        {
            var category = await _categoryService.AddAsync(_mapper.Map<Category>(request));
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return CreateActionResult(CustomResponseDTO<CategoryDto>.Success(200, categoryDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryDto request)
        {

            await _categoryService.UpdateAsync(_mapper.Map<Category>(request));
            return CreateActionResult(CustomResponseDTO<CategoryDto>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var category = await _categoryService.GetByIdAsync(Id);
            await _categoryService.RemoveAsync(category);
            return CreateActionResult(CustomResponseDTO<CategoryDto>.Success(204));
        }

    }
}