/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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

        // Properties & Fields
        #region Properties & Fields

        public double CreditSum { get; set; }


        public double OtherFTISum { get; set; }

        #endregion


        // Event Properties & Fields
        #region Event Properties & Fields

        event PropertyChangedEventHandler DataChange;

        #endregion


        // Collections
        #region Collections

        public ObservableCollection<IFinancialTransactionItem> Credits { get; set; }


        public ObservableCollection<IFinancialTransactionItem> FinancialTransactionItemViewModels { get; set; }

        #endregion


        // Methods
        #region Methods

        public void CalculateCreditSum();


        public void CalculateOtherFTISum();


        FlatViewModel GetFlatViewModel(); 
        
        #endregion


    }
}
// EOF