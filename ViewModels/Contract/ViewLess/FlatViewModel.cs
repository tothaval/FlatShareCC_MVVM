/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  FlatViewModel  : BaseViewModel
 * 
 *  viewmodel for Flat model
 *  
 *  the most important data object
 */
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
using System.Windows;

namespace SharedLivingCostCalculator.ViewModels.Contract.ViewLess
{
    public class FlatViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        public string Address { get { return _flat.Address; } set { _flat.Address = value; OnPropertyChanged(nameof(Address)); } }


        public double Area
        {
            get { return _flat.Area; }
            set
            {
                //if (_flat.Area > 0.0)
                //{
                //    MessageBoxResult result = MessageBox.Show(
                //        "Warning: If you change the value all existing\n" +
                //        "calculations will be effected.\n\n" +
                //        "Proceed?",
                //        "Change Flat Area", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //    if (result == MessageBoxResult.Yes)
                //    {
                //        _flat.Area = value;
                //    }
                //}
                //else
                //{
                _flat.Area = value;
                //}


                OnPropertyChanged(nameof(Area));
                OnPropertyChanged(nameof(SharedArea));
            }
        }


        public double CombinedRoomArea => CalculateCombinedRoomArea();


        public string Details { get { return _flat.Details; } set { _flat.Details = value; OnPropertyChanged(nameof(Details)); } }


        public double ExtraCosts => CurrentExtraCosts();


        private Flat _flat;
        public Flat GetFlat => _flat;


        public string FlatNotes { get { return _flat.FlatNotes; } set { _flat.FlatNotes = value; OnPropertyChanged(nameof(FlatNotes)); } }


        public double Rent => CurrentRent();


        public int RoomCount
        {
            get { return _flat.RoomCount; }

            set
            {
                //if (_flat.RoomCount > 1)
                //{
                //    MessageBoxResult result = MessageBox.Show(
                //        "Warning: If you insert a value less than the\n" +
                //        "current value, you will loose room data.\n" +
                //        "\n" +
                //        "Warning: If you change the value all existing\n" +
                //        "calculations will be effected.\n\n" +
                //        "Proceed?",
                //        "Change Room Count", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //    if (result == MessageBoxResult.Yes)
                //    {
                _flat.RoomCount = value;
                CreateRooms();

                OnPropertyChanged(nameof(Area));
                OnPropertyChanged(nameof(RoomCount));
                //}
                //}

                //if (_flat.RoomCount <= 1)
                //{
                //    _flat.RoomCount = value;
                //    CreateRooms();
                //}
            }
        }


        public double SharedArea => CalculateSharedArea();


        public double SharedExtraCosts => CalculateSharedExtraCosts();


        public double SharedRent => CalculateSharedRent();

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event Action RoomCreation;

        #endregion event properties & fields


        // collections
        #region collections

        public ObservableCollection<RoomCostsViewModel> CurrentRoomCosts { get; set; }


        public ObservableCollection<RentViewModel> RentUpdates
        {
            get { return _flat.RentUpdates; }
            set
            {
                _flat.RentUpdates = value;
                OnPropertyChanged(nameof(RentUpdates));
            }
        }


        public ObservableCollection<RoomViewModel> Rooms
        {
            get { return _flat.Rooms; }
            set
            {
                _flat.Rooms = value;
                OnPropertyChanged(nameof(Rooms));
            }
        }


        public ObservableCollection<TenantConfigurationViewModel> TenantConfigurations
        {
            get { return _flat.TenantConfigurations; }
            set
            {
                _flat.TenantConfigurations = value;
                OnPropertyChanged(nameof(TenantConfigurations));
            }
        }


        public ObservableCollection<TenantViewModel> Tenants
        {
            get { return _flat.Tenants; }
            set
            {
                _flat.Tenants = value;
                OnPropertyChanged(nameof(Tenants));
            }
        }

        #endregion collections


        // constructors
        #region constructors

        public FlatViewModel(Flat flat)
        {
            _flat = flat;
            CurrentRoomCosts = new ObservableCollection<RoomCostsViewModel>();

            Rooms = new ObservableCollection<RoomViewModel>();

            CreateRooms();

            TenantConfigurations = new ObservableCollection<TenantConfigurationViewModel>();
        }


        #endregion constructors


        // methods
        #region methods

        public void ActiveTenantListConversion()
        {
            foreach (TenantConfigurationViewModel item in TenantConfigurations)
            {
                item.GetActiveTenants();
            }
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


        private void CreateRooms()
        {
            Rooms.Clear();

            for (int i = 0; i < RoomCount; i++)
            {
                RoomViewModel room = new RoomViewModel(new Room($"room{i + 1}", 0));

                room.RoomAreaChanged += Room_RoomAreaChanged;
                room.PropertyChanged += Room_PropertyChanged;
                Rooms.Add(room);
            }

            OnPropertyChanged(nameof(RoomCount));
            OnPropertyChanged(nameof(Rooms));
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

            return currentRent.ColdRent.Cost;
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

        #endregion methods


        // events
        #region events

        private void Room_RoomAreaChanged(object? sender, EventArgs e)
        {
            //if (CombinedRoomArea > Area)
            //{
            //    MessageBox.Show("combined area of Rooms is larger than flat area");
            //}

            OnPropertyChanged(nameof(CombinedRoomArea));
            OnPropertyChanged(nameof(SharedArea));

        }


        private void Room_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RoomViewModel? room = sender as RoomViewModel;

            if (room != null)
            {
                //if (CombinedRoomArea > Area)
                //{
                //    MessageBox.Show("combined area of Rooms is larger than flat area");
                //}

                OnPropertyChanged(nameof(CombinedRoomArea));
                OnPropertyChanged(nameof(SharedArea));
            }
        }

        #endregion events


    }
}
// EOF