using System.Collections.Generic;

namespace AdvertisingAgency.Models
{
    public class Advedirsments
    {
        public Advedirsments() { }

        public Advedirsments(string AdType, string AdDescription)
        {
            this.AdType = AdType;
            this.AdDescription = AdDescription;
        }

        public int AdvedirsmentsId { get; set; }
        public string AdType { get; set; }
        public string AdDescription { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
