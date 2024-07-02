using SharedLivingCostCalculator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.ViewModels.ViewLess
{
    public class RoomAssignementViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields
        
        public RoomViewModel RoomViewModel { get; }


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

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public EventHandler AssignementChange;

        #endregion event properties & fields


        // collections
        #region collections

        public ObservableCollection<TenantViewModel> ActiveTenants { get; } = new ObservableCollection<TenantViewModel>();

        #endregion collections


        // constructors
        #region constructors
        public RoomAssignementViewModel(RoomViewModel roomViewModel, ObservableCollection<TenantViewModel> tenantViewModels)
        {
            RoomViewModel = roomViewModel;
                                    
            foreach (TenantViewModel item in tenantViewModels)
            {
                if (!item.IsActive) { continue; }

                ActiveTenants.Add(item);
            }
        }
        #endregion constructors


    }
}
