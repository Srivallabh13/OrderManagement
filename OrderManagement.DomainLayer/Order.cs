using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.DomainLayer
{
    public class Order
    {
        public int Id { get; set; }
        public int CustId { get; set; }
        public List<string> Products { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public double Price {  get; set; }
        [ForeignKey("CustId")]
        public User User { get; set; }
    }
}
