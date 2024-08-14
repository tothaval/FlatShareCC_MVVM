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

        // Properties & Fields
        #region Properties & Fields

        [XmlIgnore]
        private readonly FlatViewModel _FlatViewModel;


        public DateTime Start { get; set; } = DateTime.Now;

        #endregion


        // Collections
        #region Collections

        [XmlArray]
        public ObservableCollection<RoomAssignment> RoomAssignments { get; set; } = new ObservableCollection<RoomAssignment>();


        [XmlIgnore]
        public ObservableCollection<RoomAssignmentViewModel> RoomAssignmentsViewModels { get; set; } = new ObservableCollection<RoomAssignmentViewModel>();


        [XmlIgnore]
        public ObservableCollection<TenantViewModel> Tenants { get; set; } = new ObservableCollection<TenantViewModel>();

        #endregion


        // Constructors
        #region Constructors

        public TenantConfiguration()
        {

        }


        public TenantConfiguration(ObservableCollection<TenantViewModel> tenants, FlatViewModel flatViewModel)
        {
            Tenants = tenants;
            _FlatViewModel = flatViewModel;

            foreach (RoomViewModel item in _FlatViewModel.Rooms)
            {
                RoomAssignmentsViewModels.Add(new RoomAssignmentViewModel(item, tenants));
            }

        }

        #endregion


        // Methods
        #region Methods

        public void BuildIDMaps()
        {
            RoomAssignments.Clear();

            foreach (RoomAssignmentViewModel item in RoomAssignmentsViewModels)
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
                            new RoomAssignmentViewModel(
                                room, flatViewModel.Tenants));
                    }
                }
            }

            foreach (RoomAssignmentViewModel item in RoomAssignmentsViewModels)
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

        #endregion


    }
}
// EOF