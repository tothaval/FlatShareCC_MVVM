/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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

        // Properties & Fields
        #region Properties & Fields

        private readonly FinancialTransactionItemBilling _FTI;
        public FinancialTransactionItemBilling FTI => _FTI;


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


        public TransactionShareTypesBilling TransactionShareTypes
        {
            get { return _FTI.TransactionShareTypes; }
            set
            {
                _FTI.TransactionShareTypes = value;
                OnPropertyChanged(nameof(TransactionShareTypes));

                ValueChange?.Invoke(this, EventArgs.Empty);
            }
        }


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

        #endregion 


        // Event Properties & Fields
        #region Event Properties & Fields

        public event EventHandler? ValueChange;

        #endregion


        // Constructors
        #region Constructors

        public FinancialTransactionItemBillingViewModel(FinancialTransactionItemBilling fti) //, RoomViewModel roomViewModel)
        {
            _FTI = fti;
        }

        #endregion


    }
}
// EOF