/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  NavigateCommand 
 *  
 *  part of the navigation infrastructure using INavigationService,
 *  NavigationService, NavigationStore and NavigationCommand to
 *  change CurrentViewModel binding of MainWindow.xaml
 */

using SharedLivingCostCalculator.Interfaces;

namespace SharedLivingCostCalculator.Commands
{
    internal class NavigateCommand : BaseCommand
    {

        private readonly INavigationService _navigationService;


        public NavigateCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }


        public override void Execute(object? parameter)
        {
            _navigationService.ChangeView();
        }


    }
}
// EOF