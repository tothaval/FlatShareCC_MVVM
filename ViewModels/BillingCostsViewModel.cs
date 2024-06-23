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
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels.ViewLess;


namespace SharedLivingCostCalculator.ViewModels
{
    internal class BillingCostsViewModel : BaseViewModel
    {

        private readonly BillingViewModel _billingViewModel;


        private readonly FlatViewModel _flatViewModel;

        public double Area => _billingViewModel.GetFlatViewModel().Area;

        public double TotalFixedCosts => _billingViewModel.TotalFixedCostsPerPeriod;
        public double TotalHeatingCosts => _billingViewModel.TotalHeatingCostsPerPeriod;
        public double TotalExtraCosts => _billingViewModel.TotalCostsPerPeriod;

        public double TotalAdvancePerPeriod => 1500.24;
        //public double TotalAdvancePerPeriod => DetermineTotalAdvancePerPeriod();

        public double TotalCosts => TotalRentCosts + TotalExtraCosts;


        // rent costs must be subtracted based on most recent rent before billing
        public double TotalPayments => _billingViewModel.CalculatePaymentsPerPeriod();


        public double Balance => DetermineBalance();


        public ObservableCollection<BillingCostsViewModel> Billing { get; }


        public ICollectionView RoomCosts { get; set; }


        public ICollectionView HeatingUnits { get; set; }

        public ICollectionView BillingListView { get; set; }

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


        public bool HasPayments => _billingViewModel.HasPayments;


        public BillingCostsViewModel(BillingViewModel billingViewModel, FlatViewModel flatViewModel)
        {
            _billingViewModel = billingViewModel;
            _flatViewModel = flatViewModel;

            Billing = new ObservableCollection<BillingCostsViewModel>() {  this };

            RoomCosts = CollectionViewSource.GetDefaultView(_billingViewModel.RoomCosts);
            RoomCosts.SortDescriptions.Add(new SortDescription("Room.ID", ListSortDirection.Ascending));

            HeatingUnits = CollectionViewSource.GetDefaultView(_billingViewModel.RoomCosts);

            BillingListView = CollectionViewSource.GetDefaultView(Billing);
        }


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


            // ich muss zunächst die menge n RentViewModels ermitteln, die
            // in die BillingPeriod hineinwirken oder drin sind 
            //TimeSpan timeSpan = _billingViewModel.EndDate - rentViewModel.StartDate;


            // nicht so kompliziert. wir suchen die Miete direkt vor der Billing 
            // mittels des most recent algorithmus, dann suchen wir, ob es eine oder mehrere gibt,
            // die danach liegen aber innerhalb der period beginnen 
            // die Miete direkt vor der Billing wird ab Billingbeginn bis zur nächsten Miete
            // für die Wertberechnung genommen, dann die Werte der darauf folgenden Miete bis
            // entweder zur nächsten Miete oder bis zum Ende der Billingperiod.

            //int months = (int)timeSpan.TotalDays / 30;

            //advance += months * rentViewModel.ExtraCostsTotal;

            ObservableCollection<RentViewModel> rentViewModels = new ObservableCollection<RentViewModel>();


            foreach (RentViewModel rentViewModel in _flatViewModel.RentUpdates)
            {
                if (rentViewModel.StartDate > _billingViewModel.EndDate)
                {
                    continue;
                }

                if (rentViewModel.StartDate < _billingViewModel.StartDate && rentViewModel.StartDate < _billingViewModel.EndDate)
                {
                    foreach (RentViewModel item in rentViewModels)
                    {
                        if (item.StartDate < rentViewModel.StartDate)
                        {
                            rentViewModels.Remove(item);
                            rentViewModels.Add(rentViewModel);                            
                        }
                    }
                }

                if (rentViewModel.StartDate > _billingViewModel.StartDate && rentViewModel.StartDate < _billingViewModel.EndDate)
                {
                    rentViewModels.Add(rentViewModel);
                }                 
            }


            foreach (RentViewModel item in rentViewModels)
            {

            }

            return 0;
        }

    }
}
// EOF