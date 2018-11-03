using ClothesSupplyWebCms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClothesSupplyWebCms.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly HttpClient _apiClient;

        public ProductsService(IOptions<AppSettings> settings, HttpClient httpClient)
        {
            ////TODO : omit bypassing certificate error on prod environment
            //HttpClientHandler handler = new HttpClientHandler();
            //handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            //httpClient = new HttpClient(handler);


            _settings = settings;
            _apiClient = httpClient;
        }

        public async Task<HttpResponseMessage> DeleteProduct([FromRoute] int id)
        {
            string _productApiUrl = $"{_settings.Value.ClothesSupplyApiUrl}/api/Products/{id}";

            var responseString = await _apiClient.DeleteAsync(_productApiUrl);

            return responseString;
        }

        public async Task<IEnumerable<ProductDto>> GetProduct()
        {
            string _productApiUrl = $"{_settings.Value.ClothesSupplyApiUrl}/api/Products";

            var responseString = await _apiClient.GetStringAsync(_productApiUrl);

            return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(responseString);
        }

        public async Task<ProductDto> GetProduct([FromRoute] int id)
        {
            string _productApiUrl = $"{_settings.Value.ClothesSupplyApiUrl}/api/Products/{id}";

            var responseString = await _apiClient.GetStringAsync(_productApiUrl);

            return JsonConvert.DeserializeObject<ProductDto>(responseString);
        }

        public async Task<ProductDto> PostProduct([FromBody] ProductDto product)
        {
            string _productApiUrl = $"{_settings.Value.ClothesSupplyApiUrl}/api/Products";

            var productContent = new StringContent(JsonConvert.SerializeObject(product), System.Text.Encoding.UTF8, "application/json-patch+json");

            var response = await _apiClient.PostAsync(_productApiUrl, productContent);

            response.EnsureSuccessStatusCode();

            return product;
        }

        public async Task<ProductDto> PutProduct([FromRoute] int id, [FromBody] ProductDto product)
        {
            string _productApiUrl = $"{_settings.Value.ClothesSupplyApiUrl}/api/Products/{id}";

            var productContent = new StringContent(JsonConvert.SerializeObject(product), System.Text.Encoding.UTF8, "application/json-patch+json");

            var response = await _apiClient.PutAsync(_productApiUrl, productContent);

            response.EnsureSuccessStatusCode();

            return product;
        }
    }
}
