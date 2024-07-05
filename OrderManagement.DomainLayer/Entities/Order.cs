using OrderManagement.DomainLayer.DTO;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.DomainLayer.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string CustId { get; set; }
        public List<OrderProduct> Products { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
        [ForeignKey("CustId")]
        public User User { get; set; }
    }
}
