namespace TestApp.DB.Repositories
{
    using TestApp.Domain;
    using TestApp.Domain.File;    

    public class FilleRepository : RepositoryBase<FileEntity>, IFileRepository
    {
        public FilleRepository(AppDbContext dbContext, ICurrentUserProvider currentUserProvider)
            : base(dbContext, currentUserProvider)
        {
        }
    }
}
