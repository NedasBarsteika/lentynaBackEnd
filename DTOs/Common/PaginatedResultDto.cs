namespace lentynaBackEnd.DTOs.Common
{
    public class PaginatedResultDto<T>
    {
        public List<T> items { get; set; } = new();
        public int page { get; set; }
        public int pageSize { get; set; }
        public int totalCount { get; set; }
        public int totalPages { get; set; }
        public bool hasNextPage { get; set; }
        public bool hasPreviousPage { get; set; }

        public PaginatedResultDto(List<T> items, int page, int pageSize, int totalCount)
        {
            this.items = items;
            this.page = page;
            this.pageSize = pageSize;
            this.totalCount = totalCount;
            this.totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            this.hasNextPage = page < totalPages;
            this.hasPreviousPage = page > 1;
        }
    }
}
