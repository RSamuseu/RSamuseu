using System;
using System.Collections.Generic;
using System.Text;

namespace advertisingAgency
{
    public class Customers
    {
        public int CustomersId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerTelephone { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }

        public override string ToString()
        {
            return "Имя заказчика: " + CustomerName + " адрес: " + CustomerAddress + "телефон: " + CustomerTelephone; 
        }
    }
}
