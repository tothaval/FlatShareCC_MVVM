using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    class AccountingViewModel : BaseViewModel
    {
        private FlatViewModel _flatViewModel;
        public FlatViewModel FlatViewModel => _flatViewModel;

        private RentManagementViewModel _rents;
        public RentManagementViewModel Rents => _rents;

        private BillingManagementViewModel _billings;
        public BillingManagementViewModel Billings => _billings;

        private PaymentManagementViewModel _payments;
        public PaymentManagementViewModel Payments => _payments;

        public ICommand LeaveCommand { get; }

        public string Address => _flatViewModel.Address;
        public string Details => _flatViewModel.Details;
        public double Area  => _flatViewModel.Area;
        public int RoomCount => _flatViewModel.RoomCount;

        public double FontSize => (double)App.Current.FindResource("FS") * 2;


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



        public AccountingViewModel(FlatViewModel flatViewModel, INavigationService navigationService)
        {
            _flatViewModel = flatViewModel;
            LeaveCommand = new NavigateCommand(navigationService);

            _billings = new BillingManagementViewModel(FlatViewModel, this);
            _payments = new PaymentManagementViewModel(FlatViewModel);
            _rents = new RentManagementViewModel(FlatViewModel);
        }
    }
}
