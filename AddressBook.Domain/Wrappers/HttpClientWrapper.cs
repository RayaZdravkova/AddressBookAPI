using AddressBook.Domain.Abstractions.Wrappers;

namespace AddressBook.Domain.Wrappers
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper(IHttpClientFactory httpClientFactory) 
        {
            _httpClient = httpClientFactory.CreateClient("Client");
        }

        public async Task<HttpResponseMessage> GetAsync(string? requestUri)
        {
            return await _httpClient.GetAsync(requestUri);
        }
    }
}
