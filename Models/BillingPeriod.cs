using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    internal class BillingPeriod
    {
        public DateTime BillingPeriodStartDate { get; set; }
        public DateTime BillingPeriodEndDate { get; set; }

        public double TotalCostsPerPeriod { get; set; }
        public double TotalFixedCostsPerPeriod { get; set;}
        public double TotalHeatingCostsPerPeriod { get; set; }

        // additional fields for room heating units consumption
        // a separate class for payments on a per room basis
    }
}
