/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomViewModel  : BaseViewModel
 * 
 *  viewmodel for Room model
 */
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Windows;


namespace SharedLivingCostCalculator.ViewModels.Contract.ViewLess
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


        private readonly FlatViewModel _FlatViewModel;


        public double InitialAdvance
        {
            get { return _Room.InitialAdvance; }
            set
            {
                _Room.InitialAdvance = value;
                     
                OnPropertyChanged(nameof(InitialAdvance));

                _FlatViewModel.CalculateInitialRent();
            }
        }

        public double InitialColdRent
        {
            get { return _Room.InitialColdRent; }
            set
            {
                _Room.InitialColdRent = value;
                          
                OnPropertyChanged(nameof(InitialColdRent));

                _FlatViewModel.CalculateInitialRent();
            }
        }

        public bool InitialCostsAreRoomBased
        {
            get { return _Room.InitialCostsAreRoomBased; }
            set
            {
                _Room.InitialCostsAreRoomBased = value;
                OnPropertyChanged(nameof(InitialCostsAreRoomBased));
            }
        }


        private Room _Room;
        public Room Room => _Room;


        public double RoomArea
        {
            get { return _Room.RoomArea; }
            set
            {
                //if (_room.RoomArea > 0.0)
                //{
                //    MessageBoxResult result = MessageBox.Show(
                //        "Warning: If you change the value all existing\n" +
                //        "calculations will be effected.\n\n" +
                //        "Proceed?",
                //        "Change Room Area", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //    if (result == MessageBoxResult.Yes)
                //    {
                //        _room.RoomArea = value;
                //    }
                //}
                //else
                //{
                _Room.RoomArea = value;
                //}

                OnPropertyChanged(nameof(RoomArea));

                RoomAreaChanged?.Invoke(this, EventArgs.Empty);
            }
        }


        public string RoomName
        {
            get { return _Room.RoomName; }
            set
            {
                _Room.RoomName = value;
                OnPropertyChanged(nameof(RoomName));

                RoomAreaChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler RoomAreaChanged;

        #endregion event properties & fields


        // constructors
        #region constructors

        public RoomViewModel(Room room, FlatViewModel flatViewModel)
        {
            _Room = room;
            _FlatViewModel = flatViewModel;
        }

        #endregion constructors


    }
}
// EOF