/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ShowFlatAccountingCommand 
 * 
 *  a specific navigate command which changes 
 *  the MainWindow.xaml CurrentViewModel to
 *  the AccountingViewModel
 */
using SharedLivingCostCalculator.Navigation;
using SharedLivingCostCalculator.ViewModels;


namespace SharedLivingCostCalculator.Services
{
    internal interface INavigationService
    {

        void ChangeView();


        void ChangeView(BaseViewModel baseViewModel);


        BaseViewModel GetViewModel();


        NavigationStore GetNavigationStore();

    
    }
}
// EOF