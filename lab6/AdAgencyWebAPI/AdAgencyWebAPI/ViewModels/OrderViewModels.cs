using System;

namespace AdAgencyWebAPI.ViewModels
{
    public class OrderViewModels
    {
        public int OrdersId         { get; set; }
        public DateTime DateOrder   { get; set; }
        public DateTime DateBegin   { get; set; }
        public DateTime DateEnd     { get; set; }
        public string Location      { get; set; }
        public int OrderCost        { get; set; }
        public int? AdvedirsmentsId { get; set; }
        public string AdType { get; set; }
        public int? CustomersId { get; set; }
        public string CustomerName { get; set; }
    }
}
