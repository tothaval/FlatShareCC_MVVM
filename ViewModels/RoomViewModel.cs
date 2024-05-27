using SharedLivingCostCalculator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class RoomViewModel : BaseViewModel
    {
        private Room _room;

        public int ID => _room.ID;
        public string RoomName => _room.RoomName;
        public double RoomArea => _room.RoomArea;
        public double TotalPayments => _room.TotalPayments;

        public ObservableCollection<Payment> Payments => _room.Payments;

        public RoomViewModel(Room room)
        {
            _room = room;
        }

        public void CalculatePayments()
        {
            _room.CalculateTotalPayments();
            OnPropertyChanged(nameof(TotalPayments));
        }

    }
}
