using NLayer.Core.DTOs;

namespace NLayer.Web.Services
{
    public class CategoryAPIService
    {
        private readonly HttpClient _httpClient;

        public CategoryAPIService(HttpClient httpClient)
        {

           
            _httpClient = httpClient;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDTO<List<CategoryDto>>>("categories");
            return response.Data;
        }

    }
}
