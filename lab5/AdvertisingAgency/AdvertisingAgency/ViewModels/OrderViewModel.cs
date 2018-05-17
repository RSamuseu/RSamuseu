using AdvertisingAgency.Models;
using System.Collections.Generic;

namespace AdvertisingAgency.ViewModels
{
    public class OrderViewModel
    {
        public Orders orders { get; set; }
        public IEnumerable<Orders> Orders { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
