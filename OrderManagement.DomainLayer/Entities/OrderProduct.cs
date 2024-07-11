using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.DomainLayer.Entities
{
    public class OrderProduct
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("Id")]
        public Order Order { get; set; }
    }
}
