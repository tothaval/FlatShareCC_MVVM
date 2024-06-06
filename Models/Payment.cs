using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
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
