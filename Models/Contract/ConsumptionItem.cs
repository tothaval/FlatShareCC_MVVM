using SharedLivingCostCalculator.Interfaces.Contract;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Models.Contract
{

    [Serializable]
    public class ConsumptionItem : IConsumptionItem
    {
        public FinancialTransactionItem ConsumptionCause { get; set; }

        public double ConsumedUnits { get; set; }

        // storing the consumption of units for each room Cause and per billing period
        [XmlArray("HeatingUnits")]
        public ObservableCollection<RoomConsumption> RoomConsumptionViewModels { get; set; } = new ObservableCollection<RoomConsumption>();



        public ConsumptionItem()
        {
                
        }


        public ConsumptionItem(FinancialTransactionItem cause, double consumedUnits = 0.0)
        {
            ConsumptionCause = cause;
            ConsumedUnits = consumedUnits;                
        }


    }
}