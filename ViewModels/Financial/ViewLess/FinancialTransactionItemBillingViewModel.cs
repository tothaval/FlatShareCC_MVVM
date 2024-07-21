/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  FinancialTransactionItemBillingViewModel  : BaseViewModel
 * 
 *  view model for FinancialTransactionItemBilling data model class
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels.Financial.ViewLess
{
    public class FinancialTransactionItemBillingViewModel : BaseViewModel, IFinancialTransactionItem
    {

        // properties & fields
        #region properties & fields

        public double TransactionSum
        {
            get { return _FTI.TransactionSum; }
            set
            {
                _FTI.TransactionSum = value;
                OnPropertyChanged(nameof(TransactionSum));

                ValueChange?.Invoke(this, EventArgs.Empty);
            }
        }


        public TransactionShareTypesBilling CostShareTypes
        {
            get { return _FTI.TransactionShareTypes; }
            set
            {
                _FTI.TransactionShareTypes = value;
                OnPropertyChanged(nameof(CostShareTypes));

                ValueChange?.Invoke(this, EventArgs.Empty);
            }
        }

        public string TransactionItem
        {
            get { return _FTI.TransactionItem; }
            set
            {
                _FTI.TransactionItem = value;
                OnPropertyChanged(nameof(TransactionItem));

                ValueChange?.Invoke(this, EventArgs.Empty);
            }
        }


        private readonly FinancialTransactionItemBilling _FTI;
        public FinancialTransactionItemBilling FTI => _FTI;

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler? ValueChange;

        #endregion event properties & fields


        // constructors
        #region constructors

        public FinancialTransactionItemBillingViewModel(FinancialTransactionItemBilling fti) //, RoomViewModel roomViewModel)
        {
            _FTI = fti;
        }

        #endregion constructors


    }
}
// EOF