using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WGMietkosten.Commands;
using WGMietkosten.Models;
using WGMietkosten.Navigation;

namespace WGMietkosten.ViewModels
{
    internal class RoomSetupViewModel : ViewModelBase
    {
        private ObservableCollection<EditRoomsViewModel> _rooms;
        public ObservableCollection<EditRoomsViewModel> Rooms => _rooms;

        public ICommand AddRoomCommand { get; }
        public ICommand RemoveRoomCommand { get; }
        public ICommand LeaveRoomSetupCommand { get; }

        public RoomSetupViewModel(NavigationStore navigationStore, AddFlatViewModel addFlatViewModel)
        {
            _rooms = new ObservableCollection<EditRoomsViewModel>();

            //AddRoomCommand = new AddFlatSetupCommand(navigationStore, ref flatManagement);

            for (int i = 0; i < addFlatViewModel.Rooms ; i++)
            {
                RoomSetup roomSetup = new RoomSetup(
                    i, "room name", 0.0, "tenant");

                AddRoomSetup(roomSetup);

                addFlatViewModel.GetFlatSetup().Rooms.Add(roomSetup);
            }
                      

            RemoveRoomCommand = new RemoveRoomSetupCommand(this);

            LeaveRoomSetupCommand = new LeaveRoomSetupViewCommand(navigationStore);
        }

        public void AddRoomSetup(RoomSetup roomSetup)
        {
            _rooms.Add(new EditRoomsViewModel(roomSetup));
        }
    }
}
