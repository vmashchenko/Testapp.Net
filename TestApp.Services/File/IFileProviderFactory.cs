namespace TestApp.Services
{
    public interface IFileProviderFactory
    {
        FileMetadataVm GetMetadata(byte[] content);
    }
}
