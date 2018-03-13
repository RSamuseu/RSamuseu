using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace adAgency.Models
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

        public DateTime DateOrder { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public string Location { get; set; }
        public int OrderCost { get; set; }
    }
}
