using System.Collections.Generic;
using System.IO;
using TestApp.Core.Guard;
using TestApp.Domain.File;

namespace TestApp.Services
{
    public sealed class Raw2Parser : FileParseBase
    {
        public const int DataOffset = 30;
        public const string Delimiter = "|";

        public Raw2Parser()
            : base(FileType.Raw2)
        {
        }

        public override int DataOffsetPosition => DataOffset;

        protected override IList<FieldVm> ParseImpl(Stream stream)
        {
            // here we parse file RAW2 based on his structure
            return new List<FieldVm>();
        }
    }
}
