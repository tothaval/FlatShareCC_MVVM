using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    internal class Costs
    {
        private readonly Flat _flat;
        public Flat Flat => _flat;

        public ObservableCollection<RoomCosts> RoomCosts => GetRoomCosts();

        public double ExtraCosts => CurrentExtraCosts();
        public double SharedExtraCosts => CalculateSharedExtraCosts();

        public double Rent => CurrentRent();
        public double SharedRent => CalculateSharedRent();

        public double SharedArea
        {
            get
            {
                return CalculateSharedArea();
            }
        }


        public Costs(Flat flat)
        {
            _flat = flat;
        }

        private double CalculateSharedArea()
        {
            if (_flat != null)
            {
                double shared_area = _flat.Area;

                foreach (Room room in _flat.Rooms)
                {
                    shared_area -= room.RoomArea;
                }

                return shared_area;
            }

            return -1.0;
        }
        private double CalculateSharedExtraCosts()
        {
            if (_flat != null)
            {
                double shared_area = CalculateSharedArea();

                double shared_rent = shared_area / _flat.Area * CurrentExtraCosts();

                return shared_rent;
            }

            return -1.0;
        }

        private double CalculateSharedRent()
        {
            if (_flat != null)
            {
                double shared_area = CalculateSharedArea();

                double shared_rent = shared_area / _flat.Area * CurrentRent();

                return shared_rent;
            }

            return -1.0;
        }
        private double CurrentExtraCosts()
        {

            if (_flat != null)
            {
                Rent currentRent = new Rent();

                foreach (Rent rent in _flat.RentUpdates)
                {
                    if (rent.StartDate > currentRent.StartDate)
                    {
                        currentRent = rent;
                    }
                }

                return currentRent.ExtraCostsTotal;
            }

            return -1.0;
        }

        private double CurrentRent()
        {

            if (_flat != null)
            {
                Rent currentRent = new Rent();

                foreach (Rent rent in _flat.RentUpdates)
                {
                    if (rent.StartDate > currentRent.StartDate)
                    {
                        currentRent = rent;
                    }
                }

                return currentRent.ColdRent;
            }

            return -1.0;
        }


        private ObservableCollection<RoomCosts> GetRoomCosts()
        {
            ObservableCollection<RoomCosts> roomCosts = new ObservableCollection<RoomCosts>();

            if (_flat != null)
            {

                foreach (Room room in _flat.Rooms)
                {
                    roomCosts.Add(new RoomCosts(room,this));
                }              
            }

            return roomCosts;
        }
    }
}
