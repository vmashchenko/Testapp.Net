using System.Collections.Generic;
using System.IO;
using TestApp.Core.Guard;
using TestApp.Domain.File;

namespace TestApp.Services
{
    public abstract class FileParseBase : IFileParser
    {
        protected FileParseBase(FileType type)
        {
            Type = type;
        }

        public FileType Type { get; }

        public abstract int DataOffsetPosition { get; }

        public IList<FieldVm> Parse(Stream stream)
        {
            Throw.IfNull(stream, nameof(stream));

            stream.Seek(DataOffsetPosition, SeekOrigin.Begin);

            return ParseImpl(stream);
        }

        protected abstract IList<FieldVm> ParseImpl(Stream stream);
    }
}
