/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PrintViewModel  : BaseViewModel
 * 
 *  viewmodel for PrintView
 *  
 *  PrintView shall generate overviews of rent and billing data
 *  they shall be formated in HTML or via FlowDocument to give a nice and easy to read
 *  overview over the costs of the selected period of time.
 *  generation of this printable data shall be done within this viewmodel.  
 */
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Contract;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using SharedLivingCostCalculator.Commands;
using System.Collections.ObjectModel;
using SharedLivingCostCalculator.Models.Financial;
using System.Windows.Media;

namespace SharedLivingCostCalculator.ViewModels
{
    public class PrintViewModel : BaseViewModel
    {

        // Properties & Fields
        #region Properties & Fields

        private readonly AccountingViewModel _AccountingViewModel;
        public AccountingViewModel Accounting => _AccountingViewModel;


        private BillingViewModel? _BillingViewModel;
        public BillingViewModel? BillingViewModel
        {
            get { return _BillingViewModel; }
            set
            {
                _BillingViewModel = value;
                OnPropertyChanged(nameof(BillingViewModel));
            }
        }


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


        private bool _BillingOutputSelected;
        public bool BillingOutputSelected
        {
            get { return _BillingOutputSelected; }
            set
            {
                _BillingOutputSelected = value;
                OnPropertyChanged(nameof(BillingOutputSelected));
            }
        }


        private bool _CreditOutputSelected;
        public bool CreditOutputSelected
        {
            get { return _CreditOutputSelected; }
            set
            {
                _CreditOutputSelected = value;
                OnPropertyChanged(nameof(CreditOutputSelected));
            }
        }


        private FlatManagementViewModel _FlatManagementViewModel;


        private FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;


        private bool _OtherOutputSelected;
        public bool OtherOutputSelected
        {
            get { return _OtherOutputSelected; }
            set
            {
                _OtherOutputSelected = value;
                OnPropertyChanged(nameof(OtherOutputSelected));
            }
        }


        private bool _RentHasBilling;
        public bool RentHasBilling
        {
            get { return _RentHasBilling; }
            set
            {
                _RentHasBilling = value;
                OnPropertyChanged(nameof(RentHasBilling));
            }
        }


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


        private bool _RoomsOutputSelected;
        public bool RoomsOutputSelected
        {
            get { return _RoomsOutputSelected; }
            set
            {
                _RoomsOutputSelected = value;
                OnPropertyChanged(nameof(RoomsOutputSelected));
            }
        }


        private string _SelectedDetailOption;
        public string SelectedDetailOption
        {
            get { return _SelectedDetailOption; }
            set
            {
                _SelectedDetailOption = value;
                OnPropertyChanged(nameof(SelectedDetailOption));
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

                CheckRentForHasBillingTrue();
            }
        }

        #endregion



        #region Collections

        public ObservableCollection<string> DetailOptions { get; set; } = new ObservableCollection<string>();


        public ObservableCollection<int> TimeScale { get; set; } = new ObservableCollection<int>();

        #endregion


        #region Commands

        public ICommand CreatePrintOutputCommand { get; }

        #endregion


        // Constructors
        #region Constructors

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

            FillDetailOptions();

            Update();
        }

        #endregion


        // methods
        #region methods

        private Block BillingPlanTable(BillingViewModel viewModel)
        {
            Table billingPlanTable = DataOutputTable();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            dataRowGroup.Rows.Add(DataOutputTableRowBilling(viewModel, viewModel.TotalFixedCostsPerPeriod, viewModel.Billing.TotalFixedCostsPerPeriod.TransactionItem));
            dataRowGroup.Rows.Add(DataOutputTableRowBilling(viewModel, viewModel.TotalHeatingCostsPerPeriod, viewModel.Billing.TotalHeatingCostsPerPeriod.TransactionItem));

            if (OtherOutputSelected)
            {
                dataRowGroup.Rows.Add(DataOutputTableRowBilling(viewModel, viewModel.OtherFTISum, "other"));
            }

            if (CreditOutputSelected)
            {
                dataRowGroup.Rows.Add(DataOutputTableRowBilling(viewModel, -1 * viewModel.CreditSum, "credit"));
            }

            if (OtherOutputSelected || CreditOutputSelected)
            {
                if (OtherOutputSelected && !CreditOutputSelected)
                {
                    dataRowGroup.Rows.Add(DataOutputTableRowBilling(viewModel, viewModel.TotalCostsNoPayments, "sum", true));
                }

                if (!OtherOutputSelected && CreditOutputSelected)
                {
                    double reducedsum = viewModel.TotalCostsPerPeriod - viewModel.CreditSum;

                    dataRowGroup.Rows.Add(DataOutputTableRowBilling(viewModel, reducedsum, "sum", true));
                }

                if (OtherOutputSelected && CreditOutputSelected)
                {
                    double reducedsum = viewModel.TotalCostsNoPayments - viewModel.CreditSum;

                    dataRowGroup.Rows.Add(DataOutputTableRowBilling(viewModel, reducedsum, "sum", true));
                }
            }
            else
            {
                dataRowGroup.Rows.Add(DataOutputTableRowBilling(viewModel, viewModel.TotalCostsPerPeriod, "sum", true));
            }

            billingPlanTable.RowGroups.Add(dataRowGroup);

            return billingPlanTable;
        }


