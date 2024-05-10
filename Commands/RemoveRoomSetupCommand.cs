using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGMietkosten.Models;
using WGMietkosten.ViewModels;

namespace WGMietkosten.Commands
{
    internal class RemoveRoomSetupCommand : CommandBase
    {
        private readonly RoomSetupViewModel _viewModel;


        public RemoveRoomSetupCommand(RoomSetupViewModel roomSetupViewModel)
        {
            _viewModel = roomSetupViewModel;
        }

        public override void Execute(object? parameter)
        {
            //if (parameter != null)
            //{
            //    MessageBox.Show(parameter.ToString());
            //}

            if (parameter != null && parameter.GetType() == typeof(EditRoomsViewModel))
            {
                EditRoomsViewModel addFlatViewModel = parameter as EditRoomsViewModel;

                if (addFlatViewModel != null)
                {
                    _viewModel.Rooms.Remove(addFlatViewModel);
                }
            }


            //parameter.

            //_viewModel.FlatSetups.

            //_flatManagement.
        }
    }
}
