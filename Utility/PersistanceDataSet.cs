/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PersistanceDataSet 
 * 
 *  serializable helper class to
 *  store and retrieve FlatViewModel
 *  data to or from hard drive storage  
 */
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Utility
{

    [Serializable]
    [XmlRoot("Flat")]
    public class PersistanceDataSet
    {

        // properties & fields
        #region properties

        public string Address { get; set; }


        public double Area { get; set; }


        public string Details { get; set; }


        public string FlatNotes { get; set; }


        [XmlIgnore]
        private readonly FlatViewModel _flatViewModel;


        public int RoomCount { get; set; }

        #endregion properties


        // collections
        #region collections

        [XmlArray("Rents")]
        public ObservableCollection<Rent> Rents { get; set; }


        [XmlArray("Rooms")]
        public ObservableCollection<Room> Rooms { get; set; }


        [XmlArray("TenantConfigurations")]
        public ObservableCollection<TenantConfiguration> TenantConfigurations { get; set; }


        [XmlArray("Tenants")]
        public ObservableCollection<Tenant> Tenants { get; set; }

        #endregion collections


        // constructors
        #region constructors

        public PersistanceDataSet()
        {
            _flatViewModel = new FlatViewModel(new Flat());

            Rents = new ObservableCollection<Rent>();
            Rooms = new ObservableCollection<Room>();
            TenantConfigurations = new ObservableCollection<TenantConfiguration>();
            Tenants = new ObservableCollection<Tenant>();
        }

        public PersistanceDataSet(FlatViewModel flatViewModel)
        {
            _flatViewModel = flatViewModel;

            Address = flatViewModel.Address;
            Area = flatViewModel.Area;
            RoomCount = flatViewModel.RoomCount;
            Details = flatViewModel.Details;
            FlatNotes = flatViewModel.FlatNotes;

            Rents = GetRents();
            Rooms = GetRooms();
            TenantConfigurations = GetTenantConfigurations();
            Tenants = GetTenants();
        }

        #endregion constructors


        // async methods
        #region async methods

        public async Task<FlatViewModel> GetFlatData()
        {
            FlatViewModel flatViewModel = new FlatViewModel(new Flat()
            {
                Area = Area,
                Address = Address,
                Details = Details,
                RoomCount = RoomCount,
                FlatNotes = FlatNotes
            });

            Task<ObservableCollection<RoomViewModel>> GetRooms = GetRoomViewModels(flatViewModel);
            flatViewModel.Rooms = await GetRooms;

            Task<ObservableCollection<RentViewModel>> GetRents = GetRentViewModels(flatViewModel);
            flatViewModel.RentUpdates = await GetRents;

            Task<ObservableCollection<TenantViewModel>> GetTenants = GetTenantViewModels(flatViewModel);
            flatViewModel.Tenants = await GetTenants;

            Task<ObservableCollection<TenantConfigurationViewModel>> GetTenantConfigurations = GetTenantConfigurationViewModels(flatViewModel);
            flatViewModel.TenantConfigurations = await GetTenantConfigurations;

            return flatViewModel;
        }


        public async Task<ObservableCollection<RentViewModel>> GetRentViewModels(FlatViewModel flatViewModel)
        {
            ObservableCollection<RentViewModel> rentViewModels = new ObservableCollection<RentViewModel>();

            foreach (Rent rent in Rents)
            {
                rent.GenerateRoomCosts(flatViewModel);

                RentViewModel rentViewModel = new RentViewModel(flatViewModel, rent);

                if (rentViewModel.HasBilling && rentViewModel.BillingViewModel != null)
                {
                    if (rentViewModel.BillingViewModel.HasPayments)
                    {
                        foreach (RoomPayments roomPayments in rentViewModel.BillingViewModel.GetBilling.RoomPayments)
                        {
                            foreach (RoomViewModel roomViewModel in rentViewModel.GetFlatViewModel().Rooms)
                            {
                                if (roomPayments.RoomName != null)
                                {
                                    if (roomPayments.RoomName.Equals(roomViewModel.RoomName))
                                    {
                                        roomPayments.RoomViewModel = roomViewModel;
                                    } 
                                }
                            }
                        }
                    }

                }

                rentViewModels.Add(rentViewModel);
            }

            return rentViewModels;
        }


        public async Task<ObservableCollection<RoomViewModel>> GetRoomViewModels(FlatViewModel flatViewModel)
        {
            ObservableCollection<RoomViewModel> roomViewModels = new ObservableCollection<RoomViewModel>();

            foreach (Room room in Rooms)
            {
                RoomViewModel roomViewModel = new RoomViewModel(room);

                roomViewModels.Add(roomViewModel);
            }

            return roomViewModels;
        }


        public async Task<ObservableCollection<TenantConfigurationViewModel>> GetTenantConfigurationViewModels(FlatViewModel flatViewModel)
        {
            ObservableCollection<TenantConfigurationViewModel> tenantConfigurationViewModels = new ObservableCollection<TenantConfigurationViewModel>();

            foreach (TenantConfiguration tenantConfiguration in TenantConfigurations)
            {
                TenantConfigurationViewModel tenantConfigurationViewModel = new TenantConfigurationViewModel(tenantConfiguration);

                tenantConfigurationViewModel.BuildRoomAssignementViewModels(flatViewModel);

                tenantConfigurationViewModels.Add(tenantConfigurationViewModel);
            }

            return tenantConfigurationViewModels;
        }


        public async Task<ObservableCollection<TenantViewModel>> GetTenantViewModels(FlatViewModel flatViewModel)
        {
            ObservableCollection<TenantViewModel> tenantViewModels = new ObservableCollection<TenantViewModel>();

            foreach (Tenant tenant in Tenants)
            {
                TenantViewModel tenantViewModel = new TenantViewModel(tenant);

                tenantViewModels.Add(tenantViewModel);
            }

            return tenantViewModels;
        }

        #endregion async methods


        // methods
        #region methods

        private ObservableCollection<Rent> GetRents()
        {
            ObservableCollection<Rent> rents = new ObservableCollection<Rent>();

            foreach (RentViewModel rentViewModel in _flatViewModel.RentUpdates)
            {
                Rent rent = rentViewModel.Rent;

                if (rentViewModel.BillingViewModel != null)
                {
                    rent.GetBilling = rentViewModel.BillingViewModel.GetBilling;
                }

                //if (rent.HasOtherCosts)
                //{
                //    rent.Costs.Clear();
                //    foreach (CostItemViewModel otherCostItemViewModel in rentViewModel.Costs)
                //    {
                //        rent.Costs.Add(otherCostItemViewModel.FinancialTransactionItem);
                //    }
                //}


                rents.Add(rent);
            }

            return rents;
        }


        private ObservableCollection<Room> GetRooms()
        {
            ObservableCollection<Room> rooms = new ObservableCollection<Room>();

            foreach (RoomViewModel roomViewModel in _flatViewModel.Rooms)
            {
                rooms.Add(roomViewModel.GetRoom);
            }

            return rooms;
        }


        private ObservableCollection<TenantConfiguration> GetTenantConfigurations()
        {
            ObservableCollection<TenantConfiguration> tenantConfigurations = new ObservableCollection<TenantConfiguration>();

            foreach (TenantConfigurationViewModel tenantConfigurationViewModel in _flatViewModel.TenantConfigurations)
            {
                tenantConfigurationViewModel.BuildRoomAssignements();

                tenantConfigurations.Add(tenantConfigurationViewModel.GetTenantsConfiguration);                
            }

            return tenantConfigurations;
        }


        private ObservableCollection<Tenant> GetTenants()
        {
            ObservableCollection<Tenant> tenants = new ObservableCollection<Tenant>();

            foreach (TenantViewModel tenantViewModel in _flatViewModel.Tenants)
            {
                tenants.Add(tenantViewModel.GetTenant);
            }

            return tenants;
        }

        #endregion methods


    }
}
// EOF