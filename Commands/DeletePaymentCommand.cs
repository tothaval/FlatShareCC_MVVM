/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  DeletePaymentCommand 
 * 
 *  command for removing a paymentviewmodel
 *  instance from a roomviewmodel instance
 */
using SharedLivingCostCalculator.ViewModels;
using System.Collections;
using System.Windows;

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
                        _paymentsSetupViewModel.RoomPaymentsViewModel.RoomPayments.Payments.Remove(item.GetPayment);

                    }
                }
            }
        }


    }
}
// EOF