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

        // Properties & Fields
        #region Properties & Fields

        private readonly RoomPaymentsViewModel _RoomPaymentsViewModel;

        #endregion


        // Constructors
        #region Constructors

        public AddPaymentCommand(RoomPaymentsViewModel roomPaymentsViewModel)
        {
            _RoomPaymentsViewModel = roomPaymentsViewModel;
        }

        #endregion


        // Methods
        #region Methods

        public override void Execute(object? parameter)
        {
            if (parameter.GetType() == typeof(int))
            {
                int quantity = (int)parameter;

                for (int i = 0; i < quantity; i++)
                {
                    _RoomPaymentsViewModel.AddPayment();
                }
            }
        }

        #endregion


    }
}
// EOF