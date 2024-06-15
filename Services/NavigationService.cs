/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  INavigationService 
 * 
 *  interface for navigation infrastructure
 *  using NavigationStore, NavigateCommand
 *  and NavigationService to change 
 *  MainWindow.xaml CurrentViewModel
 */
using SharedLivingCostCalculator.Navigation;
using SharedLivingCostCalculator.ViewModels;


namespace SharedLivingCostCalculator.Services
{
    internal class NavigationService<TViewModel> : INavigationService 
        where TViewModel : BaseViewModel
    {

        private readonly NavigationStore _navigationStore;


        private readonly Func<TViewModel> _createViewModel;


        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }


        public void ChangeView(BaseViewModel viewModel) { 
            _navigationStore.CurrentViewModel = viewModel;
        }


        public void ChangeView()
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }


        public NavigationStore GetNavigationStore()
        {
            return _navigationStore;
        }


        public BaseViewModel GetViewModel()
        {
            return _navigationStore.CurrentViewModel;
        }


    }
}
// EOF