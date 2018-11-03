using System;
using System.Collections.Generic;

namespace ClothesSupplyApi.Models
{
    public partial class Files
    {
        public int Id { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string FileContentType { get; set; }
        public string FileExtension { get; set; }
    }
}
