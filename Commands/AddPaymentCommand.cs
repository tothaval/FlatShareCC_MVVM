using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SharedLivingCostCalculator.Commands
{
    internal class AddPaymentCommand : BaseCommand
    {
        private readonly PaymentsSetupViewModel _paymentsSetupViewModel;


        public AddPaymentCommand(PaymentsSetupViewModel paymentsViewModel)
        {
            _paymentsSetupViewModel = paymentsViewModel;
        }


        public override void Execute(object? parameter)
        {
            if (parameter.GetType() == typeof(int) && _paymentsSetupViewModel != null)
            {
                int quantity = (int)parameter;

                for (int i = 0; i < quantity; i++)
                {
                    _paymentsSetupViewModel.RoomViewModel.Payments?.Add(
                        //new PaymentViewModel(_paymentsSetupViewModel.RoomViewModel)
                        new PaymentViewModel(new Payment()
                        {
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now,
                            Sum = 0.0,
                            PaymentQuantity = 1
                        }));
                    _paymentsSetupViewModel.RoomViewModel.RegisterPaymentEvents();
                }
            }            
        }
    }
}
