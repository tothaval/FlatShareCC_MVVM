using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WGMietkosten.Models;
using WGMietkosten.Navigation;
using WGMietkosten.ViewModels;

namespace WGMietkosten.Commands
{
    class AddFlatSetupCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public AddFlatSetupCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;        
        }

        public override void Execute(object? parameter)
        {
            _navigationStore.CurrentViewModel = new FlatSetupViewModel(_navigationStore);
        }

    }
}
