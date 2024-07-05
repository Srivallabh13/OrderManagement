using OrderManagement.DomainLayer.Entities;

namespace OrderManagement.DomainLayer.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public List<OrderProductDTO> Products { get; set; }
        public string CustId { get; set; }
        public double Price { get; set; }
    }
}
