using NLayer.Core.DTOs;
using NLayer.Core.Entities;

namespace NLayer.Core.Services
{
    public interface IProductService : IService<Product>
    {
        Task<CustomResponseDTO<List<ProductWithCategoryDto>>> GetProductsWithCategory();


    }
}
