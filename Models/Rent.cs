using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SharedLivingCostCalculator.Models
{
    public class Rent
    {
        public DateTime StartDate { get; set; } = DateTime.Now;

        public double ColdRent { get; set; } = 0.0;

        public double AnnualRent => ColdRent * 12;

        public double ExtraCostsShared { get; set; } = 0.0;

        public double ExtraCostsHeating { get; set; } = 0.0;

        public double ExtraCostsTotal => ExtraCostsShared + ExtraCostsHeating;
        public double CostsTotal => ColdRent + ExtraCostsTotal;
        public double AnnualExtraCosts => ExtraCostsTotal * 12;

        public Rent()
        {
                
        }

        public Rent(DateTime startDate, double coldRent, double extraCostsShared, double extraCostsHeating)
        {
            StartDate = startDate;
            ColdRent = coldRent;
            ExtraCostsShared = extraCostsShared;
            ExtraCostsHeating = extraCostsHeating;
        }
    }
}
