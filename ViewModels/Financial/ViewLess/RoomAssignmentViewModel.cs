/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomAssignementViewModel  : BaseViewModel
 * 
 *  viewmodel for RoomAssignement model
 */

using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;

namespace SharedLivingCostCalculator.ViewModels.Financial.ViewLess
{
    public class RoomAssignmentViewModel : BaseViewModel
    {

        // Properties & Fields
        #region Properties & Fields

        private TenantViewModel _AssignedTenant;
        public TenantViewModel AssignedTenant
        {
            get { return _AssignedTenant; }
            set
            {
                _AssignedTenant = value;
                OnPropertyChanged(nameof(AssignedTenant));

                AssignementChange?.Invoke(this, new EventArgs());
            }
        }


        public RoomViewModel RoomViewModel { get; }

        #endregion


        // Event Properties & Fields
        #region Event Properties & Fields

        public EventHandler AssignementChange;

        #endregion event properties & fields


        // Collections
        #region Collections

        public ObservableCollection<TenantViewModel> ActiveTenants { get; } = new ObservableCollection<TenantViewModel>();

        #endregion


        // Constructors
        #region Constructors

        public RoomAssignmentViewModel(RoomViewModel roomViewModel, ObservableCollection<TenantViewModel> tenantViewModels)
        {
            RoomViewModel = roomViewModel;

            foreach (TenantViewModel item in tenantViewModels)
            {
                if (!item.IsActive) { continue; }

                ActiveTenants.Add(item);
            }
        }

        #endregion


    }
}
// EOF