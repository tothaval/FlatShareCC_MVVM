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

        // properties & fields
        #region properties

        private readonly INavigationService _navigationService;

        #endregion properties


        // constructors
        #region constructors

        public NavigateCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #endregion constructors


        // methods
        #region methods

        public override void Execute(object? parameter)
        {
            _navigationService.ChangeView();
        }

        #endregion methods


    }
}
// EOF