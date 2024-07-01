/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Room 
 * 
 *  data model class
 *  for RoomViewModel
 */
using Microsoft.VisualBasic;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Models
{
    [Serializable]
    public class TenantsConfiguration
    {

        // properties & fields
        #region properties & fields
      
        [XmlIgnore]
        private readonly FlatViewModel _FlatViewModel;


        public DateTime Start { get; set; } = DateTime.Now;

        #endregion properties & fields


        // collections
        #region collections

        [XmlIgnore]
        public ObservableCollection<RoomAssignementViewModel> RoomAssignements { get; set; } = new ObservableCollection<RoomAssignementViewModel>();


        [XmlIgnore]
        public Dictionary<Room, TenantViewModel> RoomOccupancy { get; set; } = new Dictionary<Room, TenantViewModel>();


        [XmlArray]
        // build a function to fill this dictionary just right before saving
        // use it on load to fill RoomOccupancy after the rest of the data is initialized and present.
        public Dictionary<int, int> RoomOccupancyIDs => GetIDMaps();


        [XmlIgnore]
        public ObservableCollection<TenantViewModel> Tenants { get; set; } = new ObservableCollection<TenantViewModel>();
           
        #endregion collections


        // constructors
        #region constructors

        public TenantsConfiguration(ObservableCollection<TenantViewModel> tenants, FlatViewModel flatViewModel)
        {
            Tenants = tenants;
            _FlatViewModel = flatViewModel;

            foreach (RoomViewModel item in _FlatViewModel.Rooms)
            {
                RoomAssignements.Add(new RoomAssignementViewModel(item, tenants));
            }

        }

        #endregion constructors


        // methods
        #region methods

        public bool AssignTenantToRoom(TenantViewModel tenant, Room room)
        {
            if (RoomOccupancy.Keys.Contains(room))
            {
                return false;
            }

            if (RoomOccupancy.Values.Contains(tenant))
            {
                return false;
            }

            RoomOccupancy.Add(room, tenant);

            return true;

        }

        private Dictionary<int, int> GetIDMaps()
        {
            Dictionary<int, int> idMaps = new Dictionary<int, int>();

            if (RoomOccupancy.Count > 0)
            {
                foreach (KeyValuePair<Room, TenantViewModel> map in RoomOccupancy)
                {
                    idMaps.Add(map.Key.ID, map.Value.ID);
                }
            }

            return idMaps;
        }

        #endregion methods


    }
}
// EOF