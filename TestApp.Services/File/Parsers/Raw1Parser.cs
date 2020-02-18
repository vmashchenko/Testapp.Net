using System.Collections.Generic;
using System.IO;
using TestApp.Domain.File;

namespace TestApp.Services
{
    public sealed class Raw1Parser : FileParseBase
    {
        public const int DataOffset = 10;

        public Raw1Parser()
            : base(FileType.Raw1)
        {
        }

        public override int DataOffsetPosition => DataOffset;

        protected override IList<FieldVm> ParseImpl(Stream stream)
        {
            // here we parse file RAW1 based on his structure
            return new List<FieldVm>();
        }
    }
}
