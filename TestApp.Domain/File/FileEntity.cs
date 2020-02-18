using System;
using System.ComponentModel.DataAnnotations;

namespace TestApp.Domain.File
{
    public class FileEntity : EntityBase<long>
    {
        [Required]
        public string FileName { get; set; }

        public FileType Type { get; set; }

        [Required]
        public byte[] Content { get; set; }

        [MaxLength(20)]
        public string ContentType { get; set; }

        public long Size { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
