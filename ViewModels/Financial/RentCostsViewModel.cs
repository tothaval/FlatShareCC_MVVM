/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RentCostsViewModel : BaseViewModel
 * 
 *  viewmodel for RentCostsView
 *  
 *  displays the calculated results of 
 *  rent related data for the selected
 *  instance of FlatViewModel and the
 *  selected RentViewModel instance
 */
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels.Financial
{
    internal class RentCostsViewModel : BaseViewModel, ICostDisplay
    {

        // properties & fields
        #region properties

        private readonly FlatViewModel _flatViewModel;


        public RentViewModel RentViewModel { get; }


        public double OtherCostItemCountBasedWidth => RentViewModel.FinancialTransactionItemViewModels.Count * 105;


        private bool _ShowFlatCosts;
        public bool ShowFlatCosts
        {
            get { return _ShowFlatCosts; }
            set
            {
                _ShowFlatCosts = value;
                OnPropertyChanged(nameof(ShowFlatCosts));
            }
        }


        private bool _ShowOtherCosts;
        public bool ShowOtherCosts
        {
            get { return _ShowOtherCosts; }
            set
            {
                _ShowOtherCosts = value;

                if (_ShowOtherCosts)
                {
                    ShowRoomCostsPerMonth = false;
                    ShowRoomCostsPerYear = false;
                }

                OnPropertyChanged(nameof(ShowOtherCosts));
            }
        }

         


        private bool _ShowRoomCosts;
        public bool ShowRoomCosts
        {
            get { return _ShowRoomCosts; }
            set
            {
                _ShowRoomCosts = value;
                OnPropertyChanged(nameof(ShowRoomCosts));
            }
        }


        private bool _ShowRoomCostsPerMonth;
        public bool ShowRoomCostsPerMonth
        {
            get { return _ShowRoomCostsPerMonth; }
            set
            {
                _ShowRoomCostsPerMonth = value;

                if (_ShowRoomCostsPerMonth)
                {
                    ShowOtherCosts = false;
                    ShowRoomCostsPerYear = false;
                }

                OnPropertyChanged(nameof(ShowRoomCostsPerMonth));
            }
        }


        private bool _ShowRoomCostsPerYear;
        public bool ShowRoomCostsPerYear
        {
            get { return _ShowRoomCostsPerYear; }
            set
            {
                _ShowRoomCostsPerYear = value;

                if (_ShowRoomCostsPerYear)
                {
                    ShowOtherCosts = false;
                    ShowRoomCostsPerMonth = false;                    
                }

                OnPropertyChanged(nameof(ShowRoomCostsPerYear));
            }
        }

        public string Signature 
        {
            get => $"{RentViewModel.StartDate:d}\n" +
                $"\n" +
                $"{RentViewModel.CompleteCosts:C2}";
        }

        #endregion properties


        // constructors
        #region constructors
        public RentCostsViewModel(RentViewModel rentViewModel, FlatViewModel flatViewModel)
        {
            RentViewModel = rentViewModel;
            _flatViewModel = flatViewModel;

            ShowRoomCostsPerMonth = true;
        }

        #endregion constructors


        // Methods
        #region Methods

        public FlatViewModel GetFlatViewModel()
        {
            return _flatViewModel;
        }

        public IRoomCostsCarrier GetRoomCostsCarrier()
        {
            return RentViewModel;
        } 

        #endregion


    }
}
// EOF