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
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDTO<List<CategoryDto>>>("Category");
            return response.Data;
        }
        public async Task<CategoryDto> GetById(int Id)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDTO<CategoryDto>>($"Category/{Id}");
            return response.Data;
        }
        public async Task<CategoryDto> Create(CategoryDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("Category", request);
            if (!response.IsSuccessStatusCode) return null;
            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDTO<CategoryDto>>();
            return responseBody.Data;
        }


        public async Task<bool> Remove(int Id)
        {
            var response = await _httpClient.DeleteAsync($"Category/{Id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(CategoryDto request)
        {
            var response = await _httpClient.PutAsJsonAsync("Category",request);
            return response.IsSuccessStatusCode;
        }
    }
}
