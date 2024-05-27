﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    class RoomCosts
    {
        private readonly Room _room;
        private readonly Costs _costs;


        public string RoomName => _room.RoomName;

        public double RoomArea => _room.RoomArea;

        public double ExtraCostsShare => CalculateExtraCosts();

        public double RentShare => CalculateRent();

        public double CombinedCostShare => RentShare + ExtraCostsShare;



        public RoomCosts(Room room, Costs costs)
        {
            _room = room;
            _costs = costs;
        }


        private double CalculateExtraCosts()
        {
            if (_costs != null && _room != null)
            {
                // Case: new flat, calculation based on area ratio
                if (_costs.Flat.BillingPeriods.Count == 0)
                {
                    double sharedAreaShare = _costs.SharedArea / _costs.Flat.RoomCount;

                    double rentedArea = _room.RoomArea + sharedAreaShare;

                    double extraCosts = rentedArea / _costs.Flat.Area * _costs.ExtraCosts;

                    return extraCosts;
                }

                // Case: annual billing received, calculation is based on area ratio
                // for shared area and on consumption ratio of heating units 



            }

            return -1.0;
        }

        private double CalculateRent()
        {
            if (_costs != null && _room != null)
            {
                double sharedAreaShare = _costs.SharedArea / _costs.Flat.RoomCount;

                double rentedArea = _room.RoomArea + sharedAreaShare;

                double rent = rentedArea / _costs.Flat.Area * _costs.Rent;

                return rent;
            }

            return -1.0;
        }
    }
}