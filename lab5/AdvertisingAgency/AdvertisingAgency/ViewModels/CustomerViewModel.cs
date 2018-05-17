using AdvertisingAgency.Models;
using System.Collections.Generic;

namespace AdvertisingAgency.ViewModels
{
    public class CustomerViewModel
    {
        public IEnumerable<Customers> Customers { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
