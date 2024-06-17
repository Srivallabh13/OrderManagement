using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.ApplicationLayer
{
    internal interface IOrderService
    {
        // create order
        //create and add to the DB. -> call AddAsync. -> -> product,stock ->  valid -> avail -> call inventory update -> email service (trigger) 

    }
}
