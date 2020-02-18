using AutoMapper;
using TestApp.Domain.File;

namespace TestApp.Services
{
    public class FileMappings : Profile
    {
        public FileMappings()
        {
            CreateMap<FileEntity, FileVm>()
                .ReverseMap()
                ;

            CreateMap<FileEntity, FileDetailsVm>()
                .ReverseMap()
                ;
        }
    }
}
