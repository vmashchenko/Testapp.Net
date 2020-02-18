namespace TestApp.Domain
{
    public interface ISorting
    {
        string SortBy { get; }

        SortDirection SortDir { get; }
    }
}
