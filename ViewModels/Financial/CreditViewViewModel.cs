/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  CreditViewViewModel  : BaseViewModel
 * 
 *  viewmodel for CreditView window, currently still in use for RentUpdateView,
 *  probably gonna be replaced soon by a unifying mechanism for credit setup,
 *  it is allways the same functionality, there are too many duplications i think.
 *  
 *  i have to set the interfaced viewmodel and use that
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.Windows;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels.Financial
{
    public class CreditViewViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

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


        public RentViewModel RentViewModel { get; }


        public IRoomCostsCarrier ViewModel { get; }

        #endregion


        // commands
        #region commands

        public ICommand AddFinacialTransactionItemCommand { get; }


        public ICommand RemoveFinancialTransactionItemCommand { get; }

        #endregion commands


        // Constructors
        #region Constructors
        public CreditViewViewModel(RentViewModel rentViewModel)
        {
            AddFinacialTransactionItemCommand = new RelayCommand((s) => AddFinacialTransactionItem(s), (s) => true);
            RemoveFinancialTransactionItemCommand = new RelayCommand((s) => RemoveFinancialTransactionItem(s), (s) => true);

            RentViewModel = rentViewModel;
            
            if (RentViewModel != null)
            {
                _FlatViewModel = rentViewModel.GetFlatViewModel();

                ViewModel = RentViewModel;
            }

        }

        #endregion


        // Methods
        #region Methods
        private void AddFinacialTransactionItem(object s)
        {
            FinancialTransactionItemRentViewModel otherCostItemViewModel = new FinancialTransactionItemRentViewModel(new FinancialTransactionItemRent());

            //otherCostItemViewModel.ValueChange += Item_ValueChange;

            RentViewModel.AddCredit(otherCostItemViewModel);

            //OnPropertyChanged(nameof(CostItems));
            OnPropertyChanged(nameof(RentViewModel));
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
                        RentViewModel.RemoveCredit(item);

                        OnPropertyChanged(nameof(RentViewModel));
                    }
                }
            }
        }

        #endregion methods


    }
}
// EOF