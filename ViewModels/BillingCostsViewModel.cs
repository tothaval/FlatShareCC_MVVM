/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using SharedLivingCostCalculator.ViewModels.ViewLess;


namespace SharedLivingCostCalculator.ViewModels
{
    internal class BillingCostsViewModel : BaseViewModel
    {

        // properties & fields
        #region properties

        public double Area => _billingViewModel.GetFlatViewModel().Area;


        public double Balance => DetermineBalance();


        public ICollectionView BillingListView { get; set; }


        private readonly BillingViewModel _billingViewModel;


        private readonly FlatViewModel _flatViewModel;


        public bool HasCredits => _billingViewModel.HasCredits;


        public bool HasPayments => _billingViewModel.HasPayments;


        public ICollectionView HeatingUnits { get; set; }


        public ICollectionView RoomCosts { get; set; }


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


        public double TotalAdvancePerPeriod => 1500.24;
        //public double TotalAdvancePerPeriod => DetermineTotalAdvancePerPeriod();


        public double TotalCosts => TotalRentCosts + TotalExtraCosts;


        public double TotalExtraCosts => _billingViewModel.TotalCostsPerPeriod;


        public double TotalFixedCosts => _billingViewModel.TotalFixedCostsPerPeriod;


        public double TotalHeatingCosts => _billingViewModel.TotalHeatingCostsPerPeriod;


        // rent costs must be subtracted based on most recent rent before billing
        public double TotalPayments => _billingViewModel.CalculatePaymentsPerPeriod();


        public double TotalRentCosts
        {
            get { return -1.0; }
            //get {
            //    if (_billingViewModel.RentViewModel == null)
            //    {
            //        return -1.0;
            //    }

            //    
            // DetermineAnnualRent via calculation and date checks of rent updates in flatviewmodel
            //
            //
            //    return _billingViewModel.RentViewModel.AnnualRent;

            //}
        }
        #endregion properties


        // collections
        #region collections

        public ObservableCollection<BillingCostsViewModel> Billing { get; }

        #endregion collections


        // constructors
        #region constructors
        public BillingCostsViewModel(BillingViewModel billingViewModel, FlatViewModel flatViewModel)
        {
            _billingViewModel = billingViewModel;
            _flatViewModel = flatViewModel;

            Billing = new ObservableCollection<BillingCostsViewModel>() {  this };

            RoomCosts = CollectionViewSource.GetDefaultView(_billingViewModel.RoomCosts);
            RoomCosts.SortDescriptions.Add(new SortDescription("Room.ID", ListSortDirection.Ascending));

            HeatingUnits = CollectionViewSource.GetDefaultView(_billingViewModel.RoomCosts);

            BillingListView = CollectionViewSource.GetDefaultView(Billing);

            _billingViewModel.BillingViewModelConfigurationChange += _billingViewModel_BillingViewModelConfigurationChange;
        }
        #endregion constructors


        // methods
        #region methods
        private double DetermineBalance()
        {
            double balance = 0.0;

            if (HasPayments)
            {
                return TotalPayments - TotalCosts;
            }

            return TotalAdvancePerPeriod - TotalExtraCosts;
        }

        private double DetermineTotalAdvancePerPeriod()
        {
            double advance = 0.0;

            // use these items:
            // _billingViewModel.FindRelevantRentViewModels()
            // CalculateRentCosts() (RoomCostsViewModel <= change ColdRent to AdvanceType

            //advance += months * rentViewModel.ExtraCostsTotal;

            ObservableCollection<RentViewModel> rentViewModels = new ObservableCollection<RentViewModel>();


            return advance;
        }
        #endregion methods


        // events
        #region events
        private void _billingViewModel_BillingViewModelConfigurationChange(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(HasCredits));
            OnPropertyChanged(nameof(HasPayments));
        }
        #endregion events


    }
}
// EOF