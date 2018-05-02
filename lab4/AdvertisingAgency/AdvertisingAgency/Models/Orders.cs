using System;
using System.Collections.Generic;
using System.Text;

namespace AdvertisingAgency.Models
{
    public class Orders
    {
        public Orders() { }

        public Orders(DateTime DateOrder, DateTime DateBegin, DateTime DateEnd, string Location, int OrderCost)
        {
            this.DateOrder = DateOrder;
            this.DateBegin = DateBegin;
            this.DateEnd = DateEnd;
            this.Location = Location;
            this.OrderCost = OrderCost;
        }

        public int OrdersId { get; set; }
        public DateTime DateOrder { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public string Location { get; set; }
        public int OrderCost { get; set; }
        public bool PaymentStatus { get; set; }
        public int? EmployeeId { get; set; }
        public int? CustomersId { get; set; }
        public int AdvedirsmentsId { get; set; }
        public virtual Advedirsments Advedirsments { get; set; }
        public virtual Customers Customers { get; set; }
    }
}
