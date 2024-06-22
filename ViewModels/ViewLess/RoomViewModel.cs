/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomViewModel  : BaseViewModel
 * 
 *  viewmodel for Room model
 */
using SharedLivingCostCalculator.Models;
using System.Collections.ObjectModel;


namespace SharedLivingCostCalculator.ViewModels.ViewLess
{
    public class RoomViewModel : BaseViewModel
    {

        public event EventHandler RoomAreaChanged;

        private Room _room;
        public Room GetRoom => _room;


        public int ID
        {
            get { return _room.ID; }
            set
            {
                _room.ID = value;
                OnPropertyChanged(nameof(ID));
            }
        }


        public string RoomName
        {
            get { return _room.RoomName; }
            set
            {
                _room.RoomName = value;
                OnPropertyChanged(nameof(RoomName));

            }
        }


        public double RoomArea
        {
            get { return _room.RoomArea; }
            set
            {
                _room.RoomArea = value;
                OnPropertyChanged(nameof(RoomArea));

                RoomAreaChanged?.Invoke(this, EventArgs.Empty);
            }
        }


        private double _CombinedPayments;
        public double CombinedPayments
        {
            get { return _CombinedPayments; }
            set
            {
                _CombinedPayments = value;
                OnPropertyChanged(nameof(CombinedPayments));
            }
        }


        private DateTime _StartDate;
        public DateTime StartDate
        {
            get { return _StartDate; }
            set
            {
                _StartDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }


        private DateTime _EndDate;
        public DateTime EndDate
        {
            get { return _EndDate; }
            set
            {
                _EndDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }


        public RoomViewModel(Room room)
        {
            _room = room;
        }


    }
}
// EOF