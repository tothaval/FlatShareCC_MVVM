/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ConsumptionItem 
 * 
 *  serializable data model class
 */
using SharedLivingCostCalculator.Interfaces.Contract;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Models.Contract
{

    [Serializable]
    public class ConsumptionItem : IConsumptionItem
    {

        public FinancialTransactionItemBilling ConsumptionCause { get; set; }


        public double ConsumedUnits { get; set; }


        // storing the consumption of units for each room Cause and per billing period
        [XmlArray("ConsumptionPerRoom")]
        public ObservableCollection<RoomConsumption> RoomConsumptions { get; set; } = new ObservableCollection<RoomConsumption>();


        public ConsumptionItem()
        {
                
        }


        public ConsumptionItem(FinancialTransactionItemBilling cause, double consumedUnits = 0.0)
        {
            ConsumptionCause = cause;
            ConsumedUnits = consumedUnits;

            //RoomConsumptions = new ObservableCollection<RoomConsumption>();
        }


        public void UpdateRoomConsumptionItems(ObservableCollection<RoomConsumptionViewModel> roomConsumptionViewModels)
        {
            RoomConsumptions.Clear();

            foreach (RoomConsumptionViewModel item in roomConsumptionViewModels)
            {
                RoomConsumptions.Add(item.RoomConsumption);
            }
        }
    }
}
// EOF