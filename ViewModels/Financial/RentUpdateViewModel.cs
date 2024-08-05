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
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;


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
                _rentViewModel.HasDataLock = _DataLockCheckbox;
                OnPropertyChanged(nameof(DataLockCheckbox));
                OnPropertyChanged(nameof(HasDataLock));
            }
        }

        
        public bool HasDataLock => !DataLockCheckbox;


        public bool HasErrors => ((INotifyDataErrorInfo)_helper).HasErrors;


        private ValidationHelper _helper = new ValidationHelper();


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


        private readonly RentViewModel _rentViewModel;
        public RentViewModel RentViewModel => _rentViewModel;


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


        // event properties & fields
        #region event handlers
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion event handlers


        // commands
        #region commands

        #endregion commands


        // constructors
        #region constructors
        public RentUpdateViewModel(FlatViewModel flatViewModel, RentViewModel rentViewModel)
        {
            _rentViewModel = rentViewModel;

            if (_rentViewModel == null)
            {
                _rentViewModel = new RentViewModel(flatViewModel, new Rent());
            }

            _helper.ErrorsChanged += (_, e) => ErrorsChanged?.Invoke(this, e);


            OtherCostsRentViewModel = new OtherCostsRentViewModel(rentViewModel);
            CreditViewViewModel = new CreditViewViewModel(rentViewModel);

            SelectedIndex = 0;            
        }
        #endregion constructors


        // methods
        #region methods

        #endregion methods


        // events        
        #region events
              
        #endregion events


    }
}
// EOF