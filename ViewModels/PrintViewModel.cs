/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PrintViewModel  : BaseViewModel
 * 
 *  viewmodel for PrintView
 *  
 *  PrintView shall generate overviews of rent and billing data
 *  they shall be formated in HTML or via FlowDocument to give a nice and easy to read
 *  overview over the costs of the selected period of time.
 *  generation of this printable data shall be done within this viewmodel.
 *  
 *  currently
 */
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Contract;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models.Contract;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;

namespace SharedLivingCostCalculator.ViewModels
{
    public class PrintViewModel : BaseViewModel
    {
        private readonly AccountingViewModel _AccountingViewModel;
        public AccountingViewModel Accounting => _AccountingViewModel;


        private FlowDocument _ActiveFlowDocument;
        public FlowDocument ActiveFlowDocument
        {
            get { return _ActiveFlowDocument; }
            set
            {
                _ActiveFlowDocument = value;
                OnPropertyChanged(nameof(ActiveFlowDocument));
            }
        }


        private FlatManagementViewModel _FlatManagementViewModel;


        private FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;



        private bool _RentOutputSelected;

        public bool RentOutputSelected
        {
            get { return _RentOutputSelected; }
            set
            {
                _RentOutputSelected = value;
                OnPropertyChanged(nameof(RentOutputSelected));
            }
        }


        private int _SelectedYear;
        public int SelectedYear
        {
            get { return _SelectedYear; }
            set
            {
                _SelectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
            }
        }

        #region Collections

        public ObservableCollection<int> TimeScale { get; set; } = new ObservableCollection<int>();

        #endregion

        #region Commands
        public ICommand CreatePrintOutputCommand { get; }

        #endregion


        public PrintViewModel(FlatManagementViewModel flatManagementViewModel)
        {
            _FlatManagementViewModel = flatManagementViewModel;

            if (_FlatManagementViewModel.SelectedItem != null)
            {
                _FlatViewModel = flatManagementViewModel.SelectedItem;
                _FlatViewModel.PropertyChanged += _FlatViewModel_PropertyChanged;
            }

            _AccountingViewModel = flatManagementViewModel.Accounting;

            _AccountingViewModel.AccountingChanged += _AccountingViewModel_AccountingChanged;

            CreatePrintOutputCommand = new RelayCommand((s) => BuildFlowDocument(), (s) => true);

            //_AccountingViewModel.FlatManagement.FlatViewModelChange += FlatManagement_FlatViewModelChange;

            Update();
        }

        // methods
        #region methods

        private string BuildAddressDetailsString()
        {
            if (Application.Current.Resources["IDF_Rooms"] != null)
            {
                string rooms = Application.Current.Resources["IDF_Rooms"].ToString();

                return $"{_FlatViewModel.Area}m², {_FlatViewModel.RoomCount} {rooms}";                
            }

            return string.Empty;
        }

        private void BuildFlowDocument()
        {
            string ReportHeader = "";

            Style textParagraph = new Style();
            Style headerParagraph = new Style();

    
            try
            {
                textParagraph = Application.Current.FindResource("TextParagraph") as Style;
                headerParagraph = Application.Current.FindResource("HeaderParagraph") as Style;

                ReportHeader = Application.Current.FindResource("IDF_Address").ToString();
            }
            catch (Exception)
            {

            }


            ActiveFlowDocument = new FlowDocument();

            Paragraph p = new Paragraph(new Run(ReportHeader));
            p.Style = headerParagraph;
            ActiveFlowDocument.Blocks.Add(p);

            p = new Paragraph(new Run(FlatViewModel.Address));
            p.Style = textParagraph;
            ActiveFlowDocument.Blocks.Add(p);

            p = new Paragraph(new Run(BuildAddressDetailsString()));
            p.Style = textParagraph;
            ActiveFlowDocument.Blocks.Add(p);

            p = new Paragraph(new Run("Rent Report:"));
            p.Style = headerParagraph;
            ActiveFlowDocument.Blocks.Add(p);

            p = new Paragraph(new Run(BuildRentDetailsString()));
            p.Style = textParagraph;
            ActiveFlowDocument.Blocks.Add(p);

        }

        private string BuildRentDetailsString()
        {
            string rentOutput = string.Empty;

            if (RentOutputSelected)
            {
                ObservableCollection<RentViewModel> RentList = FindRelevantRentViewModels();

      

                for (int i = 0; i < RentList.Count; i++)
                {

                    rentOutput += $"rent begin:\t\t{RentList[i].StartDate:d}\n";

                    for (int monthCounter = 0; monthCounter < 12; monthCounter++)
                    {
                        if (i + 1 < RentList.Count)
                        {
                            if (RentList[i + 1].StartDate.Month - 2 < monthCounter)
                            {
                                break;
                            }

                            rentOutput += $"{monthCounter + 1:00}/{SelectedYear} rent:\t\t{RentList[i].ColdRent:C2}\n" +
                                    $"{monthCounter + 1:00}/{SelectedYear} fixed\t\t{RentList[i].FixedCostsAdvance:C2}\n" +
                                    $"{monthCounter + 1:00}/{SelectedYear} heating\t{RentList[i].HeatingCostsAdvance:C2}\n\n";
                            
                        }
                        else
                        {
                            if (monthCounter < RentList[i].StartDate.Month - 1)
                            {
                                continue;
                            }
                            else
                            {
                                rentOutput += $"{monthCounter + 1:00}/{SelectedYear} rent:\t\t{RentList[i].ColdRent:C2}\n" +
                                    $"{monthCounter + 1:00}/{SelectedYear} fixed\t\t{RentList[i].FixedCostsAdvance:C2}\n" +
                                    $"{monthCounter + 1:00}/{SelectedYear} heating\t{RentList[i].HeatingCostsAdvance:C2}\n\n";
                            }
                            
                        }

                    }



                }           
            }

            return rentOutput;
        }

