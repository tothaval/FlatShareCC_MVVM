using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGMietkosten.Models
{
    internal class FlatSetup
    {
        // internally assigned id
        private readonly int _ID;
        public int ID { get { return _ID; } }

        // address of the flat
        private readonly string _Address;
        public string Address { get { return _Address; } }

        // name, description or location of the flat, f.e. 1st floor, 2nd left
        private readonly string _FlatName;
        public string FlatName { get { return _FlatName; } }

        // area of the flat
        private readonly double _Area;
        public double Area { get { return _Area; } }

        // room count of the flat
        private readonly int _RoomCount;
        public int RoomCount { get { return _RoomCount; } }

        // the tenants of the flat // maybe replace this later with a contract object with time span and so on
        private string _TenantNames;
        public string Tenants {
            get { return _TenantNames; } 
            set { _TenantNames = value; }
        }

        private ObservableCollection<RoomSetup> _rooms;
        public ObservableCollection<RoomSetup> Rooms {
            get {  return _rooms; } 
            set { _rooms = value; }
        }

        /// <summary>
        /// initialize a FlatSetup object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <param name="area"></param>
        /// <param name="roomCount"></param>
        /// <param name="tenantNames"></param>
        public FlatSetup(int id, string address, string flatName, double area, int roomCount, string tenantNames = "tenants")
        {
            _ID = id;
            _Address = address;
            _FlatName = flatName;
            _Area = area;
            _RoomCount = roomCount;
            _TenantNames = tenantNames;

            _rooms = new ObservableCollection<RoomSetup>();
        }

        /// <summary>
        /// add a room to _Rooms observable collection, if _RoomCount is higher then Rooms.Count
        /// and if combined area of rooms in _Rooms plus roomSetup.Area is below _Area of flat.
        /// </summary>
        /// <param name="roomSetup"></param>
        /// <returns></returns>
        public bool AddRoom(RoomSetup roomSetup)
        {
            if(Rooms != null)
            {
                if ((GetCombinedRoomArea() + roomSetup.Area) < Area && (Rooms.Count < RoomCount))
                {
                    Rooms.Add(roomSetup);

                    return true;
                }
            }

            return false;            
        }

        /// <summary>
        /// returns how much area the rooms in _Rooms occupy
        /// </summary>
        /// <returns></returns>
        private double GetCombinedRoomArea()
        {
            double area = 0;

            if (Rooms != null)
            {
                foreach (RoomSetup item in Rooms)
                {
                    area += item.Area;
                }
            }
            
            return area;
        }

        internal bool Conflicts(FlatSetup flatSetup)
        {
            return flatSetup.Address == Address &&
                flatSetup.Area == Area &&
                flatSetup.RoomCount == RoomCount &&
                flatSetup.FlatName == FlatName;
        }
    }
}
