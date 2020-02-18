using AutoMapper;
using TestApp.Domain.File;

namespace TestApp.Services
{
    public class FieldMappings : Profile
    {
        public FieldMappings()
        {
            CreateMap<FieldEntity, FieldVm>()
                .ReverseMap()
                ;
        }
    }
}
