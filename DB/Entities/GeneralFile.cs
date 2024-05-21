using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Entities
{
    [Table("general_file")]
    public class GeneralFile
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string OriginalName { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Path { get; set; }

        [StringLength(200)]
        public string ThumbnailFilePath { get; set; }

        public int? Size { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(200)]
        public string MimeType { get; set; }

        [StringLength(50)]
        public string Extension { get; set; }

        public DateTime? UploadDT { get; set; }
    }
}
