/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RentUpdateViewModel  : BaseViewModel
 * 
 *  viewmodel for RentUpdateView
 *  
 *  allows for editing of RentViewModel
 *  
 *  is encapsulated within a RentManagementViewModel
 */
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels.Financial
{
    public class RentUpdateViewModel : BaseViewModel
    {

        // properties & fields
        #region properties
        
        private CreditViewViewModel _CreditViewViewModel;
        public CreditViewViewModel CreditViewViewModel
        {
            get { return _CreditViewViewModel; }
            set
            {
                _CreditViewViewModel = value;
                OnPropertyChanged(nameof(CreditViewViewModel));
            }
        }


        private bool _DataLockCheckbox;
        public bool DataLockCheckbox
        {
            get { return _DataLockCheckbox; }
            set
            {
                _DataLockCheckbox = value;
                _RentViewModel.HasDataLock = _DataLockCheckbox;
                OnPropertyChanged(nameof(DataLockCheckbox));
                OnPropertyChanged(nameof(HasDataLock));
            }
        }

        
        public bool HasDataLock => !DataLockCheckbox;


        private OtherCostsRentViewModel _OtherCostsRentViewModel;
        public OtherCostsRentViewModel OtherCostsRentViewModel
        {
            get { return _OtherCostsRentViewModel; }
            set
            {
                _OtherCostsRentViewModel = value;
                OnPropertyChanged(nameof(OtherCostsRentViewModel));
            }
        }


        private readonly RentViewModel _RentViewModel;
        public RentViewModel RentViewModel => _RentViewModel;


        private int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                _SelectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        #endregion properties


        // constructors
        #region constructors

        public RentUpdateViewModel(FlatViewModel flatViewModel, RentViewModel rentViewModel)
        {
            _RentViewModel = rentViewModel;

            if (_RentViewModel == null)
            {
                _RentViewModel = new RentViewModel(flatViewModel, new Rent());
            }


            OtherCostsRentViewModel = new OtherCostsRentViewModel(rentViewModel);
            CreditViewViewModel = new CreditViewViewModel(rentViewModel);

            SelectedIndex = 0;            
        }

        #endregion constructors


    }
}
// EOF