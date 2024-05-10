using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WGMietkosten.Models;

namespace WGMietkosten.ViewModels
{
    internal class EditRoomsViewModel : ViewModelBase
    {
        private RoomSetup _roomSetup;

        public int ID { get  => _roomSetup.ID; }
        public string RoomName { get => _roomSetup.RoomName; set => _roomSetup.RoomName = value; }
        public double Area  { get => _roomSetup.Area; set => _roomSetup.Area = value; }
    public string TenantName { get => _roomSetup.TenantName; set => _roomSetup.TenantName = value; }

public EditRoomsViewModel(RoomSetup roomSetup)
        {
            _roomSetup = roomSetup;
        }

        public RoomSetup GetRoomSetup()
        {
            return _roomSetup;
        }


    }
}
