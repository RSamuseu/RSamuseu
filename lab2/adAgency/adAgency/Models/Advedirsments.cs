using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace adAgency.Models
{
    public class Advedirsments
    {
        public Advedirsments() { }

        public Advedirsments(string AdType, string AdDescription)
        {
            this.AdType = AdType;
            this.AdDescription = AdDescription;
        }

        public string AdType { get; set; }
        public string AdDescription { get; set; }
    }
}
