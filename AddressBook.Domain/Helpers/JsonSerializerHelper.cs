using AddressBook.Domain.Abstractions.Helpers;
using System.Text.Json;

namespace AddressBook.Domain.Helpers
{
    public class JsonSerializerHelper : IJsonSerializerHelper
    {
        public async Task<T?> DeserializeHttpContentAsync<T>(HttpContent httpContent)
        {
            string json = await httpContent.ReadAsStringAsync();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(json, jsonOptions);
        }
    }
}
