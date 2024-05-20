using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    class Rent : INotifyPropertyChanged
    {
        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        public DateTime EndDate { get; set; } // necessary? or calculate using new Rent StartDate - 1, when adding RentUpdate?

        private double coldRent;
        public double ColdRent
        {

            get { return coldRent; }
            set
            {
                coldRent = value;
                OnPropertyChanged(nameof(ColdRent));
                OnPropertyChanged(nameof(AnnualRent));
            }

        }

        public double AnnualRent => ColdRent * 12;


        private double extraCostsShared;
        public double ExtraCostsShared
        {
            get { return extraCostsShared; }
            set
            {
                extraCostsShared = value;
                OnPropertyChanged(nameof(ExtraCostsShared));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
            }
        }

        private double extraCostsHeating;
        public double ExtraCostsHeating
        {
            get { return extraCostsHeating; }
            set
            {
                extraCostsHeating = value;
                OnPropertyChanged(nameof(ExtraCostsHeating));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
            }
        }

        public double ExtraCostsTotal => ExtraCostsShared + ExtraCostsHeating;
        public double AnnualExtraCosts => ExtraCostsTotal * 12;


        public Rent()
        {

        }

        public Rent(DateTime startDate, double coldRent, double extraCostsShared, double extraCostsHeating)
        {
            StartDate = startDate;
            ColdRent = coldRent;
            ExtraCostsShared = extraCostsShared;
            ExtraCostsHeating = extraCostsHeating;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
