using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    public class Tenant
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Tenant()
        {
            ID = -1;
            Name = string.Empty;
            StartDate = DateTime.Now;
            StartDate = DateTime.Now.AddMonths(1);
        }
    }
}
