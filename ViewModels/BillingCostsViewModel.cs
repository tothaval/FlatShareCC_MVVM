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
using System.ComponentModel;
using System.Windows.Data;


namespace SharedLivingCostCalculator.ViewModels
{
    internal class BillingCostsViewModel : BaseViewModel
    {

        private readonly BillingViewModel _billingViewModel;


        private readonly FlatViewModel _flatViewModel;


        public double TotalExtraCosts => _billingViewModel.TotalCostsPerPeriod;


        public double TotalCosts => TotalRentCosts + TotalExtraCosts;


        // rent costs must be subtracted based on most recent rent before billing
        public double TotalPayments => _billingViewModel.CalculatePaymentsPerPeriod();


        public double Balance => TotalPayments - TotalCosts;


        public ICollectionView RoomCosts { get; set; }


        public ICollectionView HeatingUnits { get; set; }


        public double TotalRentCosts
        {
            get { return -1.0; }
            //get {
            //    if (_billingViewModel.RentViewModel == null)
            //    {
            //        return -1.0;
            //    }

            //    // DetermineAnnualRent via calculation and date checks of rent updates in flatviewmodel
            //    return _billingViewModel.RentViewModel.AnnualRent;
            //}
        }


        public BillingCostsViewModel(BillingViewModel billingViewModel, FlatViewModel flatViewModel)
        {
                    _billingViewModel = billingViewModel;
                    _flatViewModel = flatViewModel;

            RoomCosts = CollectionViewSource.GetDefaultView(_billingViewModel.RoomCosts);
            RoomCosts.SortDescriptions.Add(new SortDescription("Room.ID", ListSortDirection.Ascending));

            HeatingUnits = CollectionViewSource.GetDefaultView(_billingViewModel.RoomCosts);            
        }


    }
}
// EOF