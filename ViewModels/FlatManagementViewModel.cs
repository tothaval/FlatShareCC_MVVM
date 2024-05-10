using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WGMietkosten.Commands;
using WGMietkosten.Models;
using WGMietkosten.Navigation;

namespace WGMietkosten.ViewModels
{
    class FlatManagementViewModel : ViewModelBase
    {
        private ObservableCollection<AddFlatViewModel> _flatSetups;
        public ObservableCollection<AddFlatViewModel> FlatSetups => _flatSetups;

        private NavigationStore _navigationStore;

        public ICommand AddFlatCommand { get; }
        public ICommand RemoveFlatCommand { get; }
        public ICommand ShowFlatCommand { get; }

        public FlatManagementViewModel(NavigationStore navigationStore)
        {
            _flatSetups = new ObservableCollection<AddFlatViewModel>();
            _navigationStore = navigationStore;

            foreach (FlatSetup flatSetup in navigationStore.FlatManager.GetFlats())
            {
                _flatSetups.Add(new AddFlatViewModel(flatSetup));
            }
            //[
            //    new AddFlatViewModel(new FlatSetup(
            //         0, "Königsbrücker Platz 2, 01097 Dresden", "VH 3.OG Links", 88.88, 3)),
                
            //    new AddFlatViewModel(new FlatSetup(
            //         1, "Rudolf-Leonhardt-Straße 38, 01097 Dresden", "VH 2.OG Links", 60, 2)),
                
            //    new AddFlatViewModel(new FlatSetup(
            //         2, "Königsbrücker Platz 2, 01097 Dresden", "VH 1.OG Rechts", 92, 4)),
            // ];

            AddFlatCommand = new AddFlatSetupCommand(navigationStore);
            RemoveFlatCommand = new RemoveFlatSetupCommand(this);

            ShowFlatCommand = new ShowRoomSetupCommand(navigationStore);
        }

        public void AddFlatSetup(FlatSetup flatSetup)
        {
            _flatSetups.Add(new AddFlatViewModel(flatSetup));

             _navigationStore.FlatManager.AddFlat(flatSetup);
        }
    }
}
