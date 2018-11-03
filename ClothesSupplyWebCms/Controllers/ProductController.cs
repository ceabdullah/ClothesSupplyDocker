using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClothesSupplyWebCms.Models;
using ClothesSupplyWebCms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace ClothesSupplyWebCms.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly IFilesService _filesService;

        public ProductController(IProductsService productsService, IFilesService filesService)
        {
            _productsService = productsService;
            _filesService = filesService;
        }

        // GET: Product
        public async Task<IActionResult> Index(string productName)
        {
            var products = await _productsService.GetProduct();

            products = String.IsNullOrEmpty(productName) ? products : products.Where(a => a.Name.ToLower().Contains(productName.ToLower()));

            return View(products);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productsService.GetProduct(id);
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            Files cFile = null;
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    if (file.Length > 0)
                    {

                        byte[] data = new byte[file.Length];
                        file.OpenReadStream().Read(data);

                        cFile = await _filesService.PostFiles(new Models.Files
                        {
                            File = data,
                            FileContentType = file.ContentType,
                            FileExtension = System.IO.Path.GetExtension(file.FileName),
                            FileName = file.FileName
                        });
                    }
                }

                await _productsService.PostProduct(new ProductDto
                {
                    LastUpdated = DateTime.Now,
                    Name = collection["Name"],
                    Photo = cFile == null ? null : "/file/" + cFile.FileName,
                    Price = Double.Parse(collection["Price"])
                });

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productsService.GetProduct(id);
            HttpContext.Session.SetString("cProduct", JsonConvert.SerializeObject(product));
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {
            ProductDto cProduct = null;
            Files cFile = null;
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    if (file.Length > 0)
                    {

                        byte[] data = new byte[file.Length];
                        file.OpenReadStream().Read(data);

                        cFile = await _filesService.PostFiles(new Models.Files
                        {
                            File = data,
                            FileContentType = file.ContentType,
                            FileExtension = System.IO.Path.GetExtension(file.FileName),
                            FileName = file.FileName
                        });
                    }
                }

                cProduct = await _productsService.PutProduct(id, new ProductDto
                {
                    Id = id,
                    LastUpdated = DateTime.Now,
                    Name = collection["Name"],
                    Photo = cFile == null ? JsonConvert.DeserializeObject<ProductDto>(HttpContext.Session.GetString("cProduct")).Photo : "/file/" + cFile.FileName,
                    Price = Double.Parse(collection["Price"])
                });

                HttpContext.Session.Remove("cProduct");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(cProduct);
            }
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productsService.GetProduct(id);
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await _productsService.DeleteProduct(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Route("/file/{fileName}")]
        [HttpGet]
        public async Task<IActionResult> GetFile([FromRoute]string fileName)
        {
            Files file = await _filesService.GetFile(fileName);

            if (file == null)
            {
                return NotFound();
            }

            return File(file.File, file.FileContentType);
        }

        [Route("/ExportToExcel")]
        public async Task<IActionResult> ExportToExcel()
        {

            var comlumHeadrs = new string[]
            {
                "Product Name",
                "Product Price",
                "Product Last Update",
                "Product Photo"
            };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Products");
                using (var cells = worksheet.Cells[1, 1, 1, 5])
                {
                    cells.Style.Font.Bold = true;
                }

                for (var i = 0; i < comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                }

                var products = await _productsService.GetProduct();

                var j = 2;
                foreach (var product in products)
                {
                    worksheet.Cells["A" + j].Value = product.Name;
                    worksheet.Cells["B" + j].Value = product.Price;
                    worksheet.Cells["C" + j].Value = product.LastUpdated.ToString();
                    worksheet.Cells["D" + j].Value = product.Photo;

                    j++;
                }
                result = package.GetAsByteArray();
            }

            return File(result, "application/ms-excel", $"Products_{DateTime.Now.ToString()}.xlsx");
        }

    }
}