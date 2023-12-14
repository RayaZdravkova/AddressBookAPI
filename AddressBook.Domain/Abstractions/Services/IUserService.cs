using AddressBook.Domain.Models;
using AddressBook.Domain.Pagination;
using AddressBook.Domain.Parameters;

namespace AddressBook.Domain.Abstractions.Services
{
    public interface IUserService
    {
        Task<PaginatedResult<User>> GetPaginatedUsersAsync(PagingInfo pagingInfo, QueryParameters parameters);
    }
}
