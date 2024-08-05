/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Room 
 * 
 *  data model class
 *  for RoomViewModel
 */
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Models.Contract
{
    [Serializable]
    public class TenantConfiguration
    {

        // properties & fields
        #region properties & fields

        [XmlIgnore]
        private readonly FlatViewModel _FlatViewModel;


        public DateTime Start { get; set; } = DateTime.Now;

        #endregion properties & fields


        // collections
        #region collections

        [XmlArray]
        public ObservableCollection<RoomAssignment> RoomAssignments { get; set; } = new ObservableCollection<RoomAssignment>();


        [XmlIgnore]
        public ObservableCollection<RoomAssignementViewModel> RoomAssignmentsViewModels { get; set; } = new ObservableCollection<RoomAssignementViewModel>();


        [XmlIgnore]
        public ObservableCollection<TenantViewModel> Tenants { get; set; } = new ObservableCollection<TenantViewModel>();

        #endregion collections


        // constructors
        #region constructors

        public TenantConfiguration()
        {

        }


        public TenantConfiguration(ObservableCollection<TenantViewModel> tenants, FlatViewModel flatViewModel)
        {
            Tenants = tenants;
            _FlatViewModel = flatViewModel;

            foreach (RoomViewModel item in _FlatViewModel.Rooms)
            {
                RoomAssignmentsViewModels.Add(new RoomAssignementViewModel(item, tenants));
            }

        }

        #endregion constructors


        // methods
        #region methods

        public void BuildIDMaps()
        {
            RoomAssignments.Clear();

            foreach (RoomAssignementViewModel item in RoomAssignmentsViewModels)
            {
                if (item.AssignedTenant != null)
                {
                    RoomAssignments.Add(new RoomAssignment(
                        item.RoomViewModel.RoomName,
                        item.AssignedTenant.Name
                        ));
                }

                else
                {
                    RoomAssignments.Add(new RoomAssignment(
                        item.RoomViewModel.RoomName,
                        $"tenant_{new Random().Next(101)}"
                        ));
                }

            }
        }


        public void BuildRoomAssignmentViewModels(FlatViewModel flatViewModel)
        {
            RoomAssignmentsViewModels.Clear();

            foreach (RoomAssignment item in RoomAssignments)
            {
                foreach (RoomViewModel room in flatViewModel.Rooms)
                {
                    if (room.RoomName.Equals(item.RoomName))
                    {
                        RoomAssignmentsViewModels.Add(
                            new RoomAssignementViewModel(
                                room, flatViewModel.Tenants));
                    }
                }
            }

            foreach (RoomAssignementViewModel item in RoomAssignmentsViewModels)
            {
                foreach (RoomAssignment roomAssignment in RoomAssignments)
                {
                    if (roomAssignment.RoomName.Equals(item.RoomViewModel.RoomName))
                    {
                        foreach (TenantViewModel tenantViewModel in item.ActiveTenants)
                        {
                            if (tenantViewModel.Name.Equals(roomAssignment.TenantName))
                            {
                                item.AssignedTenant = tenantViewModel;
                            }
                        }
                    }
                }
            }
        }

        #endregion methods


    }
}
// EOF