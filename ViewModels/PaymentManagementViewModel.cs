using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class PaymentManagementViewModel : BaseViewModel
    {
        private readonly FlatViewModel _flatViewModel;
        public ObservableCollection<RoomViewModel> Rooms { get; set; }

        private PaymentsViewModel _updateViewModel;
        public PaymentsViewModel UpdateViewModel
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

                UpdateViewModel = new PaymentsViewModel(SelectedValue);

                OnPropertyChanged(nameof(SelectedValue));
            }
        }

        private int _quantity;

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        //private BaseViewModel currentPaymentViewModel;

        //public BaseViewModel CurrentPaymentViewModel
        //{
        //    get { return currentPaymentViewModel; }
        //    set {
        //        currentPaymentViewModel = value;
        //        OnPropertyChanged(nameof(CurrentPaymentViewModel));
        //    }
        //}


        //public ICommand AddPaymentCommand { get; }
        //public ICommand DeletePaymentCommand { get; }

        public PaymentManagementViewModel(FlatViewModel flatViewModel)
        {
            _flatViewModel = flatViewModel;
            Rooms = new ObservableCollection<RoomViewModel>();
            _quantity = 1;


            foreach (Room room in _flatViewModel.Rooms)
            {
                Rooms.Add(new RoomViewModel(room));
            }

            //AddPaymentCommand = new AddPaymentCommand(this);
            //DeletePaymentCommand = new DeletePaymentCommand(this);
        }
        
    }
}
