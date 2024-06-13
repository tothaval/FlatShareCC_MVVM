using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace SharedLivingCostCalculator.Commands
{
    internal class DeletePaymentCommand : BaseCommand
    {
        private readonly PaymentsSetupViewModel _paymentsSetupViewModel;


        public DeletePaymentCommand(PaymentsSetupViewModel paymentsSetupViewModel)
        {
            _paymentsSetupViewModel = paymentsSetupViewModel;
        }


        public override void Execute(object? parameter)
        {
            IList selection = (IList)parameter;

            if (selection != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Do you wan't to delete selected payments?",
                    "Remove Payment(s)", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var selected = selection.Cast<PaymentViewModel>().ToArray();

                    foreach (var item in selected)
                    {
                        _paymentsSetupViewModel.RoomViewModel.Payments.Remove(item);
                    }
                }
            }
        }
    }
}
