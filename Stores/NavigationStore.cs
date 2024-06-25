/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  NavigationStore 
 * 
 *  storage class for navigation infrastructure
 *  using INavigationService, NavigationService,
 *  NavigationStore and NavigationCommand to
 *  change CurrentViewModel binding of MainWindow.xaml
 */
using SharedLivingCostCalculator.ViewModels.ViewLess;


namespace SharedLivingCostCalculator.Navigation
{
    class NavigationStore
    {

        // properties & fields
        #region properties

        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        #endregion properties


        // event properties
        #region event properties

        public event Action CurrentViewModelChanged;

        #endregion event properties


        // methods
        #region methods

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }

        #endregion methods


    }
}
// EOF