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


        public double OtherCostItemCountBasedWidth => RentViewModel.Costs.Count * 105;


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

        #endregion properties


        // constructors
        #region constructors
        public RentCostsViewModel(RentViewModel rentViewModel, FlatViewModel flatViewModel)
        {
            RentViewModel = rentViewModel;
            _flatViewModel = flatViewModel;                      
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