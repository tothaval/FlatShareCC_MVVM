/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomPayments 
 * 
 *  serializable data model class
 *  for RoomPaymentsViewModel
 */
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Models
{
    [Serializable]
    public class RoomPayments
    {

        // properties & fields
        #region properties

        public string RoomName { get; set; }


        [XmlIgnore]
        public RoomViewModel RoomViewModel { get; set; }

        #endregion properties


        // collections
        #region collections

        [XmlArray("PaymentCollection")]
        public ObservableCollection<Payment> Payments { get; set; } = new ObservableCollection<Payment>();

        #endregion collections


        // constructors
        #region constructors

        public RoomPayments() 
        {
        }


        public RoomPayments(RoomViewModel roomViewModel)
        {
            RoomViewModel = roomViewModel;
            RoomName = roomViewModel.RoomName;
        }
        
        #endregion constructors


    }
}
// EOF