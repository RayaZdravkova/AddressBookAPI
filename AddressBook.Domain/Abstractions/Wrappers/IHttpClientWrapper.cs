namespace AddressBook.Domain.Abstractions.Wrappers
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(string? requestUri);
    }
}
