/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Flat 
 * 
 *  serializable data model class
 *  for _FlatViewModel
 */

using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System.Collections.ObjectModel;
using System.Xml.Serialization;


namespace SharedLivingCostCalculator.Models.Contract
{

    [Serializable]
    public class Flat
    {

        // Properties & Fields
        #region Properties & Fields

        public string Address { get; set; }


        public double Area { get; set; }


        public string Details { get; set; }


        public string FlatNotes { get; set; } = "notes";


        public bool HasDataLock { get; set; } = false;


        public int RoomCount { get; set; }


        public bool InitialValuesFinalized { get; set; } = false;


        public Rent InitialRent { get; set; }


        public bool UseRooms { get; set; } = true;


        public bool UseWorkspaces { get; set; } = false;

        #endregion


        // Collections
        #region Collections

        [XmlIgnore]
        public ObservableCollection<BillingViewModel> AnnualBillings { get; set; }


        [XmlIgnore]
        public ObservableCollection<RentViewModel> RentUpdates { get; set; }


        [XmlIgnore]
        public ObservableCollection<RoomViewModel> Rooms { get; set; }


        [XmlIgnore]
        public ObservableCollection<TenantConfigurationViewModel> TenantConfigurations { get; set; }


        [XmlIgnore]
        public ObservableCollection<TenantViewModel> Tenants { get; set; }

        #endregion


        // Constructors
        #region Constructors

        public Flat()
        {
            Address = string.Empty;
            Details = string.Empty;
            Area = 0.0;
            RoomCount = 1;
            InitialRent = new Rent(true);
            AnnualBillings = new ObservableCollection<BillingViewModel>();
            Rooms = new ObservableCollection<RoomViewModel>();
            RentUpdates = new ObservableCollection<RentViewModel>();
            TenantConfigurations = new ObservableCollection<TenantConfigurationViewModel>();
            Tenants = new ObservableCollection<TenantViewModel>();
        }

        #endregion


    }
}
// EOF