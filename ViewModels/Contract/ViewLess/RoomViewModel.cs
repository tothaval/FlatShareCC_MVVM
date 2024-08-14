﻿/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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

        public RoomViewModel(Room room)
        {
            _Room = room;
        }

        #endregion constructors


    }
}
// EOF