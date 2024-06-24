/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PersistanceDataSet 
 * 
 *  serializable helper class to
 *  store and retrieve FlatViewModel
 *  data to or from hard drive storage  
 */
using Microsoft.VisualBasic;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Room> Rooms { get; set; }


        [XmlArray("Rents")]
        public ObservableCollection<Rent> Rents { get; set; }


        private ObservableCollection<Rent> GetRents()
        {
            ObservableCollection<Rent> rents = new ObservableCollection<Rent>();

            foreach (RentViewModel rentViewModel in _flatViewModel.RentUpdates)
            {
                Rent rent = rentViewModel.Rent;

                if (rentViewModel.BillingViewModel != null)
                {
                    rent.GetBilling = rentViewModel.BillingViewModel.GetBilling;
                }

                //if (rent.HasOtherCosts)
                //{
                //    rent.OtherCosts.Clear();
                //    foreach (OtherCostItemViewModel otherCostItemViewModel in rentViewModel.OtherCosts)
                //    {
                //        rent.OtherCosts.Add(otherCostItemViewModel.OtherCostItem);
                //    }
                //}


                rents.Add(rent);
            }

            return rents;
        }


        private ObservableCollection<Room> GetRooms()
        {
            ObservableCollection<Room> rooms = new ObservableCollection<Room>();

            foreach (RoomViewModel roomViewModel in _flatViewModel.Rooms)
            {
                rooms.Add(roomViewModel.GetRoom);
            }

            return rooms;
        }

    
        public async Task<ObservableCollection<RentViewModel>> GetRentViewModels(FlatViewModel flatViewModel)
        {
            ObservableCollection<RentViewModel> rentViewModels = new ObservableCollection<RentViewModel>();

            foreach (Rent rent in Rents)
            {

                rent.GenerateRoomCosts(flatViewModel);


                RentViewModel rentViewModel = new RentViewModel(flatViewModel, rent);

                if (rentViewModel.HasBilling && rentViewModel.BillingViewModel != null)
                {
                    if (rentViewModel.BillingViewModel.HasPayments)
                    {
                        foreach (RoomPayments roomPayments in rentViewModel.BillingViewModel.GetBilling.RoomPayments)
                        {
                            foreach (RoomViewModel roomViewModel in rentViewModel.GetFlatViewModel().Rooms)
                            {
                                if (roomPayments.RoomID == roomViewModel.ID)
                                {
                                    roomPayments.RoomViewModel = roomViewModel;
                                }
                            }
                        }
                    }

                }



                rentViewModels.Add(rentViewModel);
            }

            return rentViewModels;
        }


        public async Task<ObservableCollection<RoomViewModel>> GetRoomViewModels(FlatViewModel flatViewModel)
        {
            ObservableCollection<RoomViewModel> roomViewModels = new ObservableCollection<RoomViewModel>();

            foreach (Room room in Rooms)
            {
                RoomViewModel roomViewModel = new RoomViewModel(room);
            
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

            return flatViewModel;
        }


        public PersistanceDataSet()
        {
            _flatViewModel = new FlatViewModel(new Flat());

            Rents = new ObservableCollection<Rent>();
            Rooms = new ObservableCollection<Room>();
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

            Rents = GetRents();
            Rooms = GetRooms();
        }


    }
}
// EOF