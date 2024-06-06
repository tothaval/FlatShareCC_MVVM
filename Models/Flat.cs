using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Models
{
    [Serializable]
    public class Flat
    {
        public int ID { get; set; }
        public string Address { get; set; }
        public string Details { get; set; }
        public double Area { get; set; }
        public int RoomCount { get; set; }

        [XmlIgnore]
        public ObservableCollection<RoomViewModel> Rooms { get; set; }

        [XmlIgnore]
        public ObservableCollection<BillingViewModel> BillingPeriods { get; set; }

        [XmlIgnore]
        public ObservableCollection<RentViewModel> RentUpdates { get; set; }

        public Flat()
        {
            ID = -1;
            Address = string.Empty;
            Details = string.Empty;
            Area = 0.0;
            RoomCount = 0;
            Rooms = new ObservableCollection<RoomViewModel>();
            BillingPeriods = new ObservableCollection<BillingViewModel>();
            RentUpdates = new ObservableCollection<RentViewModel>();
        }

        public Flat(int id, string address, double area, int roomCount, string details = "")
        {
            ID = id;
            Address = address;
            Area = area;
            RoomCount = roomCount;

            Details = details;           

            Rooms = new ObservableCollection<RoomViewModel> { };

            for (int i = 0; i < roomCount; i++)
            {
                Rooms.Add(new RoomViewModel(new Room(i)));
            }

            BillingPeriods = new ObservableCollection<BillingViewModel>();
            RentUpdates = new ObservableCollection<RentViewModel>();
        }

        public Flat(int iD, string address, double area, int roomCount, ObservableCollection<RoomViewModel> rooms, string details = "") : this(iD, address, area, roomCount)
        {
            Rooms = rooms;
            Details += details;

            BillingPeriods = new ObservableCollection<BillingViewModel>();
            RentUpdates = new ObservableCollection<RentViewModel>();
        }
    }
}
