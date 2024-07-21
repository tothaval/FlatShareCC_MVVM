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
        public BillingViewModel BillingViewModel { get; }


        private bool _DataLockCheckbox;
        public bool DataLockCheckbox
        {
            get { return _DataLockCheckbox; }
            set
            {

                if (BillingViewModel != null)
                {
                    _DataLockCheckbox = value;
                    BillingViewModel.CostsHasDataLock = value;
                }

                OnPropertyChanged(nameof(DataLockCheckbox));
                OnPropertyChanged(nameof(DataLock));
            }
        }


        public bool DataLock => !DataLockCheckbox;


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



        // commands
        #region commands

        public ICommand AddFinacialTransactionItemCommand { get; }


        public ICommand CloseCommand { get; }


        public ICommand LeftPressCommand { get; }


        public ICommand RemoveFinancialTransactionItemCommand { get; }

        #endregion commands


        public CreditSetupViewModel(BillingViewModel billingViewModel)
        {
            AddFinacialTransactionItemCommand = new RelayCommand((s) => AddFinacialTransactionItem(s), (s) => true);
            RemoveFinancialTransactionItemCommand = new RelayCommand((s) => RemoveFinancialTransactionItem(s), (s) => true);

            BillingViewModel = billingViewModel; 

            _FlatViewModel = billingViewModel.GetFlatViewModel();

            ViewModel = BillingViewModel;


            if (BillingViewModel.HasDataLock)
            {
                DataLockCheckbox = true;
            }

        }


        // Methods
        #region Methods
        private void AddFinacialTransactionItem(object s)
        {
            FinancialTransactionItemBillingViewModel otherCostItemViewModel = new FinancialTransactionItemBillingViewModel(new FinancialTransactionItemBilling());

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
                    var selected = selection.Cast<FinancialTransactionItemBillingViewModel>().ToArray();

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
                
        #endregion events


    }
}
