/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Rent 
 * 
 *  serializable data model class
 *  for RentViewModel
 */
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
// EOF