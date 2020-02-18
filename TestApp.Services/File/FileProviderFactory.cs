using System;
using System.IO;
using TestApp.Core.Guard;
using TestApp.Domain.File;

namespace TestApp.Services
{
    public sealed class FileProviderFactory : IFileProviderFactory
    {
        public const int HeaderLength = 4;

        private readonly System.Text.Encoding _encoding;

        public FileProviderFactory()
        {
            _encoding = System.Text.Encoding.UTF8;
        }

        public FileMetadataVm GetMetadata(byte[] content)
        {
            Throw.IfNull(content, nameof(content));

            if (content.Length < HeaderLength)
            {
                throw new InvalidOperationException("Could not detect file ");
            }

            using (MemoryStream ms = new MemoryStream(content))
            {
                var reader = new BinaryReader(ms);
                byte[] headerBytes = reader.ReadBytes(HeaderLength);
                string header = _encoding.GetString(headerBytes);

                IFileParser parser = null;
                switch (header.ToLower())
                {
                    case "raw1":
                        parser = new Raw1Parser();
                        break;

                    case "raw2":
                        parser = new Raw2Parser();
                        break;
                }

                if (parser == null)
                {
                    return new FileMetadataVm() { Type = FileType.Unknown };
                }

                var fields = parser.Parse(ms);
                return new FileMetadataVm() { Fields = fields, Type = parser.Type };
            }            
        }
    }
}
