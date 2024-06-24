using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.DomainLayer.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(40)]
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int PinCode { get; set; }
        public bool IsClient { get; set; } = false;

        public List<Order> Orders { get; set; }

    }
}
