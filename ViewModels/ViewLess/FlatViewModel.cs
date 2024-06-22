/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  FlatViewModel  : BaseViewModel
 * 
 *  viewmodel for Flat model
 *  
 *  the most important data object
 */
using SharedLivingCostCalculator.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace SharedLivingCostCalculator.ViewModels.ViewLess
{
    public class FlatViewModel : BaseViewModel
    {

        public event Action RoomCreation;


        private Flat _flat;
        public Flat GetFlat => _flat;


        public int ID => _flat.ID;


        public string Address { get { return _flat.Address; } set { _flat.Address = value; OnPropertyChanged(nameof(Address)); } }


        public string Details { get { return _flat.Details; } set { _flat.Details = value; OnPropertyChanged(nameof(Details)); } }


        public double Area { get { return _flat.Area; } set { _flat.Area = value; OnPropertyChanged(nameof(Area)); OnPropertyChanged(nameof(SharedArea)); } }


        public int RoomCount { get { return _flat.RoomCount; } set { _flat.RoomCount = value;  CreateRooms(); } }


        public string FlatNotes { get { return _flat.FlatNotes; } set { _flat.FlatNotes = value; OnPropertyChanged(nameof(FlatNotes)); } }


        public ObservableCollection<RoomCostsViewModel> CurrentRoomCosts { get; set; }


        public ObservableCollection<RoomViewModel> Rooms
        {
            get { return _flat.Rooms; }
            set
            {
                _flat.Rooms = value;
                OnPropertyChanged(nameof(Rooms));
            }
        }


        public ObservableCollection<RentViewModel> RentUpdates
        {
            get { return _flat.RentUpdates; }
            set
            {
                _flat.RentUpdates = value;
                OnPropertyChanged(nameof(RentUpdates));
            }
        }


        public double ExtraCosts => CurrentExtraCosts();


        public double SharedExtraCosts => CalculateSharedExtraCosts();


        public double Rent => CurrentRent();


        public double SharedRent => CalculateSharedRent();


        public double SharedArea => CalculateSharedArea();


        public double CombinedRoomArea => CalculateCombinedRoomArea();


        public FlatViewModel(Flat flat)
        {
            _flat = flat;
            CurrentRoomCosts = new ObservableCollection<RoomCostsViewModel>();

            ConnectRooms();
        }

        
        private double CalculateCombinedRoomArea()
        {
            double combinedRoomArea = 0.0;

            foreach (RoomViewModel room in Rooms)
            {
                combinedRoomArea += room.RoomArea;
            }

            return combinedRoomArea;
        }


        private double CalculateSharedArea()
        {
            double shared_area = Area;

            foreach (RoomViewModel room in Rooms)
            {
                shared_area -= room.RoomArea;
            }

            return shared_area;

        }


        private double CalculateSharedExtraCosts()
        {
            double shared_area = CalculateSharedArea();

            double shared_rent = shared_area / Area * CurrentExtraCosts();

            return shared_rent;

        }


        private double CalculateSharedRent()
        {
            double shared_area = CalculateSharedArea();

            double shared_rent = shared_area / Area * CurrentRent();

            return shared_rent;

        }

        public void ConnectRooms()
        {
            if (Rooms != null && Rooms.Count > 0)
            {
                foreach (RoomViewModel room in Rooms)
                {
                    room.RoomAreaChanged += Room_RoomAreaChanged;

                    //room.PropertyChanged += Room_PropertyChanged;
                }
            }
        }

        private void Room_RoomAreaChanged(object? sender, EventArgs e)
        {    
            if (CombinedRoomArea > Area)
                {
                    MessageBox.Show("combined area of Rooms is larger than flat area");
                }

                OnPropertyChanged(nameof(CombinedRoomArea));
                OnPropertyChanged(nameof(SharedArea));
            
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


        private double CurrentExtraCosts()
        {
            RentViewModel currentRent = new RentViewModel(this, new Rent());
            currentRent.StartDate = new DateTime(1, 1, 1);

            foreach (RentViewModel rent in RentUpdates)
            {
                if (rent.StartDate > currentRent.StartDate)
                {
                    currentRent = rent;
                }
            }

            return currentRent.ExtraCostsTotal;
        }


        private double CurrentRent()
        {
            RentViewModel currentRent = new RentViewModel(this, new Rent());
            currentRent.StartDate = new DateTime();

            foreach (RentViewModel rent in RentUpdates)
            {
                if (rent.StartDate > currentRent.StartDate)
                {
                    currentRent = rent;
                }
            }

            return currentRent.ColdRent;
        }


        public RentViewModel? GetMostRecentRent()
        {
            RentViewModel? rentViewModel = null;
            if (RentUpdates.Count > 0)
            {
                foreach (RentViewModel rent in RentUpdates)
                {
                    if (rentViewModel == null)
                    {
                        rentViewModel = rent;

                        continue;
                    }

                    if (rent.StartDate > rentViewModel.StartDate)
                    {
                        rentViewModel = rent;
                    }
                }
            }

            return rentViewModel;
        }


        public void SetMostRecentCosts()
        {
            RentViewModel? rent = GetMostRecentRent();

            if (rent != null && rent.RoomCosts != null)
            {
                CurrentRoomCosts = rent.RoomCosts;
            }

            OnPropertyChanged(nameof(CurrentRoomCosts));
        }


        private void Room_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RoomViewModel? room = sender as RoomViewModel;

            if (room != null)
            {
                if (CombinedRoomArea > Area)
                {
                    MessageBox.Show("combined area of Rooms is larger than flat area");
                }

                OnPropertyChanged(nameof(CombinedRoomArea));
                OnPropertyChanged(nameof(SharedArea));
            }
        }    


    }
}
// EOF