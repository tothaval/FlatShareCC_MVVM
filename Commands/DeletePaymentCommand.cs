/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  DeletePaymentCommand 
 * 
 *  command for removing a paymentviewmodel instance from a _BillingViewModel instance
 */
using SharedLivingCostCalculator.ViewModels.Financial;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System.Collections;
using System.Windows;

namespace SharedLivingCostCalculator.Commands
{
    internal class DeletePaymentCommand : BaseCommand
    {

        // Properties & Fields
        #region Properties & Fields

        private readonly PaymentsSetupViewModel _PaymentsSetupViewModel;

        #endregion


        // Constructors
        #region Constructors

        public DeletePaymentCommand(PaymentsSetupViewModel paymentsSetupViewModel)
        {
            _PaymentsSetupViewModel = paymentsSetupViewModel;
        }

        #endregion


        // Methods
        #region Methods

        public override void Execute(object? parameter)
        {
            IList selection = (IList)parameter;

            if (selection != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Do you want to delete selected payments?",
                    "Remove Payment(s)", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var selected = selection.Cast<PaymentViewModel>().ToArray();

                    foreach (var item in selected)
                    {
                        _PaymentsSetupViewModel.RoomPaymentsViewModel.RoomPayments.Payments.Remove(item.Payment);

                    }
                }
            }
        }

        #endregion


    }
}
// EOF