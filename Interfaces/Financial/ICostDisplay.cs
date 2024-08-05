/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ICostDisplay 
 * 
 *  interface for viewmodels that deal with cost display 
 */
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using System.ComponentModel;

namespace SharedLivingCostCalculator.Interfaces.Financial
{
    public interface ICostDisplay
    {
        public string Signature { get; }

        FlatViewModel GetFlatViewModel();

        IRoomCostsCarrier GetRoomCostsCarrier();


    }
}
// EOF