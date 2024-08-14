/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomPayments 
 * 
 *  serializable data model class
 *  for RoomPaymentsViewModel
 */
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Models.Financial
{
    [Serializable]
    public class RoomPayments
    {

        // Properties & Fields
        #region Properties & Fields

        public string RoomName { get; set; }


        [XmlIgnore]
        public RoomViewModel RoomViewModel { get; set; }

        #endregion


        // Collections
        #region Collections

        [XmlArray("PaymentCollection")]
        public ObservableCollection<Payment> Payments { get; set; } = new ObservableCollection<Payment>();

        #endregion


        // Constructors
        #region Constructors

        public RoomPayments()
        {
        }


        public RoomPayments(RoomViewModel roomViewModel)
        {
            RoomViewModel = roomViewModel;
            RoomName = roomViewModel.RoomName;
        }

        #endregion


    }
}
// EOF