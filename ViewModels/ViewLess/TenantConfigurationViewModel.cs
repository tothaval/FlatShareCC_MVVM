/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  TenantConfigurationViewModel  : BaseViewModel
 * 
 *  viewmodel for TenantsConfiguration model
 */

using SharedLivingCostCalculator.Models;
using System.Collections.ObjectModel;

namespace SharedLivingCostCalculator.ViewModels.ViewLess
{
    public class TenantConfigurationViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        public DateTime Start
        {
            get { return _TenantsConfiguration.Start; }
            set
            {
                _TenantsConfiguration.Start = value;
                OnPropertyChanged(nameof(Start));
            }
        }


        private TenantsConfiguration _TenantsConfiguration;
        public TenantsConfiguration GetTenantsConfiguration => _TenantsConfiguration;

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields



        #endregion event properties & fields


        // collections
        #region collections

        public ObservableCollection<RoomAssignementViewModel> RoomAssignements {
            get { return _TenantsConfiguration.RoomAssignements; } 
            set
            {
                _TenantsConfiguration.RoomAssignements = value;
                OnPropertyChanged(nameof(RoomAssignements));
            }
        }


        public Dictionary<Room, TenantViewModel> RoomOccupancy
        {
            get { return _TenantsConfiguration.RoomOccupancy; }

            set
            {
                _TenantsConfiguration.RoomOccupancy = value;
                OnPropertyChanged(nameof(RoomOccupancy));
            }
        }


        public Dictionary<int, int> RoomOccupancyIDs
        {
            get { return _TenantsConfiguration.RoomOccupancyIDs; }
        }

        #endregion collections


        // constructors
        #region constructors

        public TenantConfigurationViewModel(TenantsConfiguration tenantsConfiguration)
        {
            _TenantsConfiguration = tenantsConfiguration;

        }

        #endregion constructors


    }
}
// EOF