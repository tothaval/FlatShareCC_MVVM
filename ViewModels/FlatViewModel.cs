using SharedLivingCostCalculator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class FlatViewModel : BaseViewModel
    {
        private Flat _flat;

        public int ID => _flat.ID;
        public string Address { get { return _flat.Address; } set { _flat.Address = value; } }
        public string Details { get { return _flat.Details; } set { _flat.Details = value; } } 
        public double Area { get { return _flat.Area; } set { _flat.Area = value; } }
        public int Rooms { get { return _flat.RoomCount; } set { _flat.RoomCount = value; CreateRooms(); } }
        public string Sizes => GetSizes();

        public ObservableCollection<Room> rooms { get { return _flat.Rooms; } set { _flat.Rooms = value; } }

        public ObservableCollection<BillingPeriod> BillingPeriods
        {
            get { return _flat.BillingPeriods; }
            set { _flat.BillingPeriods = value; }
        }


        public ObservableCollection<Rent> RentUpdates
        {
            get { return _flat.RentUpdates; }
            set { _flat.RentUpdates = value; }
        }


        public FlatViewModel(Flat flat)
        {
            _flat = flat;

            ConnectRooms();
        }

        private void ConnectRooms()
        {
            if (rooms != null && rooms.Count > 0)
            {
                foreach (Room room in rooms)
                {
                    room.PropertyChanged += Room_PropertyChanged;
                }
            }
        }


        private void CreateRooms()
        {
            rooms.Clear();

            for (int i = 0; i < Rooms; i++)
            {
                Room room = new Room(i, $"room{i + 1}", 0);

                room.PropertyChanged += Room_PropertyChanged;
                rooms.Add(room);
            }
        }

        private void Room_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Room? room = sender as Room;

            if (room != null)
            {
                double area = _flat.Area;

                foreach (Room item in _flat.Rooms)
                {
                    area -= item.RoomArea;
                }

                if (area < 0)
                {
                    room.RoomArea = 0;
                    MessageBox.Show("combined area of rooms is larger than flat area");
                }
                
            }

            

        }

        public event Action RoomCreation;

        public string GetSizes()
        {
            StringBuilder sizes = new StringBuilder();

            foreach (Room room in _flat.Rooms)
            {
                sizes.Append($"<{room.RoomName} {room.RoomArea}>");
            }

            return sizes.ToString();
        }

    }
}
