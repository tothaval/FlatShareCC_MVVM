using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
            MainWindowTitle = CurrentViewModel.MainWindowTitleText;
        }
    }
}
