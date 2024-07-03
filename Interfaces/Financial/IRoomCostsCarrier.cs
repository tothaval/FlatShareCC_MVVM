/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  IRoomCostsCarrier 
 * 
 *  interface for viewmodels containing instances
 *  of RoomCostsViewModel
 */

using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using System.ComponentModel;

namespace SharedLivingCostCalculator.Interfaces.Financial
{
    public interface IRoomCostsCarrier
    {
        void GenerateRoomCosts();


        FlatViewModel GetFlatViewModel();


        event PropertyChangedEventHandler DataChange;

    }
}
// EOF