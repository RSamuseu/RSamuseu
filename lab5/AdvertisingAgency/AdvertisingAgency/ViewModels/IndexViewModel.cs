using AdvertisingAgency.Models;
using System.Collections.Generic;

namespace AdvertisingAgency.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Advedirsments> Advedirsments { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
