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
        private readonly PaymentsViewModel _paymentsViewModel;


        public AddPaymentCommand(PaymentsViewModel paymentsViewModel)
        {
            _paymentsViewModel = paymentsViewModel;
        }


        public override void Execute(object? parameter)
        {
            if (parameter.GetType() == typeof(int) && _paymentsViewModel != null)
            {
                int quantity = (int)parameter;

                for (int i = 0; i < quantity; i++)
                {
                    _paymentsViewModel.RoomViewModel.Payments?.Add(
                        new Payment(_paymentsViewModel.RoomViewModel)
                        {
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now,
                            Sum = 0.0,
                            PaymentQuantity = 1
                        });
                }
            }            
        }
    }
}
