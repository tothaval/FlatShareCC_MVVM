/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  CreditSetupViewModel  : BaseViewModel
 * 
 *  viewmodel for CreditSetupView, currently in use for AnnualBillingView,
 *  probably gonna be replaced soon by a unifying method for credit setup,
 *  it is allways the same functionality, there are too many duplications i think.
 *  
 *  i have to set the interfaced viewmodel and use that
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.Windows.Input;
using System.Windows;
using SharedLivingCostCalculator.Interfaces.Financial;

namespace SharedLivingCostCalculator.ViewModels.Financial
{
    public class CreditSetupViewModel : BaseViewModel
    {
        
        // Properties & Fields
        #region Properties & Fields

        public BillingViewModel BillingViewModel { get; }


        public bool DataLock
        {
            get { return !BillingViewModel.HasDataLock; }
        }


        private readonly FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;


        private double _SumPerMonth;
        public double OtherFTISum
        {
            get { return _SumPerMonth; }
            set
            {
                _SumPerMonth = value;
                OnPropertyChanged(nameof(OtherFTISum));
            }
        }


        public IRoomCostsCarrier ViewModel { get; }

        #endregion


        // commands
        #region commands

        public ICommand AddFinacialTransactionItemCommand { get; }


        public ICommand RemoveFinancialTransactionItemCommand { get; }

        #endregion commands


        // Constructors
        #region Constructors

        public CreditSetupViewModel(BillingViewModel billingViewModel)
        {
            AddFinacialTransactionItemCommand = new RelayCommand((s) => AddFinacialTransactionItem(s), (s) => true);
            RemoveFinancialTransactionItemCommand = new RelayCommand((s) => RemoveFinancialTransactionItem(s), (s) => true);

            BillingViewModel = billingViewModel;

            _FlatViewModel = billingViewModel.GetFlatViewModel();

            ViewModel = BillingViewModel;

            billingViewModel.PropertyChanged += BillingViewModel_PropertyChanged; ;

            OnPropertyChanged(nameof(DataLock));

        } 

        #endregion


        // Methods
        #region Methods
        private void AddFinacialTransactionItem(object s)
        {
            FinancialTransactionItemRentViewModel otherCostItemViewModel = new FinancialTransactionItemRentViewModel(new FinancialTransactionItemRent());

            BillingViewModel.AddCredit(otherCostItemViewModel);

            OnPropertyChanged(nameof(BillingViewModel));
        }


        private void RemoveFinancialTransactionItem(object s)
        {
            IList selection = (IList)s;

            if (selection != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Do you want to delete selected other costs?",
                    "Remove Other TransactionSum(s)", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var selected = selection.Cast<FinancialTransactionItemRentViewModel>().ToArray();

                    foreach (var item in selected)
                    {
                        BillingViewModel.RemoveCredit(item);

                        //OnPropertyChanged(nameof(CostItems));
                        OnPropertyChanged(nameof(BillingViewModel));
                    }
                }
            }
        }

        #endregion methods


        // events
        #region events

        private void BillingViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(DataLock));
        }

        #endregion events


    }
}
// EOF