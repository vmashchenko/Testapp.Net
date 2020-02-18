namespace TestApp.Domain
{
    /// <summary> Search, sorting and paging </summary>
    public class Criteria : IPaging, ISorting
    {
        public string Search { get; set; }

        public int? PageSize { get; set; }

        public int? PageNumber { get; set; }

        public string SortBy { get; set; }

        public SortDirection SortDir { get; set; }

        public static Criteria Empty => new Criteria();
    }
}
