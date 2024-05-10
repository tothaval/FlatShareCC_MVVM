using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGMietkosten.Models;
using WGMietkosten.Navigation;
using WGMietkosten.ViewModels;

namespace WGMietkosten.Commands
{
    class CancelFlatSetupCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public CancelFlatSetupCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object? parameter)
        {
            _navigationStore.CurrentViewModel = new FlatManagementViewModel(_navigationStore);
        }
    }
}
