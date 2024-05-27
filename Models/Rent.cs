using SharedLivingCostCalculator.BoilerPlateReduction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SharedLivingCostCalculator.Models
{
    class Rent : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private ValidationHelper helper = new ValidationHelper();

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
                helper.ClearError();

                if (Double.IsNaN(value))
                {
                    helper.AddError("value must be a number", nameof(ColdRent));
                }

                if (value < 0)
                {
                    helper.AddError("value must be greater than 0", nameof(ColdRent));
                }

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
                helper.ClearError();

                if (Double.IsNaN(value))
                {
                    helper.AddError("value must be a number", nameof(ColdRent));
                }

                if (value < 0)
                {
                    helper.AddError("value must be greater than 0", nameof(ColdRent));
                }

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
                helper.ClearError();

                if (Double.IsNaN(value))
                {
                    helper.AddError("value must be a number", nameof(ColdRent));
                }

                if (value < 0)
                {
                    helper.AddError("value must be greater than 0", nameof(ColdRent));
                }

                extraCostsHeating = value;
                OnPropertyChanged(nameof(ExtraCostsHeating));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
            }
        }

        public double ExtraCostsTotal => ExtraCostsShared + ExtraCostsHeating;
        public double AnnualExtraCosts => ExtraCostsTotal * 12;

        public bool HasErrors => ((INotifyDataErrorInfo)helper).HasErrors;

        public Rent()
        {
                helper.ErrorsChanged += (_, e) => this.ErrorsChanged?.Invoke(this, e);
        }

        public Rent(DateTime startDate, double coldRent, double extraCostsShared, double extraCostsHeating)
        {
            helper.ErrorsChanged += (_, e) => this.ErrorsChanged?.Invoke(this, e);

            StartDate = startDate;
            ColdRent = coldRent;
            ExtraCostsShared = extraCostsShared;
            ExtraCostsHeating = extraCostsHeating;
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            return ((INotifyDataErrorInfo)helper).GetErrors(propertyName);
        }
    }
}
