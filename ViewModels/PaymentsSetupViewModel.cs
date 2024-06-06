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
    internal class PaymentsSetupViewModel : BaseViewModel
    {
        private readonly RoomViewModel _roomViewModel;
        public RoomViewModel RoomViewModel => _roomViewModel;

        public ObservableCollection<PaymentViewModel> Payments => _roomViewModel.Payments;

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

        public ICommand AddPaymentCommand { get; }
        public ICommand DeletePaymentCommand { get; }

        public PaymentsSetupViewModel(RoomViewModel roomViewModel)
        {
            _roomViewModel = roomViewModel;
            _quantity = 1;

            AddPaymentCommand = new AddPaymentCommand(this);
            DeletePaymentCommand = new DeletePaymentCommand(this);
        }
    }
}
