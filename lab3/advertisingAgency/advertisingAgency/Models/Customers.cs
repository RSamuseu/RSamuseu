using System;
using System.Collections.Generic;
using System.Text;

namespace advertisingAgency.Models
{
    public class Customers
    {
        public Customers() { }

        public Customers(string CustomerName, string CustomerAddress, string CustomerTelephone)
        {
            this.CustomerName = CustomerName;
            this.CustomerAddress = CustomerAddress;
            this.CustomerTelephone = CustomerTelephone;
        }

        public int CustomersId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerTelephone { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
