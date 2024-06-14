namespace OrderManagement.DomainLayer
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int PinCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsClient { get; set; }

        public List<Order> Orders { get; set; }

    }
}
