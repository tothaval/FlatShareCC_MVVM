/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  IRoomCostsCarrier 
 * 
 *  interface for viewmodels containing instances
 *  of RoomCostsViewModel
 */

using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SharedLivingCostCalculator.Interfaces.Financial
{
    public interface IRoomCostsCarrier
    {

        public double CreditSum { get; set; }


        public double OtherFTISum { get; set; }


        event PropertyChangedEventHandler DataChange;


        public ObservableCollection<IFinancialTransactionItem> Credits { get; set; }


        public ObservableCollection<IFinancialTransactionItem> FinancialTransactionItemViewModels { get; set; }


        public void CalculateCreditSum();


        public void CalculateOtherFTISum();


        FlatViewModel GetFlatViewModel();

    }
}
// EOF