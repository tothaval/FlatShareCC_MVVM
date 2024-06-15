/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  NavigationStore 
 * 
 *  storage class for navigation infrastructure
 *  using INavigationService, NavigationService,
 *  NavigationStore and NavigationCommand to
 *  change CurrentViewModel binding of MainWindow.xaml
 */
using SharedLivingCostCalculator.ViewModels;


namespace SharedLivingCostCalculator.Navigation
{
    class NavigationStore
    {

        public event Action CurrentViewModelChanged;


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


        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }


    }
}
// EOF