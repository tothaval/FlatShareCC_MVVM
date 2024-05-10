using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGMietkosten.Models
{
    internal class RoomSetup
    {
        // internally assigned id
        private readonly int _ID;
        public int ID { get { return _ID; } }

        // the name or location of the room, f.e. living room
        private string _RoomName;
        public string RoomName {
            get { return _RoomName; }
            set { _RoomName = value; }
        }

        // the room area
        private double _Area;
        public double Area {
            get { return _Area; }
            set { _Area = value; }
        }

        // the person living in the room
        private string _TenantName;
        public string TenantName
        {
            get { return _TenantName; }
            set { _TenantName = value; }
        }

        // constructor initializing the class and its readonly values.
        public RoomSetup(int id, string roomName, double area, string tenantName = "tenant")
        {
            _ID = id;
            _RoomName = roomName;
            _Area = area;
            _TenantName = tenantName;
        }

    }
}
