﻿using NLayer.Core.DTOs;
using NLayer.Core.Entities;

namespace NLayer.Core.Services
{
    public interface ICategoryService : IService<Category>
    {
        public Task<CustomResponseDTO<CategoryWithProductsDto>> GetSingleCategoryByIdWithProductsAsync(int categoryId);

    }
}
