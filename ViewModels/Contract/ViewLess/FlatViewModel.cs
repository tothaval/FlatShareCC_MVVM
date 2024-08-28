/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  _FlatViewModel  : BaseViewModel
 * 
 *  viewmodel for Flat model
 *  
 *  the most important data object
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace SharedLivingCostCalculator.ViewModels.Contract.ViewLess
{
    public class FlatViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        public string Address { get { return _Flat.Address; } set { _Flat.Address = value; OnPropertyChanged(nameof(Address)); } }


        public double Area
        {
            get { return _Flat.Area; }
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
                _Flat.Area = value;
                //}


                OnPropertyChanged(nameof(Area));
                OnPropertyChanged(nameof(SharedArea));
            }
        }


        public double CombinedRoomArea => CalculateCombinedRoomArea();


        public string Details { get { return _Flat.Details; } set { _Flat.Details = value; OnPropertyChanged(nameof(Details)); } }


        private Flat _Flat;
        public Flat Flat => _Flat;


        public string FlatNotes { get { return _Flat.FlatNotes; } set { _Flat.FlatNotes = value; OnPropertyChanged(nameof(FlatNotes)); } }


        public bool HasDataLock
        {
            get { return Flat.HasDataLock; }
            set
            {
                Flat.HasDataLock = value;

                OnPropertyChanged(nameof(HasDataLock));
            }
        }


        public bool InitialValuesFinalized
        {
            get { return Flat.InitialValuesFinalized; }
            set
            {
                Flat.InitialValuesFinalized = value;

                OnPropertyChanged(nameof(InitialValuesFinalized));
            }
        }


        private RentViewModel _InitialRent;
        public RentViewModel InitialRent
        {
            get { return _InitialRent; }
            set
            {
                _InitialRent = value;
                OnPropertyChanged(nameof(InitialRent));
            }
        }


        public int RoomCount
        {
            get { return _Flat.RoomCount; }

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
                _Flat.RoomCount = value;
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


        private bool _UseRoomCosts;
        public bool UseRoomCosts
        {
            get { return _UseRoomCosts; }
            set
            {
                _UseRoomCosts = value;

                InitialRent.UseRoomCosts4InitialRent(value);

                OnPropertyChanged(nameof(UseRoomCosts));
            }
        }

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event Action RoomCreation;

        #endregion event properties & fields


        // collections
        #region collections

        public ObservableCollection<BillingViewModel> AnnualBillings
        {
            get { return _Flat.AnnualBillings; }
            set
            {
                _Flat.AnnualBillings = value;
                OnPropertyChanged(nameof(AnnualBillings));
            }
        }


        public ObservableCollection<RentViewModel> RentUpdates
        {
            get { return _Flat.RentUpdates; }
            set
            {
                _Flat.RentUpdates = value;
                OnPropertyChanged(nameof(RentUpdates));
            }
        }


        public ObservableCollection<RoomViewModel> Rooms
        {
            get { return _Flat.Rooms; }
            set
            {
                _Flat.Rooms = value;
                OnPropertyChanged(nameof(Rooms));
            }
        }


        public ObservableCollection<TenantConfigurationViewModel> TenantConfigurations
        {
            get { return _Flat.TenantConfigurations; }
            set
            {
                _Flat.TenantConfigurations = value;
                OnPropertyChanged(nameof(TenantConfigurations));
            }
        }


        public ObservableCollection<TenantViewModel> Tenants
        {
            get { return _Flat.Tenants; }
            set
            {
                _Flat.Tenants = value;
                OnPropertyChanged(nameof(Tenants));
            }
        }

        #endregion collections



        // constructors
        #region constructors

        public FlatViewModel(Flat flat)
        {
            _Flat = flat;

            InitialRent = new RentViewModel(this, _Flat.InitialRent);

            Rooms = new ObservableCollection<RoomViewModel>();

            UseRoomCosts = true;
            InitialRent.Rent.UseRoomCosts4InitialRent = true;

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


        public void CalculateInitialRent()
        {

            double coldRent = 0.0;
            double advance = 0.0;

            foreach (RoomViewModel item in Rooms)
            {
                coldRent += item.InitialColdRent;
                advance += item.InitialAdvance;
            }

            InitialRent.ColdRent = coldRent;
            InitialRent.Advance = advance;
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


        public void ConnectRooms()
        {
            if (Rooms != null && Rooms.Count > 0)
            {
                foreach (RoomViewModel room in Rooms)
                {
                    room.RoomAreaChanged += Room_RoomAreaChanged;
                }
            }
        }


        private void CreateRooms()
        {
            Rooms.Clear();

            for (int i = 0; i < RoomCount; i++)
            {
                RoomViewModel room = new RoomViewModel(new Room($"room{i + 1}", 0), this);

                room.RoomAreaChanged += Room_RoomAreaChanged;
                room.PropertyChanged += Room_PropertyChanged;
                Rooms.Add(room);
            }

            OnPropertyChanged(nameof(RoomCount));
            OnPropertyChanged(nameof(Rooms));
        }

        public BillingViewModel? GetMostRecentBilling()
        {
            BillingViewModel? billingViewModel = null;
            if (AnnualBillings.Count > 0)
            {
                foreach (BillingViewModel billing in AnnualBillings)
                {
                    if (billingViewModel == null)
                    {
                        billingViewModel = billing;

                        continue;
                    }

                    if (billing.StartDate > billingViewModel.StartDate)
                    {
                        billingViewModel = billing;
                    }
                }
            }

            return billingViewModel;
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



        /// <summary>
        /// trigger this via ListView header click
        /// </summary>
        public void OrderAnnualBillingsAscending()
        {
            AnnualBillings = new ObservableCollection<BillingViewModel>(AnnualBillings.OrderBy(i => i.Year));

            OnPropertyChanged(nameof(AnnualBillings));
        }

        /// <summary>
        /// trigger this via ListView header click
        /// </summary>
        public void OrderRentUpdatesAscending()
        {
            RentUpdates = new ObservableCollection<RentViewModel>(RentUpdates.OrderBy(i => i.StartDate.Year));

            OnPropertyChanged(nameof(RentUpdates));
        }


        internal void UseRoomCosts4InitialRent(bool value)
        {
            InitialRent.Rent.SetUseRoomCosts(value);

            foreach (RoomViewModel item in Rooms)
            {
                item.InitialCostsAreRoomBased = InitialRent.Rent.UseRoomCosts4InitialRent;
            }
        }


        internal void UseRooms()
        {
            Flat.UseRooms = true;
            Flat.UseWorkspaces = false;
        }


        internal void UseWorkplaces()
        {
            Flat.UseRooms = false;
            Flat.UseWorkspaces = true;
        }

        #endregion methods


        // events
        #region events

        private void Room_RoomAreaChanged(object? sender, EventArgs e)
        {
            RoomViewModel? room = sender as RoomViewModel;

            if (room != null && UseRoomCosts)
            {


                OnPropertyChanged(nameof(CombinedRoomArea));
                OnPropertyChanged(nameof(SharedArea));
            }

            OnPropertyChanged(nameof(CombinedRoomArea));
            OnPropertyChanged(nameof(SharedArea));

        }


        private void Room_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RoomViewModel? room = sender as RoomViewModel;

            if (room != null)
            {
                OnPropertyChanged(nameof(CombinedRoomArea));
                OnPropertyChanged(nameof(SharedArea));
            }
        }

        #endregion events


    }
}
// EOF