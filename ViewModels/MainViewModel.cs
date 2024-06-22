/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  MainViewModel  : BaseViewModel
 * 
 *  viewmodel for MainWindow
 */
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;


namespace SharedLivingCostCalculator.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {

        private string mainWindowTitle;


        public string MainWindowTitle
        {
            get { return mainWindowTitle; }
            set { mainWindowTitle = value; OnPropertyChanged(nameof(MainWindowTitle)); }
        }


        public BaseViewModel CurrentViewModel { get; set; }


        public MainViewModel(ObservableCollection<FlatViewModel> flatViewModels)
        {
            CurrentViewModel = new FlatManagementViewModel(flatViewModels);
        }


    }
}
// EOF