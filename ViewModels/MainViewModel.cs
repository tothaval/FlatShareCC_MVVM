/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  MainViewModel  : BaseViewModel
 * 
 *  viewmodel for MainWindow
 */
using SharedLivingCostCalculator.Navigation;
using SharedLivingCostCalculator.ViewModels.ViewLess;


namespace SharedLivingCostCalculator.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {

        private readonly NavigationStore _navigationStore;


        private string mainWindowTitle;


        public string MainWindowTitle
        {
            get { return mainWindowTitle; }
            set { mainWindowTitle = value; OnPropertyChanged(nameof(MainWindowTitle)); }
        }


        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;


        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }


        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }


    }
}
// EOF