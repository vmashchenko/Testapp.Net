using System.IO;
using System.Collections.Generic;

using TestApp.Domain.File;

namespace TestApp.Services
{
    public interface IFileParser
    {
        public FileType Type { get; }

        public int DataOffsetPosition { get; }

        IList<FieldVm> Parse(Stream stream);
    }
}
