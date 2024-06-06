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

        public Costs Costs => _flatViewModel.Costs;


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

                foreach (RoomHeatingUnitsViewModel rhuvm in bvm.RoomConsumptionValues)
                {
                    billingData.RoomConsumptionValues.Add(rhuvm.GetRoomHeatingUnits);
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

        public ObservableCollection<BillingViewModel> GetBillingViewModels(FlatViewModel flat)
        {
            ObservableCollection<BillingViewModel> billingViewModels = new ObservableCollection<BillingViewModel>();

            foreach (BillingData billingData in BillingPeriods)
            {
                BillingViewModel billingViewModel = new BillingViewModel(new Billing(flat)
                { 
                    StartDate = billingData.StartDate,
                    EndDate = billingData.EndDate,
                    TotalCostsPerPeriod = billingData.TotalCostsPerPeriod,
                    TotalFixedCostsPerPeriod = billingData.TotalFixedCostsPerPeriod,
                    TotalHeatingCostsPerPeriod = billingData.TotalHeatingCostsPerPeriod,
                    TotalHeatingUnitsConsumption = billingData.TotalHeatingUnitsConsumption,
                    TotalHeatingUnitsRoom = billingData.TotalHeatingUnitsRoom
                });

                ObservableCollection<RoomHeatingUnitsViewModel> rhuvm = new ObservableCollection<RoomHeatingUnitsViewModel>();

                foreach (RoomHeatingUnits rhu in billingData.RoomConsumptionValues)
                {
                    RoomHeatingUnitsViewModel roomHeatingUnitsViewModel = new RoomHeatingUnitsViewModel(rhu, billingViewModel);

                    foreach (RoomViewModel roomViewModel in flat.Rooms)
                    {
                        if (roomHeatingUnitsViewModel.GetRoomHeatingUnits.ID == roomViewModel.ID)
                        {
                            roomHeatingUnitsViewModel.GetRoomHeatingUnits.Room = roomViewModel;
                        }
                    }


                    

                    rhuvm.Add(roomHeatingUnitsViewModel);
                }

                billingViewModel.RoomConsumptionValues = rhuvm;

                billingViewModels.Add(billingViewModel);
            }

            return billingViewModels;
        }


        public ObservableCollection<RentViewModel> GetRentViewModels()
        {
            ObservableCollection<RentViewModel> rentViewModels = new ObservableCollection<RentViewModel>();

            foreach (Rent rent in Rents)
            {
                RentViewModel rentViewModel = new RentViewModel(rent);

                rentViewModels.Add(rentViewModel);
            }

            return rentViewModels;
        }

        public ObservableCollection<RoomViewModel> GetRoomViewModels()
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
            
            flatViewModel.Rooms = GetRoomViewModels();
            flatViewModel.RentUpdates = GetRentViewModels();
            flatViewModel.BillingPeriods = GetBillingViewModels(flatViewModel);

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
