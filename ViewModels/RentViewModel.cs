using SharedLivingCostCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.ViewModels
{
    public class RentViewModel : BaseViewModel
    {
        private readonly Rent _rent;
        public readonly BillingViewModel? BillingViewModel;
        public bool HasBilling { get; } = false;

        public Rent GetRent => _rent;

        // startdates in RentViewModel list indicate enddates of older rentviewmodels
        // the next oldest RentViewModel is the successor of a RentViewModel
        // this has to be taken into account for the cost calculations

        // a new billing must allways create a new RentViewModel
        // the user can add RentViewModels, in case a raise was received
        // for different reasons than annual billing

        public DateTime StartDate
        {
            get { return _rent.StartDate; }
            set { _rent.StartDate = value; OnPropertyChanged(nameof(StartDate)); }
        }


        // monthly costs
        public double ColdRent
        {
            get { return _rent.ColdRent; }
            set { _rent.ColdRent = value; 
                OnPropertyChanged(nameof(ColdRent));
                OnPropertyChanged(nameof(CostsTotal));
                OnPropertyChanged(nameof(AnnualRent));
                OnPropertyChanged(nameof(AnnualCostsTotal));
            }

        }

        public double ExtraCostsShared
        {
            get { return _rent.ExtraCostsShared; }
            set { _rent.ExtraCostsShared = value;
                OnPropertyChanged(nameof(ExtraCostsShared));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(CostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
                OnPropertyChanged(nameof(AnnualCostsTotal));
            }
        }

        public double ExtraCostsHeating
        {
            get { return _rent.ExtraCostsHeating; }
            set { _rent.ExtraCostsHeating = value; 
                OnPropertyChanged(nameof(ExtraCostsHeating));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(CostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
                OnPropertyChanged(nameof(AnnualCostsTotal));
            }
        }


        // monthly costs sums
        public double ExtraCostsTotal => ExtraCostsShared + ExtraCostsHeating;
        public double CostsTotal => ColdRent + ExtraCostsTotal;

        // annual interval sums
        public double AnnualRent => ColdRent * 12;
        public double AnnualExtraCosts => ExtraCostsTotal * 12;

        // annual costs sum
        public double AnnualCostsTotal => AnnualRent + AnnualExtraCosts;

        public RentViewModel(Rent rent, BillingViewModel? billingViewModel = null)
        {
            _rent = rent;

            if (billingViewModel != null)
            {
                HasBilling = true;
            }
            BillingViewModel = billingViewModel;

        }
    }
}