        private string BuildAddressDetails()
        {
            //Section section = new Section();

            //string ReportHeader = "";
            string rooms = "";

            try
            {
                //ReportHeader = Application.Current.FindResource("IDF_Address").ToString();
                rooms = Application.Current.Resources["IDF_Rooms"].ToString();
            }
            catch (Exception)
            {
            }

            //Paragraph p = new Paragraph(new Run(ReportHeader));
            //p.Style = headerParagraph;
            //section.Blocks.Add(p);

            //p = new Paragraph(new Run(FlatViewModel.Address));
            //p.Style = textParagraph;
            //section.Blocks.Add(p);

            //p = new Paragraph(new Run($"{_FlatViewModel.Area}m², {_FlatViewModel.RoomCount} {rooms}"));
            //p.Style = textParagraph;
            //section.Blocks.Add(p);

            return $"{FlatViewModel.Address}, {_FlatViewModel.Area}m², {_FlatViewModel.RoomCount} {rooms}";
        }

        private Section BuildBillingDetails()
        {
            Section billingOutput = new Section();

            if (BillingOutputSelected && BillingViewModel != null)
            {
                billingOutput.Blocks.Add(BillingPlanTable(BillingViewModel));


                if (RoomsOutputSelected)
                {
                    foreach (RoomViewModel item in BillingViewModel.FlatViewModel.Rooms)
                    {
                        RoomCostShareBilling roomCostShareBilling = new RoomCostShareBilling(item.GetRoom, BillingViewModel);

                        billingOutput.Blocks.Add(DataOutputConsumptionRooms(roomCostShareBilling));

                        billingOutput.Blocks.Add(DataOutputBillingRooms(roomCostShareBilling));

                    }
                }
            }

            return billingOutput;
        }

     
        private Section BuildCreditDetails()
        {
            Section creditOutput = new Section();

            if (CreditOutputSelected)
            {
                ObservableCollection<RentViewModel> RentList = FindRelevantRentViewModels();

                for (int i = 0; i < RentList.Count; i++)
                {

                    if (!RentList[i].HasCredits)
                    {
                        continue;
                    }

                    if (RentList[i].StartDate.Year < SelectedYear)
                    {
                        if (RentList[i + 1].StartDate > RentList[i].StartDate && RentList[i + 1].StartDate.Year < SelectedYear)
                        {
                            continue;
                        }
                        else
                        {
                            creditOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 0, 0, 0), FontWeight = FontWeights.Bold });
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            creditOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 0, 0, 0), FontWeight = FontWeights.Bold });
                        }
                        else
                        {
                            creditOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 40, 0, 0), FontWeight = FontWeights.Bold });
                        }
                    }


                    Table headerTable = DataOutputTable();
                    headerTable.RowGroups.Add(TableRowGroupRentHeader());

                    creditOutput.Blocks.Add(headerTable);

                    if (SelectedDetailOption.Equals("TimeScale"))
                    {
                        for (int monthCounter = 1; monthCounter < 13; monthCounter++)
                        {
                            if (!RentList[i].HasCredits)
                            {
                                creditOutput.Blocks.Add(new Paragraph(new Run($"end:\t\t\t{RentList[i].StartDate - TimeSpan.FromDays(1):d}")));
                                continue;
                            }

                            if (RentList[i].StartDate.Year == SelectedYear && monthCounter < RentList[i].StartDate.Month)
                            {
                                continue;
                            }

                            if (i + 1 < RentList.Count)
                            {
                                // später im Ablauf, um Nachfolger zu berücksichtigen

                                if (RentList[i + 1].StartDate.Month - 1 < monthCounter)
                                {
                                    break;
                                }

                                if (RentList[i].StartDate.Year < SelectedYear)
                                {
                                    creditOutput.Blocks.Add(CreditPlanFlatTable(RentList[i], monthCounter));
                                }
                                else if (RentList[i].StartDate.Year == SelectedYear
                                    && monthCounter >= RentList[i].StartDate.Month)
                                {
                                    creditOutput.Blocks.Add(CreditPlanFlatTable(RentList[i], monthCounter));
                                }
                            }
                            else
                            {
                                if (RentList[i].StartDate.Year == SelectedYear && monthCounter < RentList[i].StartDate.Month)
                                {
                                    continue;
                                }
                                else
                                {
                                    creditOutput.Blocks.Add(CreditPlanFlatTable(RentList[i], monthCounter));
                                }
                            }
                        }
                    }
                    else
                    {
                        creditOutput.Blocks.Add(CreditPlanFlatTable(RentList[i]));
                    }

                    if (i + 1 < RentList.Count && RentList[i + 1].HasOtherCosts == false)
                    {
                        creditOutput.Blocks.Add(new Paragraph(new Run($"rent end:\t\t{RentList[i + 1].StartDate - TimeSpan.FromDays(1):d}")));

                    }
                }

            }

            return creditOutput;
        }


        private void BuildFlowDocument()
        {
            ActiveFlowDocument = new FlowDocument();
            ActiveFlowDocument.TextAlignment = TextAlignment.Left;
            ActiveFlowDocument.PageHeight = 640;
            ActiveFlowDocument.PageWidth = 640;

            Style textParagraph = new Style();
            Style headerParagraph = new Style();

            try
            {
                textParagraph = Application.Current.FindResource("TextParagraph") as Style;
                headerParagraph = Application.Current.FindResource("HeaderParagraph") as Style;

            }
            catch (Exception)
            {

            }

            Paragraph p;
            p = new Paragraph(new Run("Shared Living Cost Calculator: print output")) { Style = headerParagraph };
            ActiveFlowDocument.Blocks.Add(p);
            //ActiveFlowDocument.Blocks.Add(BuildAddressDetails(headerParagraph, textParagraph));

            CheckRentForHasBillingTrue();

            if (BillingOutputSelected && BillingViewModel != null)
            {
                p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };
                ActiveFlowDocument.Blocks.Add(p);

                p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };

                p.Inlines.Add(new Run($"Annual Billing {SelectedYear - 1}: ")
                { FontWeight = FontWeights.Bold, FontSize = 16.0 });
                p.Inlines.Add(new Run($"{BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
                //p.Style = headerParagraph;
                ActiveFlowDocument.Blocks.Add(p);

                ActiveFlowDocument.Blocks.Add(BuildBillingDetails());
            }

            if (RentOutputSelected)
            {

                p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };
                ActiveFlowDocument.Blocks.Add(p);

                p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };

                p.Inlines.Add(new Run($"Rent Plan Flat {SelectedYear}: ")
                { FontWeight = FontWeights.Bold, FontSize = 16.0 });
                p.Inlines.Add(new Run($"{BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
                //p.Style = headerParagraph;
                ActiveFlowDocument.Blocks.Add(p);

                ActiveFlowDocument.Blocks.Add(BuildRentDetails());
            }

            if (RoomsOutputSelected)
            {
                p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };
                ActiveFlowDocument.Blocks.Add(p);

                p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };
                p.Inlines.Add(new Run($"Rent Plan Rooms {SelectedYear}: ")
                { FontWeight = FontWeights.Bold, FontSize = 16.0 });
                p.Inlines.Add(new Run($"{BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
                //p.Style = headerParagraph;
                ActiveFlowDocument.Blocks.Add(p);

                ActiveFlowDocument.Blocks.Add(BuildRoomDetails());
            }

            if (OtherOutputSelected)
            {
                if (!RentOutputSelected && BillingOutputSelected)
                {

                }
                else
                {
                    p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };
                    ActiveFlowDocument.Blocks.Add(p);

                    p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };
                    p.Inlines.Add(new Run($"Other Costs Plan Rooms {SelectedYear}: ")
                    { FontWeight = FontWeights.Bold, FontSize = 16.0 });
                    p.Inlines.Add(new Run($"{BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
                    //p.Style = headerParagraph;
                    ActiveFlowDocument.Blocks.Add(p);

                    ActiveFlowDocument.Blocks.Add(BuildOtherCostDetails());
                }

            }

            if (CreditOutputSelected)
            {
                if (!RentOutputSelected && BillingOutputSelected)
                {

                }
                else if (!RentOutputSelected && !BillingOutputSelected
                    || RentOutputSelected && BillingOutputSelected
                    || RentOutputSelected && !BillingOutputSelected)
                {
                    p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };
                    ActiveFlowDocument.Blocks.Add(p);

                    p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };
                    p.Inlines.Add(new Run($"Credit Plan Rooms {SelectedYear}: ")
                    { FontWeight = FontWeights.Bold, FontSize = 16.0 });
                    p.Inlines.Add(new Run($"{BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
                    //p.Style = headerParagraph;
                    ActiveFlowDocument.Blocks.Add(p);

                    ActiveFlowDocument.Blocks.Add(BuildCreditDetails());
                }
            }
        }


        private Section BuildOtherCostDetails()
        {
            Section rentOutput = new Section();

            if (OtherOutputSelected)
            {
                ObservableCollection<RentViewModel> RentList = FindRelevantRentViewModels();

                for (int i = 0; i < RentList.Count; i++)
                {

                    if (!RentList[i].HasOtherCosts)
                    {
                        continue;
                    }

                    if (RentList[i].StartDate.Year < SelectedYear)
                    {
                        if (RentList[i + 1].StartDate > RentList[i].StartDate && RentList[i + 1].StartDate.Year < SelectedYear)
                        {
                            continue;
                        }
                        else
                        {
                            rentOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 0, 0, 0), FontWeight = FontWeights.Bold });
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            rentOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 0, 0, 0), FontWeight = FontWeights.Bold });
                        }
                        else
                        {
                            rentOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 40, 0, 0), FontWeight = FontWeights.Bold });
                        }
                    }


                    Table headerTable = DataOutputTable();
                    headerTable.RowGroups.Add(TableRowGroupRentHeader());

                    rentOutput.Blocks.Add(headerTable);

                    if (SelectedDetailOption.Equals("TimeScale"))
                    {
                        for (int monthCounter = 1; monthCounter < 13; monthCounter++)
                        {
                            if (!RentList[i].HasOtherCosts)
                            {
                                rentOutput.Blocks.Add(new Paragraph(new Run($"end:\t\t\t{RentList[i].StartDate - TimeSpan.FromDays(1):d}")));
                                continue;
                            }

                            if (RentList[i].StartDate.Year == SelectedYear && monthCounter < RentList[i].StartDate.Month)
                            {
                                continue;
                            }

                            if (i + 1 < RentList.Count)
                            {
                                // später im Ablauf, um Nachfolger zu berücksichtigen

                                if (RentList[i + 1].StartDate.Month - 1 < monthCounter)
                                {
                                    break;
                                }

                                if (RentList[i].StartDate.Year < SelectedYear)
                                {
                                    rentOutput.Blocks.Add(OtherPlanFlatTable(RentList[i], monthCounter));
                                }
                                else if (RentList[i].StartDate.Year == SelectedYear
                                    && monthCounter >= RentList[i].StartDate.Month)
                                {
                                    rentOutput.Blocks.Add(OtherPlanFlatTable(RentList[i], monthCounter));
                                }
                            }
                            else
                            {
                                if (RentList[i].StartDate.Year == SelectedYear && monthCounter < RentList[i].StartDate.Month)
                                {
                                    continue;
                                }
                                else
                                {
                                    rentOutput.Blocks.Add(OtherPlanFlatTable(RentList[i], monthCounter));
                                }

                            }

                        }
                    }
                    else
                    {
                        rentOutput.Blocks.Add(OtherPlanFlatTable(RentList[i]));
                    }


                    if (i + 1 < RentList.Count && RentList[i + 1].HasOtherCosts == false)
                    {
                        rentOutput.Blocks.Add(new Paragraph(new Run($"rent end:\t\t{RentList[i + 1].StartDate - TimeSpan.FromDays(1):d}")));

                    }
                }

            }

            return rentOutput;
        }



        private Section BuildRentDetails()
        {
            Section rentOutput = new Section();

            if (RentOutputSelected)
            {
                ObservableCollection<RentViewModel> RentList = FindRelevantRentViewModels();

                for (int i = 0; i < RentList.Count; i++)
                {



                    if (RentList[i].StartDate.Year < SelectedYear)
                    {
                        if (RentList[i + 1].StartDate > RentList[i].StartDate && RentList[i + 1].StartDate.Year < SelectedYear)
                        {
                            continue;
                        }
                        else
                        {
                            rentOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 0, 0, 0), FontWeight = FontWeights.Bold });
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            rentOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 0, 0, 0), FontWeight = FontWeights.Bold });
                        }
                        else
                        {
                            rentOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 40, 0, 0), FontWeight = FontWeights.Bold });
                        }
                    }


                    Table headerTable = DataOutputTable();
                    headerTable.RowGroups.Add(TableRowGroupRentHeader());

                    rentOutput.Blocks.Add(headerTable);

                    if (SelectedDetailOption.Equals("TimeScale"))
                    {
                        for (int monthCounter = 1; monthCounter < 13; monthCounter++)
                        {

                            if (i + 1 < RentList.Count)
                            {
                                // später im Ablauf, um Nachfolger zu berücksichtigen

                                if (RentList[i + 1].StartDate.Month - 1 < monthCounter)
                                {
                                    break;
                                }

                                if (RentList[i].StartDate.Year < SelectedYear)
                                {
                                    rentOutput.Blocks.Add(RentPlanFlatTable(RentList[i], monthCounter));
                                }
                                else if (RentList[i].StartDate.Year == SelectedYear
                                    && monthCounter >= RentList[i].StartDate.Month)
                                {
                                    rentOutput.Blocks.Add(RentPlanFlatTable(RentList[i], monthCounter));
                                }


                            }
                            else
                            {
                                if (RentList[i].StartDate.Year == SelectedYear && monthCounter < RentList[i].StartDate.Month)
                                {
                                    continue;
                                }
                                else
                                {
                                    rentOutput.Blocks.Add(RentPlanFlatTable(RentList[i], monthCounter));
                                }

                            }

                        }
                    }
                    else
                    {
                        rentOutput.Blocks.Add(RentPlanFlatTable(RentList[i]));

                    }

                }

            }


            return rentOutput;
        }


        private Section BuildRoomDetails()
        {
            Section roomsOutput = new Section();

            if (RoomsOutputSelected)
            {
                ObservableCollection<RentViewModel> RentList = FindRelevantRentViewModels();

                for (int i = 0; i < RentList.Count; i++)
                {
                    if (RentList[i].StartDate.Year < SelectedYear)
                    {
                        if (RentList[i + 1].StartDate > RentList[i].StartDate && RentList[i + 1].StartDate.Year < SelectedYear)
                        {
                            continue;
                        }
                        else
                        {
                            roomsOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 0, 0, 0), FontWeight = FontWeights.Bold });
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            roomsOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 0, 0, 0), FontWeight = FontWeights.Bold });
                        }
                        else
                        {
                            roomsOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 40, 0, 0), FontWeight = FontWeights.Bold });
                        }
                    }

                    if (SelectedDetailOption.Equals("TimeScale"))
                    {
                        for (int monthCounter = 1; monthCounter < 13; monthCounter++)
                        {

                            if (i + 1 < RentList.Count)
                            {
                                // später im Ablauf, um Nachfolger zu berücksichtigen

                                if (RentList[i + 1].StartDate.Month - 1 < monthCounter)
                                {
                                    break;
                                }

                                if (RentList[i].StartDate.Year < SelectedYear)
                                {
                                    roomsOutput.Blocks.Add(RentPlanRoomsTable(RentList[i], monthCounter));
                                }
                                else if (RentList[i].StartDate.Year == SelectedYear
                                    && monthCounter >= RentList[i].StartDate.Month)
                                {
                                    roomsOutput.Blocks.Add(RentPlanRoomsTable(RentList[i], monthCounter));
                                }


                            }
                            else
                            {
                                if (RentList[i].StartDate.Year == SelectedYear && monthCounter < RentList[i].StartDate.Month)
                                {
                                    continue;
                                }
                                else
                                {
                                    roomsOutput.Blocks.Add(RentPlanRoomsTable(RentList[i], monthCounter));
                                }

                            }

                        }
                    }
                    else
                    {
                        roomsOutput.Blocks.Add(RentPlanRoomsTable(RentList[i]));

                    }

                }

            }


            return roomsOutput;
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


        private void CheckRentForHasBillingTrue()
        {
            foreach (RentViewModel item in FlatViewModel.RentUpdates)
            {
                if (item.StartDate.Year > _SelectedYear)
                {
                    continue;
                }

                foreach (BillingViewModel billingViewModel in FlatViewModel.AnnualBillings)
                {
                    if (billingViewModel.Year == item.StartDate.Year - 1)
                    {
                        RentHasBilling = true;

                        BillingViewModel = billingViewModel;
                    }
                }

                RentHasBilling = false;
                BillingViewModel = null;
            }
        }


        private Block CreditPlanFlatTable(RentViewModel rentViewModel, int monthCounter = -1)
        {
            Table otherPlanFlatTable = DataOutputTable();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;


            foreach (FinancialTransactionItemRentViewModel item in rentViewModel.Credits)
            {
                dataRowGroup.Rows.Add(DataOutputTableRow(rentViewModel, item.TransactionSum, item.TransactionItem, monthCounter));
            }

            dataRowGroup.Rows.Add(DataOutputTableRow(rentViewModel, rentViewModel.CreditSum, "sum", monthCounter, true));

            otherPlanFlatTable.RowGroups.Add(dataRowGroup);

            return otherPlanFlatTable;
        }


        private Table DataOutputBillingRooms(RoomCostShareBilling roomCostShareBilling)
        {
            Table billingTableRooms = DataOutputTableRoomsBilling();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            //TableRow dataRow = new TableRow();

            //TableCell Year = new TableCell();
            //Year.TextAlignment = TextAlignment.Right;

            //TableCell RoomName = new TableCell();

            //TableCell Area = new TableCell();

            //TableCell AreaSum = new TableCell();
            //AreaSum.TextAlignment = TextAlignment.Right;

            //Year.Blocks.Add(new Paragraph(new Run($"{BillingViewModel.StartDate.Year}")));
            //RoomName.Blocks.Add(new Paragraph(new Run(roomCostShareBilling.RoomName)));
            //Area.Blocks.Add(new Paragraph(new Run($"Area {roomCostShareBilling.RoomArea:N2} + {roomCostShareBilling.SharedAreaShare:N2}")));
            //AreaSum.Blocks.Add(new Paragraph(new Run($"{roomCostShareBilling.RentedAreaShare:N2}")) {Margin = new Thickness(0, 0, 10, 0)});

            //dataRow.Cells.Add(Year);
            //dataRow.Cells.Add(RoomName);
            //dataRow.Cells.Add(Area);
            //dataRow.Cells.Add(AreaSum);

            //dataRowGroup.Rows.Add(dataRow);

            TableRow tableRowFixedCostsShare = DataOutputTableRowRoomsBilling(
                    BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.FixedCostsAnnualCostsShare,
                    BillingViewModel.Billing.TotalFixedCostsPerPeriod.TransactionItem);

            dataRowGroup.Rows.Add(tableRowFixedCostsShare);

            TableRow tableRowHeatingCostsShare = DataOutputTableRowRoomsBilling(
                    BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.HeatingCostsAnnualCostsShare,
                    BillingViewModel.Billing.TotalHeatingCostsPerPeriod.TransactionItem);

            dataRowGroup.Rows.Add(tableRowHeatingCostsShare);

            if (OtherOutputSelected)
            {
                foreach (FinancialTransactionItemBillingViewModel item in roomCostShareBilling.FinancialTransactionItemViewModels)
                {
                    dataRowGroup.Rows.Add(DataOutputTableRowRoomsBilling(
                        BillingViewModel,
                        roomCostShareBilling.RoomName,
                        item.TransactionSum,
                        item.TransactionItem));
                }
            }

            billingTableRooms.RowGroups.Add(dataRowGroup);

            dataRowGroup.Rows.Add(DataOutputTableRowRoomsBilling(
                 BillingViewModel,
                 roomCostShareBilling.RoomName,
                 roomCostShareBilling.TotalCostsAnnualCostsShare,
                 "sum", true));

            return billingTableRooms;
        }


        private Block DataOutputConsumptionRooms(RoomCostShareBilling roomCostShareBilling)
        {
            Table billingTableRooms = DataOutputTableRoomsBilling();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            dataRowGroup.Rows.Add(DataOutputTableRowConsumptionRooms(
                BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.HeatingUnitsTotalConsumptionShare,
                BillingViewModel.Billing.ConsumptionItems[0].ConsumedUnits,
                BillingViewModel.Billing.TotalHeatingCostsPerPeriod.TransactionItem));

            if (OtherOutputSelected)
            {
                foreach (RoomConsumptionViewModel roomConsumptionViewModel in roomCostShareBilling.ConsumptionItemViewModels)
                {
                    if (roomConsumptionViewModel.RoomArea == roomCostShareBilling.RoomArea
                        && roomConsumptionViewModel.RoomName.Equals(roomCostShareBilling.RoomName))
                    {
                        dataRowGroup.Rows.Add(DataOutputTableRowConsumptionRooms(
                            BillingViewModel,
                            roomCostShareBilling.RoomName,
                            roomConsumptionViewModel.TotalConsumptionValue,
                            roomConsumptionViewModel.ConsumptionItemViewModel.ConsumedUnits,
                            roomConsumptionViewModel.ConsumptionCause));
                    }

                }
            }

            billingTableRooms.RowGroups.Add(dataRowGroup);

            return billingTableRooms;
        }


        private Table DataOutputTable()
        {

            Table dataOutputTable = new Table();

            TableColumn DateColumn = new TableColumn() { Width = new GridLength(100) };
            TableColumn ItemColumn = new TableColumn() { Width = new GridLength(250) };
            TableColumn CostColumn = new TableColumn() { Width = new GridLength(100) };

            dataOutputTable.Columns.Add(DateColumn);
            dataOutputTable.Columns.Add(ItemColumn);
            dataOutputTable.Columns.Add(CostColumn);

            return dataOutputTable;
        }


        private Table DataOutputTableRooms()
        {

            Table dataOutputTable = new Table();

            TableColumn RoomNameColumn = new TableColumn() { Width = new GridLength(150) };
            TableColumn DateColumn = new TableColumn() { Width = new GridLength(100) };
            TableColumn ItemColumn = new TableColumn() { Width = new GridLength(250) };
            TableColumn CostColumn = new TableColumn() { Width = new GridLength(100) };

            dataOutputTable.Columns.Add(RoomNameColumn);
            dataOutputTable.Columns.Add(DateColumn);
            dataOutputTable.Columns.Add(ItemColumn);
            dataOutputTable.Columns.Add(CostColumn);

            return dataOutputTable;
        }

        private Table DataOutputTableRoomsBilling()
        {
            Table dataOutputTable = new Table();

            TableColumn DateColumn = new TableColumn() { Width = new GridLength(100) };
            TableColumn RoomNameColumn = new TableColumn() { Width = new GridLength(150) };
            TableColumn ItemColumn = new TableColumn() { Width = new GridLength(200) };
            TableColumn CostColumn = new TableColumn() { Width = new GridLength(150) };

            dataOutputTable.Columns.Add(DateColumn);
            dataOutputTable.Columns.Add(RoomNameColumn);
            dataOutputTable.Columns.Add(ItemColumn);
            dataOutputTable.Columns.Add(CostColumn);

            return dataOutputTable;
        }


        private TableRow DataOutputTableRow(RentViewModel viewModel, double payment, string item, int month = -1, bool FontWeightBold = false)
        {
            TableRow dataRow = new TableRow();

            TableCell DueTime = new TableCell();
            DueTime.TextAlignment = TextAlignment.Right;

            TableCell Item = new TableCell();
            TableCell Payment = new TableCell();
            Payment.TextAlignment = TextAlignment.Right;

            if (month == -1 && viewModel.StartDate.Year == SelectedYear)
            {
                DueTime.Blocks.Add(new Paragraph(new Run($"> {viewModel.StartDate.Month}/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
            }
            else if (month == -1 && viewModel.StartDate.Year < SelectedYear)
            {
                DueTime.Blocks.Add(new Paragraph(new Run($"> 1/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
            }
            else
            {
                DueTime.Blocks.Add(new Paragraph(new Run($"{month}/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
            }

            Item.Blocks.Add(new Paragraph(new Run(item)));

            if (FontWeightBold)
            {
                Paragraph paymentParagraph = new Paragraph(new Run($"{payment:C2}"))
                {
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    BorderThickness = new Thickness(0, 1, 0, 0),
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 10, 0)
                };

                Payment.Blocks.Add(paymentParagraph);
            }
            else
            {
                Paragraph paymentParagraph = new Paragraph(new Run($"{payment:C2}"))
                {
                    Margin = new Thickness(0, 0, 10, 0)
                };

                Payment.Blocks.Add(paymentParagraph);
            }

            dataRow.Cells.Add(DueTime);
            dataRow.Cells.Add(Item);
            dataRow.Cells.Add(Payment);

            return dataRow;
        }


        private TableRow DataOutputTableRowBilling(BillingViewModel viewModel, double payment, string item, bool FontWeightBold = false)
        {
            TableRow dataRow = new TableRow();

            TableCell DueTime = new TableCell();
            DueTime.TextAlignment = TextAlignment.Right;

            TableCell Item = new TableCell();
            TableCell Payment = new TableCell();
            Payment.TextAlignment = TextAlignment.Right;

            DueTime.Blocks.Add(new Paragraph(new Run($"{viewModel.StartDate:d}-{viewModel.EndDate:d}")) { Margin = new Thickness(0, 0, 10, 0) });

            Item.Blocks.Add(new Paragraph(new Run(item)));

            if (FontWeightBold)
            {
                Paragraph paymentParagraph = new Paragraph(new Run($"{payment:C2}"))
                {
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    BorderThickness = new Thickness(0, 1, 0, 0),
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 10, 0)
                };

                Payment.Blocks.Add(paymentParagraph);
            }
            else
            {
                Paragraph paymentParagraph = new Paragraph(new Run($"{payment:C2}"))
                {
                    Margin = new Thickness(0, 0, 10, 0)
                };

                Payment.Blocks.Add(paymentParagraph);
            }

            dataRow.Cells.Add(DueTime);
            dataRow.Cells.Add(Item);
            dataRow.Cells.Add(Payment);

            return dataRow;
        }


        private TableRow DataOutputTableRowRooms(RentViewModel viewModel, string roomname, double payment, string item, int month = -1, bool FontWeightBold = false)
        {
            TableRow dataRow = new TableRow();

            TableCell RoomName = new TableCell();

            TableCell DueTime = new TableCell();
            DueTime.TextAlignment = TextAlignment.Right;

            TableCell Item = new TableCell();
            TableCell Payment = new TableCell();
            Payment.TextAlignment = TextAlignment.Right;

            RoomName.Blocks.Add(new Paragraph(new Run(roomname)));

            if (month == -1 && viewModel.StartDate.Year == SelectedYear)
            {
                DueTime.Blocks.Add(new Paragraph(new Run($"> {viewModel.StartDate.Month}/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
            }
            else if (month == -1 && viewModel.StartDate.Year < SelectedYear)
            {
                DueTime.Blocks.Add(new Paragraph(new Run($"> 1/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
            }
            else
            {
                DueTime.Blocks.Add(new Paragraph(new Run($"{month}/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
            }

            Item.Blocks.Add(new Paragraph(new Run(item)));

            if (FontWeightBold)
            {
                Paragraph paymentParagraph = new Paragraph(new Run($"{payment:C2}"))
                {
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    BorderThickness = new Thickness(0, 1, 0, 0),
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 10, 0)
                };

                Payment.Blocks.Add(paymentParagraph);
            }
            else
            {
                Paragraph paymentParagraph = new Paragraph(new Run($"{payment:C2}"))
                {
                    Margin = new Thickness(0, 0, 10, 0)
                };

                Payment.Blocks.Add(paymentParagraph);
            }

            dataRow.Cells.Add(RoomName);
            dataRow.Cells.Add(DueTime);
            dataRow.Cells.Add(Item);
            dataRow.Cells.Add(Payment);

            return dataRow;
        }


        private TableRow DataOutputTableRowConsumptionRooms(BillingViewModel viewModel, string roomname, double consumption, double totalConsumption, string item)
        {
            TableRow dataRow = new TableRow();

            TableCell Year = new TableCell();
            Year.TextAlignment = TextAlignment.Right;

            TableCell RoomName = new TableCell();
            TableCell Item = new TableCell();

            TableCell Consumption = new TableCell();
            Consumption.TextAlignment = TextAlignment.Right;

            Year.Blocks.Add(new Paragraph(new Run($"{viewModel.StartDate.Year}")));
            RoomName.Blocks.Add(new Paragraph(new Run(roomname)));
            Item.Blocks.Add(new Paragraph(new Run(item)));

            Paragraph paymentParagraph = new Paragraph(new Run($"{consumption:N2} ({consumption / totalConsumption * 100:N2}%)"))
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(0, 0, 10, 0)
            };

            Consumption.Blocks.Add(paymentParagraph);

            dataRow.Cells.Add(Year);
            dataRow.Cells.Add(RoomName);
            dataRow.Cells.Add(Item);
            dataRow.Cells.Add(Consumption);

            return dataRow;
        }

        private TableRow DataOutputTableRowRoomsBilling(BillingViewModel viewModel, string roomname, double payment, string item, bool FontWeightBold = false)
        {
            TableRow dataRow = new TableRow();

            TableCell Year = new TableCell();
            Year.TextAlignment = TextAlignment.Right;

            TableCell RoomName = new TableCell();
            TableCell Item = new TableCell();

            TableCell Payment = new TableCell();
            Payment.TextAlignment = TextAlignment.Right;

            Year.Blocks.Add(new Paragraph(new Run($"{viewModel.StartDate.Year}")));
            RoomName.Blocks.Add(new Paragraph(new Run(roomname)));
            Item.Blocks.Add(new Paragraph(new Run(item)));

            if (FontWeightBold)
            {
                Paragraph paymentParagraph = new Paragraph(new Run($"{payment:C2}"))
                {
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    BorderThickness = new Thickness(0, 1, 0, 0),
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 10, 0)
                };

                Payment.Blocks.Add(paymentParagraph);
            }
            else
            {
                Paragraph paymentParagraph = new Paragraph(new Run($"{payment:C2}"))
                {
                    Margin = new Thickness(0, 0, 10, 0)
                };

                Payment.Blocks.Add(paymentParagraph);
            }

            dataRow.Cells.Add(Year);
            dataRow.Cells.Add(RoomName);
            dataRow.Cells.Add(Item);
            dataRow.Cells.Add(Payment);

            return dataRow;
        }


        private void FillDetailOptions()
        {
            DetailOptions.Add("TimeScale");
            DetailOptions.Add("ValueChange");

            SelectedDetailOption = "ValueChange";

            OnPropertyChanged(nameof(SelectedDetailOption));
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
                    if (rent.StartDate < endDate)
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


        private Block OtherPlanFlatTable(RentViewModel rentViewModel, int monthCounter = -1)
        {
            Table otherPlanFlatTable = DataOutputTable();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;


            foreach (FinancialTransactionItemRentViewModel item in rentViewModel.FinancialTransactionItemViewModels)
            {
                dataRowGroup.Rows.Add(DataOutputTableRow(rentViewModel, item.TransactionSum, item.TransactionItem, monthCounter));
            }

            dataRowGroup.Rows.Add(DataOutputTableRow(rentViewModel, rentViewModel.OtherFTISum, "sum", monthCounter, true));

            otherPlanFlatTable.RowGroups.Add(dataRowGroup);

            return otherPlanFlatTable;
        }


        private Block RentPlanFlatTable(RentViewModel viewModel, int month = -1)
        {
            Table rentPlanFlatTable = DataOutputTable();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            dataRowGroup.Rows.Add(DataOutputTableRow(viewModel, viewModel.ColdRent, viewModel.Rent.ColdRent.TransactionItem, month));
            dataRowGroup.Rows.Add(DataOutputTableRow(viewModel, viewModel.FixedCostsAdvance, viewModel.Rent.FixedCostsAdvance.TransactionItem, month));
            dataRowGroup.Rows.Add(DataOutputTableRow(viewModel, viewModel.HeatingCostsAdvance, viewModel.Rent.HeatingCostsAdvance.TransactionItem, month));

            if (OtherOutputSelected)
            {
                dataRowGroup.Rows.Add(DataOutputTableRow(viewModel, viewModel.OtherFTISum, "other", month));
            }

            if (CreditOutputSelected)
            {
                dataRowGroup.Rows.Add(DataOutputTableRow(viewModel, -1 * viewModel.CreditSum, "credit", month));
            }

            if (OtherOutputSelected || CreditOutputSelected)
            {
                if (OtherOutputSelected && !CreditOutputSelected)
                {
                    dataRowGroup.Rows.Add(DataOutputTableRow(viewModel, viewModel.CompleteCosts, "sum", month, true));
                }

                if (!OtherOutputSelected && CreditOutputSelected)
                {
                    double reducedsum = viewModel.CostsTotal - viewModel.CreditSum;

                    dataRowGroup.Rows.Add(DataOutputTableRow(viewModel, reducedsum, "sum", month, true));
                }

                if (OtherOutputSelected && CreditOutputSelected)
                {
                    double reducedsum = viewModel.CompleteCosts - viewModel.CreditSum;

                    dataRowGroup.Rows.Add(DataOutputTableRow(viewModel, reducedsum, "sum", month, true));
                }
            }
            else
            {
                dataRowGroup.Rows.Add(DataOutputTableRow(viewModel, viewModel.CostsTotal, "sum", month, true));
            }

            rentPlanFlatTable.RowGroups.Add(dataRowGroup);

            return rentPlanFlatTable;
        }


        private Block RentPlanRoomsTable(RentViewModel viewModel, int month = -1)
        {
            Table rentPlanRoomsTable = DataOutputTableRooms();

            rentPlanRoomsTable.RowGroups.Add(TableRowGroupRoomHeader());

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            foreach (RoomCostShareRent item in viewModel.RoomCostShares)
            {

                dataRowGroup.Rows.Add(DataOutputTableRowRooms(viewModel, item.RoomName, item.RentShare, viewModel.Rent.ColdRent.TransactionItem, month));

                dataRowGroup.Rows.Add(DataOutputTableRowRooms(viewModel, item.RoomName, item.FixedCostsAdvanceShare, viewModel.Rent.FixedCostsAdvance.TransactionItem, month));

                dataRowGroup.Rows.Add(DataOutputTableRowRooms(viewModel, item.RoomName, item.HeatingCostsAdvanceShare, viewModel.Rent.HeatingCostsAdvance.TransactionItem, month));


                if (OtherOutputSelected)
                {
                    dataRowGroup.Rows.Add(DataOutputTableRowRooms(viewModel, item.RoomName, item.OtherCostsShare, "other", month));
                }

                if (CreditOutputSelected)
                {
                    dataRowGroup.Rows.Add(DataOutputTableRowRooms(viewModel, item.RoomName, -101.01, "credit", month));
                }

                if (OtherOutputSelected || CreditOutputSelected)
                {
                    if (OtherOutputSelected && !CreditOutputSelected)
                    {
                        dataRowGroup.Rows.Add(DataOutputTableRowRooms(viewModel, item.RoomName, item.CompleteCostShare, "sum", month, true));
                    }

                    if (!OtherOutputSelected && CreditOutputSelected)
                    {
                        double reducedsum = item.PriceShare - 101.01;

                        dataRowGroup.Rows.Add(DataOutputTableRowRooms(viewModel, item.RoomName, reducedsum, "sum", month, true));
                    }

                    if (OtherOutputSelected && CreditOutputSelected)
                    {
                        double reducedsum = item.CompleteCostShare - 101.01;

                        dataRowGroup.Rows.Add(DataOutputTableRowRooms(viewModel, item.RoomName, reducedsum, "sum", month, true));
                    }
                }
                else
                {
                    dataRowGroup.Rows.Add(DataOutputTableRowRooms(viewModel, item.RoomName, item.PriceShare, "sum", month, true));
                }


                TableRow tableRow = new TableRow();
                tableRow.Cells.Add(new TableCell(new Paragraph()));

                dataRowGroup.Rows.Add(tableRow);
            }

            rentPlanRoomsTable.RowGroups.Add(dataRowGroup);

            return rentPlanRoomsTable;
        }


        private TableRowGroup TableRowGroupRentHeader()
        {
            TableRowGroup headerRowGroup = new TableRowGroup();

            headerRowGroup.Style = Application.Current.FindResource("HeaderRowStyle") as Style;

            headerRowGroup.FontSize = 14;

            TableRow headerRow = new TableRow();

            TableCell headerCell_DueTime = new TableCell();
            TableCell headerCell_Item = new TableCell();
            TableCell headerCell_Costs = new TableCell();

            headerCell_DueTime.Blocks.Add(new Paragraph(new Run("Time")));
            headerCell_Item.Blocks.Add(new Paragraph(new Run("Item")));
            headerCell_Costs.Blocks.Add(new Paragraph(new Run("Costs")));

            headerRow.Cells.Add(headerCell_DueTime);
            headerRow.Cells.Add(headerCell_Item);
            headerRow.Cells.Add(headerCell_Costs);

            headerRowGroup.Rows.Add(headerRow);

            return headerRowGroup;
        }


        private TableRowGroup TableRowGroupRoomHeader()
        {
            TableRowGroup headerRowGroup = new TableRowGroup();

            headerRowGroup.Style = Application.Current.FindResource("HeaderRowStyle") as Style;

            headerRowGroup.FontSize = 14;

            TableRow headerRow = new TableRow();

            TableCell RoomName = new TableCell();
            TableCell DueTime = new TableCell();
            TableCell Item = new TableCell();
            TableCell Costs = new TableCell();

            RoomName.Blocks.Add(new Paragraph(new Run("Room")));
            DueTime.Blocks.Add(new Paragraph(new Run("Time")));
            Item.Blocks.Add(new Paragraph(new Run("Item")));
            Costs.Blocks.Add(new Paragraph(new Run("Costs")));

            headerRow.Cells.Add(RoomName);
            headerRow.Cells.Add(DueTime);
            headerRow.Cells.Add(Item);
            headerRow.Cells.Add(Costs);

            headerRowGroup.Rows.Add(headerRow);

            return headerRowGroup;
        }


        public void Update()
        {
            if (_AccountingViewModel.FlatViewModel != null)
            {
                _FlatViewModel = _AccountingViewModel.FlatViewModel;

                BuildTimeScale();

                BuildFlowDocument();
            }

            OnPropertyChanged(nameof(BillingViewModel));
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