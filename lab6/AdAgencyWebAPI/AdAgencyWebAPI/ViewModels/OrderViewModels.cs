using System;

namespace AdAgencyWebAPI.ViewModels
{
    public class OrderViewModels
    {
        public int OrdersId { get; set; }
        public DateTime DateOrder { get; set; }
        public string Location { get; set; }
        public int OrderCost { get; set; }
    }
}
