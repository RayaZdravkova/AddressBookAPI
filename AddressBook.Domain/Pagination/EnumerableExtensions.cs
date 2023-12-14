using System.ComponentModel.DataAnnotations;

namespace AddressBook.Domain.Pagination
{
    public static class EnumerableExtensions
    {
        public static PaginatedResult<T> Paginate<T>(this IEnumerable<T> collection, int pageNumber, int pageSize)
        {
            if (pageNumber < 0 || pageSize < 0)
            {
                throw new ValidationException("Page and size should not be negative values!");
            }

            var totalElements = collection.Count();
            var skip = pageNumber * pageSize;

            if (totalElements == 0 || totalElements < skip)
            {
                return PaginatedResult<T>.EmptyResult(totalElements, pageNumber);
            }

            var result = collection.Skip(skip).Take(pageSize).ToList();

            return new PaginatedResult<T>(result, totalElements, pageNumber, pageSize);
        }
    }
}
