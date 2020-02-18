using System.Threading.Tasks;

namespace TestApp.Services
{
    public interface IFileService
    {
        Task<FileDetailsVm> Get(long id);

        Task<long> Upload(FileVm vm);        
    }
}
