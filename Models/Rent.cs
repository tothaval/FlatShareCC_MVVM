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
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Models
{
    [Serializable]
    public class Rent
    {
        public int ID {  get; set; } = 0;

        public int BillingID { get; set; } = -1;

        public DateTime StartDate { get; set; } = DateTime.Now;

        public double ColdRent { get; set; } = 0.0;

        [XmlIgnore]
        public double AnnualRent => ColdRent * 12;

        public double ExtraCostsShared { get; set; } = 0.0;

        public double ExtraCostsHeating { get; set; } = 0.0;

        public double ExtraCostsTotal => ExtraCostsShared + ExtraCostsHeating;

        public double CostsTotal => ColdRent + ExtraCostsTotal;

        [XmlIgnore]
        public double AnnualExtraCosts => ExtraCostsTotal * 12;

        public Rent()
        {

        }

        public Rent(
            int id,
            DateTime startDate,
            double coldRent,
            double extraCostsShared,
            double extraCostsHeating)
        {
            ID = id;
            StartDate = startDate;
            ColdRent = coldRent;
            ExtraCostsShared = extraCostsShared;
            ExtraCostsHeating = extraCostsHeating;
        }
    }
}
