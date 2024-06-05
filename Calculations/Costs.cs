using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Calculations
{
    public class Costs
    {
        private readonly FlatViewModel _flatViewModel;
        public FlatViewModel FlatViewModel => _flatViewModel;

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


        public Costs(FlatViewModel flatViewModel)
        {
            _flatViewModel = flatViewModel;
        }

        private double CalculateSharedArea()
        {
            if (_flatViewModel != null)
            {
                double shared_area = _flatViewModel.Area;

                foreach (Room room in _flatViewModel.Rooms)
                {
                    shared_area -= room.RoomArea;
                }

                return shared_area;
            }

            return -1.0;
        }
        private double CalculateSharedExtraCosts()
        {
            if (_flatViewModel != null)
            {
                double shared_area = CalculateSharedArea();

                double shared_rent = shared_area / _flatViewModel.Area * CurrentExtraCosts();

                return shared_rent;
            }

            return -1.0;
        }

        private double CalculateSharedRent()
        {
            if (_flatViewModel != null)
            {
                double shared_area = CalculateSharedArea();

                double shared_rent = shared_area / _flatViewModel.Area * CurrentRent();

                return shared_rent;
            }

            return -1.0;
        }
        private double CurrentExtraCosts()
        {

            if (_flatViewModel != null)
            {
                RentViewModel currentRent = new RentViewModel(new Models.Rent());
                currentRent.StartDate = new DateTime(1, 1, 1);

                foreach (RentViewModel rent in _flatViewModel.RentUpdates)
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

            if (_flatViewModel != null)
            {
                RentViewModel currentRent = new RentViewModel(new Models.Rent());

                foreach (RentViewModel rent in _flatViewModel.RentUpdates)
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

            if (_flatViewModel != null)
            {

                foreach (Room room in _flatViewModel.Rooms)
                {
                    roomCosts.Add(new RoomCosts(room, this));
                }
            }

            return roomCosts;
        }
    }
}
