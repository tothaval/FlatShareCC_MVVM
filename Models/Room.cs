using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    internal class Room : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string RoomName { get; set; }

        private double roomArea;

        public double RoomArea { get { return roomArea; } set { roomArea = value; OnPropertyChanged(nameof(RoomArea)); } }

        public Room()
        {
            ID = -1;
            RoomName = string.Empty;
            RoomArea = 0;                
        }

        public Room(int id) { ID = id; }

        public Room(int iD, string roomName, double roomArea)
        {
            ID = iD;
            RoomName = roomName;
            RoomArea = roomArea;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
