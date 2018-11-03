using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClothesSupplyApi.Models;

namespace ClothesSupplyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ProductCatalogDbContext _context;

        public FilesController(ProductCatalogDbContext context)
        {
            _context = context;
        }

        // GET: api/Files
        [HttpGet]
        public IEnumerable<Files> GetFiles()
        {
            return _context.Files;
        }

        // GET: api/Files/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFiles([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var files = await _context.Files.FindAsync(id);

            if (files == null)
            {
                return NotFound();
            }

            return Ok(files);
        }

        // PUT: api/Files/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFiles([FromRoute] int id, [FromBody] Files files)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != files.Id)
            {
                return BadRequest();
            }

            _context.Entry(files).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Files
        [HttpPost]
        public async Task<IActionResult> PostFiles([FromBody] Files files)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.Files.Any(a => a.FileName == files.FileName))
            {
                var cFile = await _context.Files.Where(a => a.FileName == files.FileName).FirstOrDefaultAsync();
                return CreatedAtAction("GetFiles", new { id = cFile.Id }, cFile);
            }
            else
            {
                _context.Files.Add(files);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetFiles", new { id = files.Id }, files);
            }
        }

        // DELETE: api/Files/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFiles([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var files = await _context.Files.FindAsync(id);
            if (files == null)
            {
                return NotFound();
            }

            _context.Files.Remove(files);
            await _context.SaveChangesAsync();

            return Ok(files);
        }

        private bool FilesExists(int id)
        {
            return _context.Files.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("/api/getfilebyname/{fileName}")]
        public async Task<Files> GetFileByName([FromRoute]string fileName)
        {
            var file = await _context.Files.Where(a => a.FileName == fileName).FirstOrDefaultAsync();
            return file;
        }
    }
}