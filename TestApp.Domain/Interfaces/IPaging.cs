namespace TestApp.Domain
{
    public interface IPaging
    {
        int? PageSize { get; }

        int? PageNumber { get; }
    }
}
