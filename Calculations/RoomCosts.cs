using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Calculations
{
    public class RoomCosts
    {
        private readonly RoomViewModel _room;
        private readonly Costs _costs;

        public RoomViewModel Room => _room;

        //public string RoomName => _room.RoomName;

        //private object _MyProperty;

        //public object MyProperty
        //{
        //    get { return _MyProperty; }
        //    set
        //    {
        //        _MyProperty = value;
        //        OnPropertyChanged(nameof(MyProperty));
        //    }
        //}

        //public double RoomArea => _room.RoomArea;

        public double ExtraCostsShare => CalculateExtraCosts();

        public double RentShare => CalculateRent();

        public double CombinedCostShare => RentShare + ExtraCostsShare;



        public RoomCosts(RoomViewModel room, Costs costs)
        {
            _room = room;
            _costs = costs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double CalculateExtraCosts()
        {
            double extraCosts = -1.0;

            if (_costs != null && _room != null)
            {
                double sharedAreaShare = _costs.SharedArea / _costs.FlatViewModel.RoomCount;

                double rentedArea = _room.RoomArea + sharedAreaShare;

                // Case: new flat, calculation based on area ratio
                if (_costs.FlatViewModel.BillingPeriods.Count == 0)
                {
                    extraCosts = rentedArea / _costs.FlatViewModel.Area * _costs.ExtraCosts;

                    return extraCosts;
                }

                // Case: annual billing received, calculation is based on area ratio
                // for shared area and on consumption ratio of heating units 

                //double consumedUnits =


                // wie eintragen, welche billing genutzt werden soll?
                // bzw. über eine methode erledigen, die eine billing entgegen nimmt.
                // ggf. costs verschieben oder auch als liste machen
                // wäre dann immer das rentupdate, welches ausschlaggebend ist,
                // pro cost object ein rent update,



            }

            return extraCosts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double CalculateRent()
        {
            if (_costs != null && _room != null)
            {
                double sharedAreaShare = _costs.SharedArea / _costs.FlatViewModel.RoomCount;

                double rentedArea = _room.RoomArea + sharedAreaShare;

                double rent = rentedArea / _costs.FlatViewModel.Area * _costs.Rent;

                return rent;
            }

            return -1.0;
        }
    }
}
