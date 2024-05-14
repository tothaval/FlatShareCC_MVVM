using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class FlatManagementViewModel : BaseViewModel
    {
        private string headerText;

        public string HeaderText
        {
            get { return headerText; }
            set { headerText = value; OnPropertyChanged(nameof(HeaderText)); }
        }

        private ObservableCollection<FlatViewModel> _flatCollection;
        public ObservableCollection<FlatViewModel> FlatCollection => _flatCollection;


        public ICommand NewFlatCommand { get; }
        public ICommand EnterFlatCommand { get; }

        public FlatManagementViewModel(ObservableCollection<FlatViewModel> flatCollection, INavigationService newFlatSetupNavigationService)
        {
            NewFlatCommand = new CreateNewFlatCommand(newFlatSetupNavigationService);

            _flatCollection = flatCollection;

            MainWindowTitleText = "Shared Living Cost Calculator - Flat Overview";
            HeaderText = "Flat Overview";
        }


        public void NewFlat(FlatViewModel flatViewModel)
        {
            FlatCollection.Add(flatViewModel);
        }
    }
}
