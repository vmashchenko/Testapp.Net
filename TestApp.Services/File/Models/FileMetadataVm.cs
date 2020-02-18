using System.Collections.Generic;
using TestApp.Domain.File;

namespace TestApp.Services
{
    public sealed class FileMetadataVm
    {
        public FileMetadataVm()
        {
            Fields = new List<FieldVm>();
        }

        public FileType Type { get; set; }

        public IList<FieldVm> Fields { get; set; }
    }
}
