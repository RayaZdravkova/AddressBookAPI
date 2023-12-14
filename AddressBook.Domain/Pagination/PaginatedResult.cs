namespace AddressBook.Domain.Pagination
{
    public record PaginatedResult<T>
    {
        public List<T> Content { get; set; }
        public int TotalPages { get; set; }
        public int Size { get; set; }
        public int Number { get; set; }
        public bool Empty { get; set; }
        public static PaginatedResult<T> EmptyResult(int totalElements, int pageNumber) => new(new PaginatedResult<T>(new List<T>(), totalElements, pageNumber, 0));

        public PaginatedResult(List<T> content, int totalElements, int pageNumber, int pageSize)
        {
            Content = content;
            Size = pageSize;
            Number = pageNumber;
            TotalPages = content.Count > 0 ? (int)Math.Ceiling(totalElements / (double)pageSize) : 0;
            Empty = !content.Any();
        }
    }
}
