/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ShowFlatAccountingCommand 
 * 
 *  a specific navigate command which changes 
 *  the MainWindow.xaml CurrentViewModel to
 *  the AccountingViewModel
 */

using SharedLivingCostCalculator.Interfaces;
using SharedLivingCostCalculator.ViewModels;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.Commands
{
    class ShowFlatAccountingCommand : BaseCommand
    {
        private INavigationService _navigationService;


        public ShowFlatAccountingCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }


        public override void Execute(object? parameter)
        {
            if (parameter != null && parameter.GetType()==typeof(FlatViewModel))
            {
                _navigationService.ChangeView(new AccountingViewModel(
                                        (FlatViewModel)parameter, _navigationService));
            }

        }


    }
}
// EOF