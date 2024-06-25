/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  MainViewModel  : BaseViewModel
 * 
 *  viewmodel for MainWindow
 */
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;


namespace SharedLivingCostCalculator.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {

        // properties & fields
        #region properties

        public BaseViewModel CurrentViewModel { get; set; }


        private string mainWindowTitle;
        public string MainWindowTitle
        {
            get { return mainWindowTitle; }
            set { mainWindowTitle = value; OnPropertyChanged(nameof(MainWindowTitle)); }
        }

        #endregion properties


        // constructors
        #region constructors

        public MainViewModel(ObservableCollection<FlatViewModel> flatViewModels)
        {
            CurrentViewModel = new FlatManagementViewModel(flatViewModels);
        }

        #endregion constructors


    }
}
// EOF