using SharedLivingCostCalculator.Calculations;
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
    public class FlatViewModel : BaseViewModel
    {
        private Flat _flat;
        public Flat GetFlat => _flat;

        private Costs _costs;
        public Costs Costs
        {
            get { return _costs; }
        }

        public int ID => _flat.ID;
        public string Address { get { return _flat.Address; } set { _flat.Address = value; } }
        public string Details { get { return _flat.Details; } set { _flat.Details = value; } } 
        public double Area { get { return _flat.Area; } set { _flat.Area = value; } }
        public int RoomCount { get { return _flat.RoomCount; } set { _flat.RoomCount = value; CreateRooms(); } }

        public ObservableCollection<RoomViewModel> Rooms { get { return _flat.Rooms; } set { _flat.Rooms = value;
                OnPropertyChanged(nameof(Rooms));
            } }

        public ObservableCollection<BillingViewModel> BillingPeriods
        {
            get { return _flat.BillingPeriods; }
            set { _flat.BillingPeriods = value;
                OnPropertyChanged(nameof(BillingPeriods));
            }
        }

        public ObservableCollection<RentViewModel> RentUpdates
        {
            get { return _flat.RentUpdates; }
            set { _flat.RentUpdates = value;
                OnPropertyChanged(nameof(RentUpdates));
            }
        }



        public FlatViewModel(Flat flat)
        {
            _flat = flat;
            _costs = new Costs(this);

            ConnectRooms();
        }

        private void ConnectRooms()
        {
            if (Rooms != null && Rooms.Count > 0)
            {
                foreach (RoomViewModel room in Rooms)
                {
                    room.PropertyChanged += Room_PropertyChanged;
                }
            }
        }


        private void CreateRooms()
        {
            Rooms.Clear();

            for (int i = 0; i < RoomCount; i++)
            {
                RoomViewModel room = new RoomViewModel(new Room(i, $"room{i + 1}", 0));

                room.PropertyChanged += Room_PropertyChanged;
                Rooms.Add(room);
            }
        }

        private void Room_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RoomViewModel? room = sender as RoomViewModel;

            if (room != null)
            {
                double area = _flat.Area;

                foreach (RoomViewModel item in _flat.Rooms)
                {
                    area -= item.RoomArea;
                }

                if (area < 0)
                {
                    room.RoomArea = 0;
                    MessageBox.Show("combined area of Rooms is larger than flat area");
                }
                
            }

            

        }

        public event Action RoomCreation;

    }
}
