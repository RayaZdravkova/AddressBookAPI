namespace AddressBook.Domain.Abstractions.Helpers
{
    public interface IJsonSerializerHelper
    {
        Task<T?> DeserializeHttpContentAsync<T>(HttpContent httpContent);
    }
}
