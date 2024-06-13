﻿using SharedLivingCostCalculator.Calculations;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Utility
{
    [Serializable]
    [XmlRoot("Flat")]
    public class PersistanceDataSet
    {
        [XmlIgnore]
        private readonly FlatViewModel _flatViewModel;

        public int ID { get; set; }
        public string Address { get; set; }
        public string Details { get; set; }
        public double Area { get; set; }
        public int RoomCount { get; set; }
        public string FlatNotes { get; set; }

        [XmlArray("Rooms")]
        public ObservableCollection<RoomData> Rooms { get; set; }
        //{
        //    get { return GetRooms(); }
        //    set
        //    { Rooms = value; }
        //}

        [XmlArray("Billings")]
        public ObservableCollection<BillingData> BillingPeriods { get; set; }

        [XmlArray("Rents")]
        public ObservableCollection<Rent> Rents { get; set; }

        
        private ObservableCollection<BillingData> GetBillings()
        {
            ObservableCollection<BillingData> billings = new ObservableCollection<BillingData>();

            foreach (BillingViewModel bvm in _flatViewModel.BillingPeriods)
            {
                BillingData billingData = new BillingData();
                billingData.StartDate = bvm.StartDate;
                billingData.EndDate = bvm.EndDate;
                billingData.TotalCostsPerPeriod = bvm.TotalCostsPerPeriod;
                billingData.TotalFixedCostsPerPeriod = bvm.TotalFixedCostsPerPeriod;
                billingData.TotalHeatingCostsPerPeriod = bvm.TotalHeatingCostsPerPeriod;
                billingData.TotalHeatingUnitsConsumption = bvm.TotalHeatingUnitsConsumption;
                billingData.TotalHeatingUnitsRoom = bvm.TotalHeatingUnitsRoom;
                billingData.basedOnRent_ID = bvm.RentId;

                foreach (RoomCostsViewModel rhuvm in bvm.RoomCosts)
                {
                    billingData.RoomConsumptionValues.Add(rhuvm.GetRoomCosts);
                }

                billings.Add(billingData);
            }

            return billings;
        }


        private ObservableCollection<Rent> GetRents()
        {
            ObservableCollection<Rent> rents = new ObservableCollection<Rent>();

            foreach (RentViewModel rent in _flatViewModel.RentUpdates)
            {

                rents.Add(rent.GetRent);
            }

            return rents;
        }

        private ObservableCollection<RoomData> GetRooms()
        {
            ObservableCollection<RoomData> rooms = new ObservableCollection<RoomData>();

            foreach (RoomViewModel room in _flatViewModel.Rooms)
            {
                RoomData roomData = new RoomData();
                roomData.ID = room.ID;
                roomData.RoomName = room.RoomName;
                roomData.RoomArea = room.RoomArea;

                foreach (PaymentViewModel pvm in room.Payments)
                {
                    roomData.Payments.Add(pvm.GetPayment);
                }

                rooms.Add(roomData);
            }

            return rooms;
        }

        public async Task<ObservableCollection<BillingViewModel>> GetBillingViewModels(FlatViewModel flatViewModel)
        {
            ObservableCollection<BillingViewModel> billingViewModels = new ObservableCollection<BillingViewModel>();

            foreach (BillingData billingData in BillingPeriods)
            {
                BillingViewModel billingViewModel = new BillingViewModel(flatViewModel, new Billing()
                { 
                    StartDate = billingData.StartDate,
                    EndDate = billingData.EndDate,
                    TotalCostsPerPeriod = billingData.TotalCostsPerPeriod,
                    TotalFixedCostsPerPeriod = billingData.TotalFixedCostsPerPeriod,
                    TotalHeatingCostsPerPeriod = billingData.TotalHeatingCostsPerPeriod,
                    TotalHeatingUnitsConsumption = billingData.TotalHeatingUnitsConsumption,
                    TotalHeatingUnitsRoom = 0,
                    RentID = billingData.basedOnRent_ID
                });

                foreach (RoomCosts roomCosts in billingData.RoomConsumptionValues)
                {
                    foreach (RoomViewModel room in flatViewModel.Rooms)
                    {
                        if (room.ID == roomCosts.RoomID)
                        {
                            RoomCostsViewModel roomCostsViewModel = new RoomCostsViewModel(room, billingViewModel);

                            roomCostsViewModel.HeatingUnitsConsumption = roomCosts.HeatingUnitsConsumption;

                            billingViewModel.TotalHeatingUnitsRoom += roomCostsViewModel.HeatingUnitsConsumption;

                            billingViewModel.RoomCosts.Add(roomCostsViewModel);
                        }                        
                    }                    
                }

                billingViewModel.registerEvents();

                billingViewModels.Add(billingViewModel);
            }

            return billingViewModels;
        }


        public async Task<ObservableCollection<RentViewModel>> GetRentViewModels(FlatViewModel flatViewModel)
        {
            ObservableCollection<RentViewModel> rentViewModels = new ObservableCollection<RentViewModel>();

            foreach (Rent rent in Rents)
            {
                RentViewModel rentViewModel = new RentViewModel(flatViewModel, rent);

                rentViewModels.Add(rentViewModel);
            }

            return rentViewModels;
        }

        public async Task<ObservableCollection<RoomViewModel>> GetRoomViewModels(FlatViewModel flatViewModel)
        {
            ObservableCollection<RoomViewModel> roomViewModels = new ObservableCollection<RoomViewModel>();

            foreach (RoomData room in Rooms)
            {
                RoomViewModel roomViewModel = new RoomViewModel(new Room(room.ID, room.RoomName, room.RoomArea));

                foreach (Payment payment in room.Payments)
                {
                    roomViewModel.Payments.Add(new PaymentViewModel(payment));
                };                

                roomViewModels.Add(roomViewModel);
            }

            return roomViewModels;
        }


        public async Task<FlatViewModel> GetFlatData()
        {
            FlatViewModel flatViewModel = new FlatViewModel(new Flat()
            {
                ID = ID,
                Area = Area,
                Address = Address,
                Details = Details,
                RoomCount = RoomCount,
                FlatNotes = FlatNotes
            });

            Task<ObservableCollection<RoomViewModel>> GetRooms = GetRoomViewModels(flatViewModel);
            flatViewModel.Rooms = await GetRooms;

            Task<ObservableCollection<RentViewModel>> GetRents = GetRentViewModels(flatViewModel);
            flatViewModel.RentUpdates = await GetRents;

            Task<ObservableCollection<BillingViewModel>> GetBillings = GetBillingViewModels(flatViewModel);
            flatViewModel.BillingPeriods = await GetBillings;

            await flatViewModel.Calculate();

            return flatViewModel;
        }

        public PersistanceDataSet()
        {
            _flatViewModel = new FlatViewModel(new Flat());

            BillingPeriods = new ObservableCollection<BillingData>();
            Rents = new ObservableCollection<Rent>();
            Rooms = new ObservableCollection<RoomData>();
        }

        public PersistanceDataSet(FlatViewModel flatViewModel)
        {
            _flatViewModel = flatViewModel;

            ID = flatViewModel.ID;
            Address = flatViewModel.Address;
            Area = flatViewModel.Area;
            RoomCount = flatViewModel.RoomCount;
            Details = flatViewModel.Details;
            FlatNotes = flatViewModel.FlatNotes;

            BillingPeriods = GetBillings();
            Rents = GetRents();
            Rooms = GetRooms();
        }
    }
}
