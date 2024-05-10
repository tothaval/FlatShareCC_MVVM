using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WGMietkosten.Models;
using WGMietkosten.ViewModels;

namespace WGMietkosten.Commands
{
    class RemoveFlatSetupCommand : CommandBase
    {
        private readonly FlatManagementViewModel _viewModel;


        public RemoveFlatSetupCommand(FlatManagementViewModel flatManagementViewModel)
        {
            _viewModel = flatManagementViewModel;
        }

        public override void Execute(object? parameter)
        {
            //if (parameter != null)
            //{
            //    MessageBox.Show(parameter.ToString());
            //}

            if (parameter != null && parameter.GetType() == typeof(AddFlatViewModel))
            {
                AddFlatViewModel addFlatViewModel = parameter as AddFlatViewModel;

                if (addFlatViewModel != null)
                {
                    _viewModel.FlatSetups.Remove(addFlatViewModel);
                }
            }


            //parameter.

            //_viewModel.FlatSetups.

            //_flatManagement.
        }
    }
}
