/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Payment 
 * 
 *  serializable data model class
 *  for PaymentViewModel
 */
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Models
{

    [Serializable]
    public class Payment
    {

        public DateTime StartDate { get; set; } = new DateTime();
                

        public DateTime EndDate { get; set; } = new DateTime();


        public int PaymentQuantity { get; set; } = 1;


        public double Sum { get; set; } = 0.0;


        [XmlIgnore]
        public double PaymentTotal => Sum * PaymentQuantity;


        [XmlIgnore]
        public bool EndDateVisible => PaymentQuantity > 1;


        public Payment()
        {              
        }


    }
}
// EOF