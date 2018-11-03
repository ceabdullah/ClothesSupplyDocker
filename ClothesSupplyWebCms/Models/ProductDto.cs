using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClothesSupplyWebCms.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public string Photo { get; set; }
        [Required]
        public double? Price { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
