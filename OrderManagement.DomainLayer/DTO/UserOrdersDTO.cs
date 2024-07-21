using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.DomainLayer.DTO
{
    public class UserOrdersDTO
    {
        public Guid Id { get; set; }
        public string CustId { get; set; }
        public List<ProductDto> Products { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
    }

    public class ProductDto
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

}

