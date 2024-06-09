using SharedLivingCostCalculator.Calculations;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async void GetBillingViewModels(FlatViewModel flatViewModel)
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
                    TotalHeatingUnitsRoom = billingData.TotalHeatingUnitsRoom
                });

                billingViewModel.RoomCosts.Clear();

                foreach (RoomCosts roomCosts in billingData.RoomConsumptionValues)
                {
                    foreach (RoomViewModel room in flatViewModel.Rooms)
                    {
                        if (room.ID == roomCosts.RoomID)
                        {
                            RoomCostsViewModel roomCostsViewModel = new RoomCostsViewModel(room, billingViewModel);
                            billingViewModel.RoomCosts.Add(roomCostsViewModel);
                        }                        
                    }                    
                }

                billingViewModels.Add(billingViewModel);
            }

            flatViewModel.BillingPeriods = billingViewModels;

            await Task.Delay(5);
        }


        public async void GetRentViewModels(FlatViewModel flatViewModel)
        {
            ObservableCollection<RentViewModel> rentViewModels = new ObservableCollection<RentViewModel>();

            foreach (Rent rent in Rents)
            {
                RentViewModel rentViewModel = new RentViewModel(flatViewModel, rent);

                rentViewModel.GenerateRoomCosts();

                rentViewModels.Add(rentViewModel);
            }

            flatViewModel.RentUpdates = rentViewModels;

            await Task.Delay(5);
        }

        public async void GetRoomViewModels(FlatViewModel flatViewModel)
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

            flatViewModel.Rooms = roomViewModels;

            await Task.Delay(5);
        }


        public FlatViewModel GetFlatData()
        {
            FlatViewModel flatViewModel = new FlatViewModel(new Flat()
            {
                ID = ID,
                Area = Area,
                Address = Address,
                Details = Details,
                RoomCount = RoomCount,
            });


            GetRoomViewModels(flatViewModel);

            GetRentViewModels(flatViewModel);

            GetBillingViewModels(flatViewModel);

            Thread.Sleep(5);

            flatViewModel.Calculate();

            Thread.Sleep(5);

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

            BillingPeriods = GetBillings();
            Rents = GetRents();
            Rooms = GetRooms();
        }
    }
}
