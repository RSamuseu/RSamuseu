using System;
using System.Collections.Generic;
using System.Text;

namespace advertisingAgency
{
    public class Advedirsments
    {
        public int AdvedirsmentsId { get; set; }
        public string AdType { get; set; }
        public string AdDescription { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }

        public override string ToString()
        {
            return "Тип: " + AdType + " Описание: " + AdDescription;
        }
    }
}
