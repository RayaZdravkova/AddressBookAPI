namespace AddressBook.Domain.Models
{
    public class User
    {
        public string Gender { get; set; }
        public Name Name { get; set; }
        public Location Location { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Picture Picture { get; set; }
        public string Nat { get; set; }
    }
}
