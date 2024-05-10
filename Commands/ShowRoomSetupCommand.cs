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
    internal class ShowRoomSetupCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public ShowRoomSetupCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object? parameter)
        {
            if ( parameter != null && parameter.GetType() == typeof(AddFlatViewModel))
            {
                AddFlatViewModel addFlatViewModel = (AddFlatViewModel) parameter;

                _navigationStore.CurrentViewModel = new RoomSetupViewModel(_navigationStore, addFlatViewModel);
            }            
        }
    }
}
