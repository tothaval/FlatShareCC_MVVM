/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  AddPaymentCommand 
 * 
 *  command for adding a new paymentviewmodel
 *  instance to a roomviewmodel instance
 */
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.Commands
{
    internal class AddPaymentCommand : BaseCommand
    {

        private readonly RoomPaymentsViewModel _roomPaymentsViewModel;


        public AddPaymentCommand(RoomPaymentsViewModel roomPaymentsViewModel)
        {
            _roomPaymentsViewModel = roomPaymentsViewModel;
        }


        public override void Execute(object? parameter)
        {
            if (parameter.GetType() == typeof(int))
            {
                int quantity = (int)parameter;

                for (int i = 0; i < quantity; i++)
                {
                    _roomPaymentsViewModel.RoomPayments.Payments?.Add(

                        new Payment()
                        {
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now,
                            Sum = 0.0,
                            PaymentQuantity = 1
                        });

                    //_paymentsSetupViewModel.RoomViewModel.RegisterPaymentEvents();
                }
            }            
        }


    }
}
// EOF