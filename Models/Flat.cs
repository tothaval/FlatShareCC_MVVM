using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    internal class Flat
    {
        public int ID { get; set; }
        public string Address { get; set; }
        public string Details { get; set; }
        public double Area { get; set; }
        public int RoomCount { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }
        public ObservableCollection<BillingPeriod> BillingPeriods { get; set; }
        public ObservableCollection<Rent> RentUpdates { get; set; }

        public Flat()
        {
            ID = -1;
            Address = string.Empty;
            Details = string.Empty;
            Area = 0.0;
            RoomCount = 0;
            Rooms = new ObservableCollection<Room>();
            BillingPeriods = new ObservableCollection<BillingPeriod>();
            RentUpdates = new ObservableCollection<Rent>();
        }

        public Flat(int id, string address, double area, int roomCount, string details = "")
        {
            ID = id;
            Address = address;
            Area = area;
            RoomCount = roomCount;

            Details = details;           

            Rooms = new ObservableCollection<Room> { };

            for (int i = 0; i < roomCount; i++)
            {
                Rooms.Add(new Room(i));
            }

            BillingPeriods = new ObservableCollection<BillingPeriod>();
            RentUpdates = new ObservableCollection<Rent>();
        }

        public Flat(int iD, string address, double area, int roomCount, ObservableCollection<Room> rooms, string details = "") : this(iD, address, area, roomCount)
        {
            Rooms = rooms;
            Details += details;

            BillingPeriods = new ObservableCollection<BillingPeriod>();
            RentUpdates = new ObservableCollection<Rent>();
        }
    }
}
