/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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

        FlatViewModel GetFlatViewModel();

        IRoomCostsCarrier GetRoomCostsCarrier();


    }
}
// EOF