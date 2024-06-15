/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  AddPaymentCommand 
 * 
 *  command for adding a new paymentviewmodel
 *  instance to a roomviewmodel instance
 */
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels;

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
// EOF