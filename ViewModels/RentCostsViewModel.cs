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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class RentCostsViewModel : BaseViewModel
    {

        // properties & fields
        #region properties

        public double AnnualCompleteCosts => AnnualCosts + AnnualOtherCosts;


        public double AnnualCosts => _rentViewModel.AnnualCostsTotal;


        public double AnnualExtraCosts => _rentViewModel.AnnualExtraCosts;


        public double AnnualOtherCosts => _rentViewModel.AnnualOtherCosts;


        public double AnnualRentCosts => _rentViewModel.AnnualRent;


        public double CompleteCosts => TotalCosts + TotalOtherCosts;


        private readonly FlatViewModel _flatViewModel;


        public bool HasOtherCosts => _rentViewModel.HasOtherCosts;


        private readonly RentViewModel _rentViewModel;


        public double OtherCostItemCountBasedWidth => _rentViewModel.OtherCosts.Count * 105;


        public ICollectionView OtherCosts { get; set; }


        public ICollectionView RentListView { get; set; }


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


        private bool _ShowOtherCosts;
        public bool ShowOtherCosts
        {
            get { return _ShowOtherCosts; }
            set
            {
                _ShowOtherCosts = value;
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


        public double TotalCosts => _rentViewModel.CostsTotal;


        public double TotalExtraCosts => _rentViewModel.ExtraCostsTotal;


        public double TotalOtherCosts => _rentViewModel.TotalOtherCosts;


        public double TotalRentCosts => _rentViewModel.ColdRent;

        #endregion properties


        // collections
        #region collections

        public ObservableCollection<RentCostsViewModel> Rent { get; }

        #endregion collections


        // constructors
        #region constructors
        public RentCostsViewModel(RentViewModel rentViewModel, FlatViewModel flatViewModel)
        {
            _rentViewModel = rentViewModel;
            _flatViewModel = flatViewModel;

            Rent = new ObservableCollection<RentCostsViewModel>() { this };

            RoomCosts = CollectionViewSource.GetDefaultView(_rentViewModel.RoomCosts);
            RoomCosts.SortDescriptions.Add(new SortDescription("Room.ID", ListSortDirection.Ascending));

            OtherCosts = CollectionViewSource.GetDefaultView(_rentViewModel.OtherCosts);
            OtherCosts.SortDescriptions.Add(new SortDescription("Cost", ListSortDirection.Descending)); 
            
            RentListView = CollectionViewSource.GetDefaultView(Rent);
                        
        }
        #endregion constructors


    }
}
// EOF