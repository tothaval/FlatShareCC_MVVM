﻿/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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

        // properties & fields
        #region properties & fields

        private readonly FinancialTransactionItemRent _FTI;
        public FinancialTransactionItemRent FTI => _FTI;


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


        public int Rates
        {
            get { return FTI.Rates; }
            set
            {
                FTI.Rates = value;
                OnPropertyChanged(nameof(Rates));
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


        public TransactionShareTypesRent CostShareTypes
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

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler? ValueChange;

        #endregion event properties & fields


        // constructors
        #region constructors

        public FinancialTransactionItemRentViewModel(FinancialTransactionItemRent fti) //, RoomViewModel roomViewModel)
        {
            _FTI = fti;
        }

        #endregion constructors


    }
}
// EOF