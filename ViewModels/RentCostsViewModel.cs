/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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
using System.ComponentModel;
using System.Windows.Data;
using SharedLivingCostCalculator.ViewModels.ViewLess;


namespace SharedLivingCostCalculator.ViewModels
{
    internal class RentCostsViewModel : BaseViewModel
    {

        private readonly RentViewModel _rentViewModel;


        private readonly FlatViewModel _flatViewModel;


        public bool HasOtherCosts => _rentViewModel.HasOtherCosts;


        public double TotalOtherCosts => _rentViewModel.TotalOtherCosts;


        public double TotalRentCosts => _rentViewModel.ColdRent;


        public double TotalExtraCosts => _rentViewModel.ExtraCostsTotal;


        public double TotalCosts => _rentViewModel.CostsTotal;


        public double CompleteCosts => TotalCosts + TotalOtherCosts;


        public double AnnualRentCosts => _rentViewModel.AnnualRent;


        public double AnnualExtraCosts => _rentViewModel.AnnualExtraCosts;


        public double AnnualOtherCosts => _rentViewModel.AnnualOtherCosts;


        public double AnnualCosts => _rentViewModel.AnnualCostsTotal;


        public double AnnualCompleteCosts => AnnualCosts + AnnualOtherCosts;


        public ICollectionView RoomCosts { get; set; }
        public ICollectionView OtherCosts { get; set; }


        public RentCostsViewModel(RentViewModel rentViewModel, FlatViewModel flatViewModel)
        {
            _rentViewModel = rentViewModel;
            _flatViewModel = flatViewModel;

            RoomCosts = CollectionViewSource.GetDefaultView(_rentViewModel.RoomCosts);
            RoomCosts.SortDescriptions.Add(new SortDescription("Room.ID", ListSortDirection.Ascending));

            OtherCosts = CollectionViewSource.GetDefaultView(_rentViewModel.OtherCosts);
            OtherCosts.SortDescriptions.Add(new SortDescription("Cost", ListSortDirection.Descending));
        }


    }
}
// EOF