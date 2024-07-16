/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  FinancialTransactionItemViewModel  : BaseViewModel
 * 
 *  data model class
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels.Financial.ViewLess
{
    public class FinancialTransactionItemViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        public double Cost
        {
            get { return _FTI.TransactionSum; }
            set
            {
                _FTI.TransactionSum = value;
                OnPropertyChanged(nameof(Cost));

                ValueChange?.Invoke(this, EventArgs.Empty);
            }
        }


        public TransactionShareTypes CostShareTypes
        {
            get { return _FTI.TransactionShareTypes; }
            set
            {
                _FTI.TransactionShareTypes = value;
                OnPropertyChanged(nameof(CostShareTypes));

                ValueChange?.Invoke(this, EventArgs.Empty);
            }
        }

        public string Item
        {
            get { return _FTI.TransactionItem; }
            set
            {
                _FTI.TransactionItem = value;
                OnPropertyChanged(nameof(Item));

                ValueChange?.Invoke(this, EventArgs.Empty);
            }
        }


        private readonly FinancialTransactionItem _FTI;
        public FinancialTransactionItem FTI => _FTI;

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler? ValueChange;

        #endregion event properties & fields


        // constructors
        #region constructors

        public FinancialTransactionItemViewModel(FinancialTransactionItem fti) //, RoomViewModel roomViewModel)
        {
            _FTI = fti;
        }

        #endregion constructors


    }
}
// EOF