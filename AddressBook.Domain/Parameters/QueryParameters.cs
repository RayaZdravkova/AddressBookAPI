using AddressBook.Domain.Enums;

namespace AddressBook.Domain.Parameters
{
    public class QueryParameters
    {
        public GenderEnum? Gender { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }
}
