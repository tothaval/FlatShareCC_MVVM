/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BillingCostsViewModel : BaseViewModel
 * 
 *  viewmodel for BillingCostsView
 *  
 *  displays the calculated results of 
 *  billing related data for the selected
 *  instance of FlatViewModel and the
 *  selected BillingViewModel instance
 */
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;


namespace SharedLivingCostCalculator.ViewModels.Financial
{
    internal class BillingCostsViewModel : BaseViewModel, ICostDisplay
    {

        // properties & fields
        #region properties

        public BillingViewModel BillingViewModel { get; }


        private readonly FlatViewModel _flatViewModel;


        public double OtherCostItemCountBasedWidth => BillingViewModel.FinancialTransactionItemViewModels.Count * 125;


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
                    ShowOtherCostsData = false;
                    ShowRoomCostShares = false;
                    ShowRoomData = false;
                }
                OnPropertyChanged(nameof(ShowOtherCosts));
            }
        }


        private bool _ShowOtherCostsData;
        public bool ShowOtherCostsData
        {
            get { return _ShowOtherCostsData; }
            set
            {
                _ShowOtherCostsData = value;

                if (_ShowOtherCostsData)
                {
                    ShowOtherCosts = false;
                    ShowRoomCostShares = false;
                    ShowRoomData = false;
                }
                OnPropertyChanged(nameof(ShowOtherCostsData));
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


        private bool _ShowRoomCostShares;
        public bool ShowRoomCostShares
        {
            get { return _ShowRoomCostShares; }
            set
            {
                _ShowRoomCostShares = value;

                if (_ShowRoomCostShares)
                {
                    ShowOtherCosts = false;
                    ShowOtherCostsData = false;
                    ShowRoomData = false;
                }
                OnPropertyChanged(nameof(ShowRoomCostShares));
            }
        }


        private bool _ShowRoomData;
        public bool ShowRoomData
        {
            get { return _ShowRoomData; }
            set
            {
                _ShowRoomData = value;

                if (_ShowRoomData)
                {
                    ShowOtherCosts = false;
                    ShowOtherCostsData = false;
                    ShowRoomCostShares = false;
                }
                OnPropertyChanged(nameof(ShowRoomData));
            }
        }


        public string Signature
        {
            get => $"{BillingViewModel.StartDate:d}\n" +
                   $"{BillingViewModel.EndDate:d}\n" +
                   $"{BillingViewModel.TotalCostsPerPeriod:C2}";
        }

        #endregion properties


        // constructors
        #region constructors

        public BillingCostsViewModel(BillingViewModel billingViewModel, FlatViewModel flatViewModel)
        {
            BillingViewModel = billingViewModel;
            _flatViewModel = flatViewModel;

        }

        #endregion constructors


        // methods
        #region methods

        public FlatViewModel GetFlatViewModel()
        {
            return _flatViewModel;
        }


        public IRoomCostsCarrier GetRoomCostsCarrier()
        {
            return BillingViewModel;
        }

        #endregion methods


    }
}
// EOF