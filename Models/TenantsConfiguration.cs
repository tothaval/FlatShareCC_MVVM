/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Room 
 * 
 *  data model class
 *  for RoomViewModel
 */
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    public class TenantsConfiguration
    {

        // properties & fields
        #region properties & fields

        private readonly FlatViewModel _FlatViewModel;


        public DateTime Start { get; set; } = DateTime.Now;


        public DateTime End { get; set; } = DateTime.Now;

        #endregion properties & fields


        // collections
        #region collections
        
        public ObservableCollection<Tenant> Tenants { get; set; } = new ObservableCollection<Tenant>();


        public Dictionary<RoomViewModel, Tenant> RoomOccupancy { get; set; } = new Dictionary<RoomViewModel, Tenant>();

        #endregion collections


        // constructors
        #region constructors

        public TenantsConfiguration(ObservableCollection<Tenant> tenants, FlatViewModel flatViewModel)
        {
            Tenants = tenants;
            _FlatViewModel = flatViewModel;                
        }

        #endregion constructors


        // methods
        #region methods
        public string AssignTenantToRoom(Tenant tenant, RoomViewModel roomViewModel)
        {
            if (RoomOccupancy.Keys.Contains(roomViewModel))
            {
                return "room already assigned";
            }

            if (RoomOccupancy.Values.Contains(tenant))
            {
                return "tenant already assigned";
            }

            RoomOccupancy.Add(roomViewModel, tenant);

            return "sucess";

        }

        #endregion methods


    }
}
// EOF