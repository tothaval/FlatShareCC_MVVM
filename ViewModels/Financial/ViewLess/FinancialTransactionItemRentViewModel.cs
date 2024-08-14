/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  FinancialTransactionItemRentViewModel  : BaseViewModel
 * 
 *  view model for FinancialTransactionItemRent data model class
 *  
 *  implements IFinancialTransactionItem
 *  
 *  the interface is expanded by a DateTime StartDate, an int Rates and an enum TransactionDurationTypes
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels.Financial.ViewLess
{
    public class FinancialTransactionItemRentViewModel : BaseViewModel, IFinancialTransactionItem
    {

        // Properties & Fields
        #region Properties & Fields

        public TransactionDurationTypes Duration
        {
            get { return FTI.Duration; }
            set
            {
                FTI.Duration = value;
                OnPropertyChanged(nameof(Duration));
                OnPropertyChanged(nameof(HasLimitedDuration));               
            }
        }


        public DateTime EndDate
        {
            get { return FTI.EndDate; }
            set
            {
                FTI.EndDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }


        private readonly FinancialTransactionItemRent _FTI;
        public FinancialTransactionItemRent FTI => _FTI;


        public bool HasLimitedDuration
        {
            get
            {
                if (Duration == TransactionDurationTypes.Ongoing)
                {
                    return false;
                }

                return true;
            }
        }


        public DateTime StartDate
        {
            get { return FTI.StartDate; }
            set
            {
                FTI.StartDate = value;
                OnPropertyChanged(nameof(StartDate));
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


        public TransactionShareTypesRent TransactionShareTypes
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

        public FinancialTransactionItemRentViewModel(FinancialTransactionItemRent fti) //, RoomViewModel roomViewModel)
        {
            _FTI = fti;
        }

        #endregion


    }
}
// EOF