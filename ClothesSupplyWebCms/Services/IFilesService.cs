using ClothesSupplyWebCms.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesSupplyWebCms.Services
{
    public interface IFilesService
    {
        Task<Files> GetFile(string fileName);
        Task<Files> PostFiles(Files files);
    }
}
