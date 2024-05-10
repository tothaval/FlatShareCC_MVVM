using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WGMietkosten.Models;
using WGMietkosten.Navigation;
using WGMietkosten.ViewModels;

namespace WGMietkosten.Commands
{
    internal class LeaveRoomSetupViewCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public LeaveRoomSetupViewCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object? parameter)
        {
            _navigationStore.CurrentViewModel = new FlatManagementViewModel(_navigationStore);
        }
    }
}
