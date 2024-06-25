/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomViewModel  : BaseViewModel
 * 
 *  viewmodel for Room model
 */
using SharedLivingCostCalculator.Models;
using System.Windows;


namespace SharedLivingCostCalculator.ViewModels.ViewLess
{
    public class RoomViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

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


        public int ID
        {
            get { return _room.ID; }
            set
            {
                _room.ID = value;
                OnPropertyChanged(nameof(ID));
            }
        }


        private Room _room;
        public Room GetRoom => _room;


        public double RoomArea
        {
            get { return _room.RoomArea; }
            set
            {
                if (_room.RoomArea > 0.0)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Warning: If you change the value all existing\n" +
                        "calculations will be effected.\n\n" +
                        "Proceed?",
                        "Change Room Area", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _room.RoomArea = value;
                    }
                }
                else
                {
                    _room.RoomArea = value;
                }

                OnPropertyChanged(nameof(RoomArea));

                RoomAreaChanged?.Invoke(this, EventArgs.Empty);
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

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler RoomAreaChanged;

        #endregion event properties & fields


        // constructors
        #region constructors

        public RoomViewModel(Room room)
        {
            _room = room;
        }

        #endregion constructors


    }
}
// EOF