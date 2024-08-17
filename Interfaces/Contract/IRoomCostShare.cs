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

        // Properties & Fields
        #region Properties & Fields

        public double RentedAreaShare { get; set; }


        public string RoomName { get; }


        public double RoomArea { get; }


        public string Tenant { get; set; }


        public IRoomCostsCarrier ViewModel { get; set; } 

        #endregion


        // Methods
        #region Methods

        public void CalculateValues(); 

        #endregion


    }
}
// EOF