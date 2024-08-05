/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  IRoomCostShare 
 * 
 *  interface for roomcostshare models
 *  
 *  currently they handle output and calculation of room related data
 */

using SharedLivingCostCalculator.Interfaces.Financial;

namespace SharedLivingCostCalculator.Interfaces.Contract
{
    public interface IRoomCostShare
    {

        public string RoomName { get; }
        public double RoomArea { get; }
        public double SharedAreaShare { get; set; }
        public double RentedAreaShare { get; set; }

        public IRoomCostsCarrier ViewModel { get; set; }

        public void CalculateValues();

    }
}
// EOF