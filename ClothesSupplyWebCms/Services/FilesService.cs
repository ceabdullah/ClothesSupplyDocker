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
    public class FilesService : IFilesService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly HttpClient _apiClient;

        public FilesService(IOptions<AppSettings> settings, HttpClient httpClient)
        {
            _settings = settings;
            _apiClient = httpClient;
        }

        public async Task<Files> GetFile(string fileName)
        {
            string _fileApiUrl = $"{_settings.Value.ClothesSupplyApiUrl}/api/getfilebyname/{fileName}";

            var responseString = await _apiClient.GetStringAsync(_fileApiUrl);

            return JsonConvert.DeserializeObject<Files>(responseString);
        }

        public async Task<Files> PostFiles(Files files)
        {
            string _fileApiUrl = $"{_settings.Value.ClothesSupplyApiUrl}/api/Files";

            var fileContent = new StringContent(JsonConvert.SerializeObject(files), System.Text.Encoding.UTF8, "application/json-patch+json");

            var response = await _apiClient.PostAsync(_fileApiUrl, fileContent);

            response.EnsureSuccessStatusCode();

            return files;
        }
    }
}
