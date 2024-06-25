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

        // properties & fields
        #region properties                

        public DateTime EndDate { get; set; } = new DateTime();


        [XmlIgnore]
        public bool EndDateVisible => PaymentQuantity > 1;


        public int PaymentQuantity { get; set; } = 1;


        [XmlIgnore]
        public double PaymentTotal => Sum * PaymentQuantity;


        public DateTime StartDate { get; set; } = new DateTime();


        public double Sum { get; set; } = 0.0;

        #endregion properties


        // constructors
        #region constructors

        public Payment()
        {              
        }

        #endregion constructors


    }
}
// EOF