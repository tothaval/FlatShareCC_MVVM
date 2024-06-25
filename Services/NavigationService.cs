/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  INavigationService 
 * 
 *  interface for navigation infrastructure
 *  using NavigationStore, NavigateCommand
 *  and NavigationService to change 
 *  MainWindow.xaml CurrentViewModel
 */
using SharedLivingCostCalculator.Interfaces;
using SharedLivingCostCalculator.Navigation;
using SharedLivingCostCalculator.ViewModels.ViewLess;


namespace SharedLivingCostCalculator.Services
{
    internal class NavigationService<TViewModel> : INavigationService 
        where TViewModel : BaseViewModel
    {

        // properties & fields
        #region properties

        private readonly Func<TViewModel> _createViewModel;


        private readonly NavigationStore _navigationStore;

        #endregion properties


        // constructors
        #region constructors

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        #endregion constructors


        // methods
        #region methods

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
        #endregion methods


    }
}
// EOF