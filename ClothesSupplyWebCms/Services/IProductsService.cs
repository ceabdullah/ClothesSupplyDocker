using ClothesSupplyWebCms.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClothesSupplyWebCms.Services
{
    public interface IProductsService
    {
        Task<ProductDto> PostProduct([FromBody] ProductDto product);
        Task<HttpResponseMessage> DeleteProduct([FromRoute] int id);
        Task<ProductDto> PutProduct([FromRoute] int id, [FromBody] ProductDto product);
        Task<IEnumerable<ProductDto>> GetProduct();
        Task<ProductDto> GetProduct([FromRoute] int id);
    }
}
