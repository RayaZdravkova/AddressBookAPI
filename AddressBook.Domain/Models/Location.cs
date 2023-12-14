namespace AddressBook.Domain.Models
{
    public class Location
    {
        public Street Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public object Postcode { get; set; }
    }
}
