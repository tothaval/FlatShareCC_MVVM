using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class PaymentManagementViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private ValidationHelper _helper = new ValidationHelper();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool HasErrors => _helper.HasErrors;
        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        private readonly FlatViewModel _flatViewModel;
        public ObservableCollection<RoomViewModel> Rooms => _flatViewModel.Rooms;

        private PaymentsSetupViewModel _updateViewModel;
        public PaymentsSetupViewModel UpdateViewModel
        {
            get { return _updateViewModel; }
            set
            {
                _updateViewModel = value;
                OnPropertyChanged(nameof(UpdateViewModel));
            }
        }


        private RoomViewModel _selectedValue; // private Billing _selectedBillingPeriod
        public RoomViewModel SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue == value) return;
                _selectedValue = value;

                UpdateViewModel = new PaymentsSetupViewModel(SelectedValue);

                OnPropertyChanged(nameof(SelectedValue));

                foreach (RoomViewModel room in Rooms)
                {
                    room.RegisterPaymentEvents();
                    room.DetermineValues();
                }
            }
        }

        //private int _quantity;

        //public int Quantity
        //{
        //    get { return _quantity; }
        //    set
        //    {
        //        _quantity = value;
        //        OnPropertyChanged(nameof(Quantity));
        //    }
        //}

        public PaymentManagementViewModel(FlatViewModel flatViewModel)
        {
            _flatViewModel = flatViewModel;

            foreach (RoomViewModel room in Rooms)
            {
                room.DetermineValues();
            }
        }
    }
}
