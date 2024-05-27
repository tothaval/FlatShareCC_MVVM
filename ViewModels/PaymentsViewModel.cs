using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class PaymentsViewModel : BaseViewModel
    {
        private readonly FlatViewModel _flatViewModel;
        public ObservableCollection<RoomViewModel> Rooms { get; set; }
        
        private RoomViewModel _selectedValue; // private BillingPeriod _selectedBillingPeriod

        public RoomViewModel SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue == value) return;
                _selectedValue = value;
                
                OnPropertyChanged(nameof(SelectedValue));
            }
        }

        private int _quantity;

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value;
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


        public ICommand AddPaymentCommand { get; }
        public ICommand DeletePaymentCommand { get; }

        public PaymentsViewModel(FlatViewModel flatViewModel)
        {
            _flatViewModel = flatViewModel;
            Rooms = new ObservableCollection<RoomViewModel>();
            _quantity = 1;


            foreach (Room room in _flatViewModel.rooms)
            {
                Rooms.Add(new RoomViewModel(room));
            }

            AddPaymentCommand = new AddPaymentCommand(this);
            DeletePaymentCommand = new DeletePaymentCommand(this);
        }
    }
}
