/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  TenantConfigurationViewModel  : BaseViewModel
 * 
 *  viewmodel for TenantConfiguration model
 */

using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;

namespace SharedLivingCostCalculator.ViewModels.Contract.ViewLess
{
    public class TenantConfigurationViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        private ObservableCollection<string> _ActiveTenantsNames;
        public ObservableCollection<string> ActiveTenantsNames
        {
            get { return _ActiveTenantsNames; }
            set
            {
                _ActiveTenantsNames = value;
                OnPropertyChanged(nameof(ActiveTenantsNames));
            }
        }


        //private int _ActiveTenants;
        //public int ActiveTenants
        //{
        //    get { return _ActiveTenants; }
        //    set
        //    {
        //        _ActiveTenants = value;
        //        OnPropertyChanged(nameof(ActiveTenants));
        //    }
        //}


        public DateTime Start
        {
            get { return _TenantConfiguration.Start; }
            set
            {
                _TenantConfiguration.Start = value;
                OnPropertyChanged(nameof(Start));
            }
        }


        private TenantConfiguration _TenantConfiguration;
        public TenantConfiguration TenantConfiguration => _TenantConfiguration;

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields



        #endregion event properties & fields


        // collections
        #region collections

        public ObservableCollection<RoomAssignmentViewModel> RoomAssignements
        {
            get { return _TenantConfiguration.RoomAssignmentsViewModels; }
            set
            {
                _TenantConfiguration.RoomAssignmentsViewModels = value;
                GetActiveTenants();
                OnPropertyChanged(nameof(RoomAssignements));
            }
        }

        #endregion collections


        // constructors
        #region constructors

        public TenantConfigurationViewModel(TenantConfiguration tenantsConfiguration)
        {
            ActiveTenantsNames = new ObservableCollection<string>();
            _TenantConfiguration = tenantsConfiguration;

            GetActiveTenants();
        }

        #endregion constructors


        // methods
        #region methods

        public void BuildRoomAssignements()
        {
            _TenantConfiguration.BuildIDMaps();
        }


        public void BuildRoomAssignementViewModels(FlatViewModel flatViewModel)
        {
            _TenantConfiguration.BuildRoomAssignmentViewModels(flatViewModel);
        }


        //public void GetActiveTenants()
        //{
        //    if (RoomAssignements.Count > 0)
        //    {
        //        ActiveTenants = RoomAssignements[0].ActiveTenants.Count;

        //        return;
        //    }

        //    ActiveTenants = 0;
        //}

        public void GetActiveTenants()
        {
            ActiveTenantsNames.Clear();

            if (RoomAssignements.Count > 0)
            {
                foreach (TenantViewModel item in RoomAssignements[0].ActiveTenants)
                {
                    ActiveTenantsNames.Add(item.Name);

                }
            }

            OnPropertyChanged(nameof(ActiveTenantsNames));
        }

        #endregion methods


    }
}
// EOF