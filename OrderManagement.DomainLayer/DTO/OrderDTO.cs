namespace OrderManagement.DomainLayer.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public List<string> Products { get; set; }
        public string CustId { get; set; }
        public double Price { get; set; }


    }
}
