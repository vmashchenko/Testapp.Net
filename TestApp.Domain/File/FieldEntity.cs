using System.ComponentModel.DataAnnotations;

namespace TestApp.Domain.File
{
    public class FieldEntity : EntityBase<long>
    {
        public long FileId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Value { get; set; }

        public FileEntity File { get; set; }
    }
}