        private void BuildTimeScale()
        {
            TimeScale.Clear();

            foreach (RentViewModel item in _FlatViewModel.RentUpdates)
            {
                if (!TimeScale.Contains(item.StartDate.Year))
                {
                    TimeScale.Add(item.StartDate.Year);
                }                
            }

            TimeScale = new ObservableCollection<int>(TimeScale.OrderBy(i => i));

            if (TimeScale.Count > 0)
            {
                SelectedYear = TimeScale.Last();
            }            

            OnPropertyChanged(nameof(SelectedYear));
            OnPropertyChanged(nameof(TimeScale));
        }

        public double DetermineFullMonthValueUntilYearsEnd()
        {
            double Months = 0.0;

            DateTime start = new DateTime(SelectedYear, 01, 01);
            DateTime end = new DateTime(SelectedYear, 12, 31);

            int month = 0;
            double halfmonth = 0.0;

            if (start.Day == 1 && end.Day != 14 && start.Year == end.Year)
            {
                month = end.Month - start.Month + 1;
            }

            if (start.Day == 15 && end.Day != 14 && start.Year == end.Year)
            {
                month = end.Month - start.Month;
                halfmonth += 0.5;
            }

            if (end.Day == 14 || end.Day == 15 && start.Year == end.Year)
            {
                month = end.Month - start.Month - 1;
                halfmonth = 0.5;
            }

            Months = month + halfmonth;


            return Months;
        }

        public ObservableCollection<RentViewModel> FindRelevantRentViewModels()
        {
            ObservableCollection<RentViewModel> preSortList = new ObservableCollection<RentViewModel>();
            ObservableCollection<RentViewModel> RentList = new ObservableCollection<RentViewModel>();

            DateTime startDate = new DateTime(SelectedYear, 1, 1);
            DateTime endDate = new DateTime(SelectedYear, 12, 31);

            if (FlatViewModel.RentUpdates.Count > 0)
            {
                // filling the collection with potential matches
                foreach (RentViewModel rent in FlatViewModel.RentUpdates)
                {
                    // rent begins after selected year ends
                    if (rent.StartDate.Year > SelectedYear)
                    {
                        continue;
                    }

                    // rent begins before selected year starts
                    if (rent.StartDate < startDate)
                    {
                        preSortList.Add(new RentViewModel(FlatViewModel, rent.Rent));
                        continue;
                    }

                    // rent begins before selected year ends
                    if (rent.StartDate <endDate)
                    {
                        preSortList.Add(new RentViewModel(FlatViewModel, rent.Rent));

                        continue;
                    }

                    // rent begins after Billing period start but before Billing period end
                    if (rent.StartDate > startDate || rent.StartDate < endDate)
                    {
                        preSortList.Add(new RentViewModel(FlatViewModel, rent.Rent));
                    }
                }

                //    RentViewModel? comparer = new RentViewModel(_flatViewModel, new Rent() { StartDate = StartDate });
                //    bool firstRun = true;

                //    // building a collection of relevant rent items
                //    foreach (RentViewModel item in preSortList)
                //    {
                //        if (item.StartDate >= StartDate)
                //        {
                //            RentList.Add(item);
                //            continue;
                //        }

                //        if (item.StartDate < StartDate && firstRun)
                //        {
                //            firstRun = false;
                //            comparer = item;
                //            continue;
                //        }

                //        if (item.StartDate < StartDate && item.StartDate > comparer.StartDate)
                //        {
                //            comparer = item;
                //        }
                //    }
                //    RentList.Add(comparer);
            }

            // sort List by StartDate, ascending
            RentList = new ObservableCollection<RentViewModel>(preSortList.OrderBy(i => i.StartDate));

            return RentList;
        }

        public void Update()
        {
            if (_AccountingViewModel.FlatViewModel != null)
            {
                _FlatViewModel = _AccountingViewModel.FlatViewModel;

                BuildTimeScale();

                BuildFlowDocument();
            }

            OnPropertyChanged(nameof(FlatViewModel));
            OnPropertyChanged(nameof(ActiveFlowDocument));
        }

        #endregion methods


        // events
        #region events

        private void _AccountingViewModel_AccountingChanged(object? sender, EventArgs e)
        {
            _AccountingViewModel.Rents.SelectedItemChange -= Rents_SelectedItemChange;

            _AccountingViewModel.Rents.SelectedItemChange += Rents_SelectedItemChange;

            Update();
        }


        private void FlatManagement_FlatViewModelChange(object? sender, EventArgs e)
        {
            _AccountingViewModel.FlatManagement.SelectedItem.PropertyChanged += SelectedItem_PropertyChanged;

            Update();
        }


        private void _FlatViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Update();
        }


        private void Rents_SelectedItemChange(object? sender, EventArgs e)
        {
            Update();
        }


        private void SelectedItem_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Update();
        }


        private void UpdateViewModel_RentConfigurationChange(object? sender, EventArgs e)
        {
            Update();
        }

        #endregion events
    }
}
// EOF