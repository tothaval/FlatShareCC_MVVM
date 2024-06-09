using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class RentCostsViewModel : BaseViewModel
    {
        private readonly RentViewModel _rentViewModel;
        private readonly FlatViewModel _flatViewModel;

        public double TotalRentCosts => _rentViewModel.ColdRent;

        public double TotalExtraCosts => _rentViewModel.ExtraCostsTotal;

        public double TotalCosts => _rentViewModel.CostsTotal;

        public double AnnualRentCosts => _rentViewModel.AnnualRent;

        public double AnnualExtraCosts => _rentViewModel.AnnualExtraCosts;

        public double AnnualCosts => _rentViewModel.AnnualCostsTotal;

        public ICollectionView RoomCosts { get; set; }

        public RentCostsViewModel(RentViewModel rentViewModel, FlatViewModel flatViewModel)
        {
            _rentViewModel = rentViewModel;
            _flatViewModel = flatViewModel;

            RoomCosts = CollectionViewSource.GetDefaultView(_rentViewModel.RoomCosts);
            RoomCosts.SortDescriptions.Add(new SortDescription("Room.ID", ListSortDirection.Ascending));
        }
    }
}
