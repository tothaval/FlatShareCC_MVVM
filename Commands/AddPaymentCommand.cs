/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  AddPaymentCommand 
 * 
 *  command for adding a new paymentviewmodel
 *  instance to a roomviewmodel instance
 */
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;

namespace SharedLivingCostCalculator.Commands
{
    internal class AddPaymentCommand : BaseCommand
    {

        // properties & fields
        #region properties

        private readonly RoomPaymentsViewModel _roomPaymentsViewModel;

        #endregion properties


        // constructors
        #region constructors

        public AddPaymentCommand(RoomPaymentsViewModel roomPaymentsViewModel)
        {
            _roomPaymentsViewModel = roomPaymentsViewModel;
        }

        #endregion constructors


        // methods
        #region methods

        public override void Execute(object? parameter)
        {
            if (parameter.GetType() == typeof(int))
            {
                int quantity = (int)parameter;

                for (int i = 0; i < quantity; i++)
                {
                    _roomPaymentsViewModel.AddPayment();

                    //_paymentsSetupViewModel.RoomViewModel.RegisterPaymentEvents();
                }
            }
        }

        #endregion methods


    }
}
// EOF