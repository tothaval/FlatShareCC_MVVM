using SharedLivingCostCalculator.Calculations;
using SharedLivingCostCalculator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SharedLivingCostCalculator.ViewModels
{
    public class FlatViewModel : BaseViewModel
    {
        private Flat _flat;
        public Flat GetFlat => _flat;

        public int ID => _flat.ID;
        public string Address { get { return _flat.Address; } set { _flat.Address = value; } }
        public string Details { get { return _flat.Details; } set { _flat.Details = value; } }
        public double Area { get { return _flat.Area; } set { _flat.Area = value; } }
        public int RoomCount { get { return _flat.RoomCount; } set { _flat.RoomCount = value; CreateRooms(); } }

        public ObservableCollection<RoomCostsViewModel> CurrentRoomCosts { get; set; }

        public ObservableCollection<RoomViewModel> Rooms
        {
            get { return _flat.Rooms; }
            set
            {
                _flat.Rooms = value;
                OnPropertyChanged(nameof(Rooms));
            }
        }

        public ObservableCollection<BillingViewModel> BillingPeriods
        {
            get { return _flat.BillingPeriods; }
            set
            {
                _flat.BillingPeriods = value;
                OnPropertyChanged(nameof(BillingPeriods));
            }
        }

        public ObservableCollection<RentViewModel> RentUpdates
        {
            get { return _flat.RentUpdates; }
            set
            {
                _flat.RentUpdates = value;
                OnPropertyChanged(nameof(RentUpdates));
            }
        }


        public double ExtraCosts => CurrentExtraCosts();
        public double SharedExtraCosts => CalculateSharedExtraCosts();

        public double Rent => CurrentRent();
        public double SharedRent => CalculateSharedRent();
        public double SharedArea => CalculateSharedArea();



        public FlatViewModel(Flat flat)
        {
            _flat = flat;
            CurrentRoomCosts = new ObservableCollection<RoomCostsViewModel>();

            ConnectRooms();
        }

        public void Calculate()
        {
            foreach (BillingViewModel billingViewModel in BillingPeriods)
            {
                billingViewModel.GenerateRoomCosts();
            }

            foreach (RentViewModel rentViewModel in RentUpdates)
            {
                rentViewModel.GenerateRoomCosts();
            }
        }


        public void SetMostRecentCosts()
        {
            ObservableCollection<RoomCostsViewModel> roomCosts = new ObservableCollection<RoomCostsViewModel>();

            BillingViewModel? billing = GetMostRecentBillingPeriod();

            if (billing != null)
            {
                roomCosts = billing.RoomCosts;
            }
            else
            {
                RentViewModel? rent = GetMostRecentRent();

                if (rent != null)
                {
                    if (rent.RoomCosts.Count == 0)
                    {
                        foreach (RoomViewModel room in Rooms)
                        {
                            rent.RoomCosts.Add(new RoomCostsViewModel(room, rent));
                        }
                    }
                    else
                    {
                        roomCosts = rent.RoomCosts;
                    }
                }
            }

            CurrentRoomCosts = roomCosts;

            OnPropertyChanged(nameof(CurrentRoomCosts));
        }

        public ObservableCollection<RoomCostsViewModel> GetMostRecentCosts()
        {
            ObservableCollection<RoomCostsViewModel> roomCosts = new ObservableCollection<RoomCostsViewModel>();

            BillingViewModel? billing = GetMostRecentBillingPeriod();

            if (billing != null)
            {
                roomCosts = billing.RoomCosts;
            }
            else
            {
                RentViewModel? rent = GetMostRecentRent();

                if (rent != null)
                {
                    roomCosts = rent.RoomCosts;
                }
            }

            return roomCosts;
        }

        private BillingViewModel? GetMostRecentBillingPeriod()
        {
            BillingViewModel? billingViewModel = null;
            if (BillingPeriods.Count > 0)
            {
                foreach (BillingViewModel billing in BillingPeriods)
                {
                    if (billingViewModel == null)
                    {
                        if (billing.StartDate <= DateTime.Now)
                        {
                            billingViewModel = billing;
                        }

                        continue;
                    }

                    if (billing.StartDate > billingViewModel.StartDate)
                    {
                        billingViewModel = billing;
                    }
                }
            }
            return billingViewModel;
        }


        private RentViewModel? GetMostRecentRent()
        {
            RentViewModel? rentViewModel = null;
            if (RentUpdates.Count > 0)
            {
                foreach (RentViewModel rent in RentUpdates)
                {
                    if (rentViewModel == null)
                    {
                        rentViewModel = rent;

                        continue;
                    }

                    if (rent.StartDate > rentViewModel.StartDate)
                    {
                        rentViewModel = rent;
                    }
                }
            }

            return rentViewModel;
        }

        public double GetPaymentsPerPeriod(BillingViewModel billingViewModel)
        {
            double paymentsPerPeriod = 0;

            foreach (RoomViewModel room in Rooms)
            {
                foreach (PaymentViewModel payment in room.Payments)
                {
                    if (
                        payment.StartDate >= billingViewModel.StartDate && payment.StartDate <= billingViewModel.EndDate
                        && payment.EndDate >= billingViewModel.StartDate && payment.EndDate <= billingViewModel.EndDate
                        )
                    {
                        paymentsPerPeriod += payment.PaymentTotal;
                    }
                }
            }

            return paymentsPerPeriod;
        }

        public RentViewModel? GetRentForPeriod(BillingViewModel billingViewModel)
        {
            RentViewModel? rentViewModel = null;

            if (RentUpdates.Count > 0)
            {
                foreach (RentViewModel rent in RentUpdates)
                {
                    if (rentViewModel == null)
                    {
                        if (rent.StartDate <= billingViewModel.StartDate && rent.StartDate < billingViewModel.EndDate)
                        {
                            rentViewModel = rent;
                        }

                        continue;
                    }

                    if (rent.StartDate >= billingViewModel.EndDate || rent.StartDate >= billingViewModel.StartDate)
                    {
                        continue;
                    }

                    if (rent.StartDate > rentViewModel.StartDate)
                    {
                        rentViewModel = rent;
                    }
                }
            }

            return rentViewModel;
        }

        private void ConnectRooms()
        {
            if (Rooms != null && Rooms.Count > 0)
            {
                foreach (RoomViewModel room in Rooms)
                {
                    room.PropertyChanged += Room_PropertyChanged;
                }
            }
        }


        private void CreateRooms()
        {
            Rooms.Clear();

            for (int i = 0; i < RoomCount; i++)
            {
                RoomViewModel room = new RoomViewModel(new Room(i, $"room{i + 1}", 0));

                room.PropertyChanged += Room_PropertyChanged;
                Rooms.Add(room);
            }
        }

        private void Room_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RoomViewModel? room = sender as RoomViewModel;

            if (room != null)
            {
                double area = _flat.Area;

                foreach (RoomViewModel item in _flat.Rooms)
                {
                    area -= item.RoomArea;
                }

                if (area < 0)
                {
                    room.RoomArea = 0;
                    MessageBox.Show("combined area of Rooms is larger than flat area");
                }
            }
        }

        public event Action RoomCreation;




        private double CalculateSharedArea()
        {
            double shared_area = Area;

            foreach (RoomViewModel room in Rooms)
            {
                shared_area -= room.RoomArea;
            }

            return shared_area;

        }

        private double CalculateSharedExtraCosts()
        {
            double shared_area = CalculateSharedArea();

            double shared_rent = shared_area / Area * CurrentExtraCosts();

            return shared_rent;

        }

        private double CalculateSharedRent()
        {
            double shared_area = CalculateSharedArea();

            double shared_rent = shared_area / Area * CurrentRent();

            return shared_rent;

        }
        private double CurrentExtraCosts()
        {
            RentViewModel currentRent = new RentViewModel(this, new Models.Rent());
            currentRent.StartDate = new DateTime(1, 1, 1);

            foreach (RentViewModel rent in RentUpdates)
            {
                if (rent.StartDate > currentRent.StartDate)
                {
                    currentRent = rent;
                }
            }

            return currentRent.ExtraCostsTotal;
        }

        private double CurrentRent()
        {
            RentViewModel currentRent = new RentViewModel(this, new Models.Rent());
            currentRent.StartDate = new DateTime();

            foreach (RentViewModel rent in RentUpdates)
            {
                if (rent.StartDate > currentRent.StartDate)
                {
                    currentRent = rent;
                }
            }

            return currentRent.ColdRent;
        }



    }
}
