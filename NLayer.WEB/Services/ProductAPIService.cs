using NLayer.Core.DTOs;

namespace NLayer.Web.Services
{
    public class ProductAPIService
    {
        private readonly HttpClient _httpClient;

        public ProductAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductWithCategoryDto>> GetProductWithCategoryAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDTO<List<ProductWithCategoryDto>>>("product/GetproductWithCategory");
            return response.Data;
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDTO<ProductDto>>($"product/{id}");
            return response.Data;
        }

        public async Task<ProductDto> SaveAsync(ProductDto newProduct)
        {
            var response = await _httpClient.PostAsJsonAsync("product", newProduct);
            if (!response.IsSuccessStatusCode) return null;
            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDTO<ProductDto>>();
            return responseBody.Data;
        }


        public async Task<bool> UpdateAsync(ProductDto newProduct)
        {
            var response= await _httpClient.PutAsJsonAsync("product", newProduct);

            return response.IsSuccessStatusCode;
        }


        public async Task<bool> RemoveAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"product/{id}");

            return response.IsSuccessStatusCode;
        }


    }
}
