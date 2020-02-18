using System;
using System.Collections.Generic;
using TestApp.Domain.File;

namespace TestApp.Services
{
    public class FileVmBase
    {
        public string FileName { get; set; }

        public string ContentType { get; set; }        
    }

    public class FileDetailsVm : FileVmBase
    {
        public FileDetailsVm()
        {
            Fields = new List<FieldVm>();
        }

        public FileType Type { get; set; }

        public IList<FieldVm> Fields { get; set; }

        public long Size { get; set; }

        public DateTime CreatedOn { get; set; }               
    }

    public class FileVm : FileVmBase
    {
        public byte[] Content { get; set; }        

        public long Size => Content.Length;
    }
}
