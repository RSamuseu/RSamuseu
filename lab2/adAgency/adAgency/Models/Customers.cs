using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace adAgency.Models
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

        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerTelephone { get; set; }
    }
}
