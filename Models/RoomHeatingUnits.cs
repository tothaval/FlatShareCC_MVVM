using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Models
{
    [Serializable]
    public class RoomHeatingUnits
    {
        [XmlIgnore]
        public RoomViewModel Room { get; set; }

        public int ID { get; set; }

        public double HeatingUnitsConsumption {  get; set; }


        public RoomHeatingUnits()
        {
        }


        public RoomHeatingUnits(RoomViewModel room)
        {
            Room = room;
     
            ID = room.ID;
        }
    }
}
