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
        private readonly PaymentsViewModel _paymentsViewModel;


        public DeletePaymentCommand(PaymentsViewModel paymentsViewModel)
        {
            _paymentsViewModel = paymentsViewModel;
        }


        public override void Execute(object? parameter)
        {
            IList selection = (IList)parameter;


            var selected = selection.Cast<Payment>().ToArray();

            foreach (var item in selected)
            {
                _paymentsViewModel.RoomViewModel.Payments.Remove(item);
            }

        }
    }
}
