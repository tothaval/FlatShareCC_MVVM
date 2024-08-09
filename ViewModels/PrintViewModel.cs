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
using static System.Net.WebRequestMethods;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Utility.PrintViewHelperFunctions;

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

                SearchForBillingViewModel();
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
                _FlatManagementViewModel.PropertyChanged += _FlatManagementViewModel_PropertyChanged;
            }

            _AccountingViewModel = flatManagementViewModel.Accounting;

            _AccountingViewModel.AccountingChanged += _AccountingViewModel_AccountingChanged;

            CreatePrintOutputCommand = new RelayCommand((s) => BuildFlowDocument(), (s) => true);

            FillDetailOptionsComboBoxItemSource();

            Update();
        }

        #endregion


        // methods
        #region methods




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

            SearchForBillingViewModel();

            if (BillingOutputSelected && BillingViewModel != null)
            {
                p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };
                ActiveFlowDocument.Blocks.Add(p);

                p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };

                p.Inlines.Add(new Run($"Annual Billing {SelectedYear}: ")
                { FontWeight = FontWeights.Bold, FontSize = 16.0 });
                p.Inlines.Add(new Run($"{BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
                //p.Style = headerParagraph;
                ActiveFlowDocument.Blocks.Add(p);

                ActiveFlowDocument.Blocks.Add(new BillingPrintOutput(BillingViewModel).GetBillingDetails);
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

            if (OtherOutputSelected)
            {
                p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };
                ActiveFlowDocument.Blocks.Add(p);

                p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };
                p.Inlines.Add(new Run($"Other Costs Plan {SelectedYear}: ")
                { FontWeight = FontWeights.Bold, FontSize = 16.0 });
                p.Inlines.Add(new Run($"{BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
                //p.Style = headerParagraph;
                ActiveFlowDocument.Blocks.Add(p);

                ActiveFlowDocument.Blocks.Add(BuildOtherCostDetails(false));
            }

            if (CreditOutputSelected)
            {
                p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };
                ActiveFlowDocument.Blocks.Add(p);

                p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };
                p.Inlines.Add(new Run($"Credit Plan {SelectedYear}: ")
                { FontWeight = FontWeights.Bold, FontSize = 16.0 });
                p.Inlines.Add(new Run($"{BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
                //p.Style = headerParagraph;
                ActiveFlowDocument.Blocks.Add(p);

                ActiveFlowDocument.Blocks.Add(BuildOtherCostDetails(true));
            }
        }


        private Section BuildOtherCostDetails(bool isCredit)
        {
            Section rentOutput = new Section();

            ObservableCollection<RentViewModel> RentList = FindRelevantRentViewModels();

            for (int i = 0; i < RentList.Count; i++)
            {

                if (isCredit)
                {
                    if (!RentList[i].HasCredits)
                    {
                        continue;
                    }
                }
                else
                {
                    if (!RentList[i].HasOtherCosts)
                    {
                        continue;
                    }
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


                Table headerTable = new PrintOutputBase().GetOutputTableForFlat;
                headerTable.RowGroups.Add(TableRowGroupRentHeader());

                rentOutput.Blocks.Add(headerTable);

                if (SelectedDetailOption.Equals("TimeScale"))
                {
                    for (int monthCounter = 1; monthCounter < 13; monthCounter++)
                    {
                        if (isCredit)
                        {
                            if (!RentList[i].HasCredits)
                            {
                                rentOutput.Blocks.Add(new Paragraph(new Run($"end:\t\t\t{RentList[i].StartDate - TimeSpan.FromDays(1):d}")));
                                continue;
                            }
                        }
                        else
                        {
                            if (!RentList[i].HasOtherCosts)
                            {
                                rentOutput.Blocks.Add(new Paragraph(new Run($"end:\t\t\t{RentList[i].StartDate - TimeSpan.FromDays(1):d}")));
                                continue;
                            }
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
                                rentOutput.Blocks.Add(BuildNewPlanTable(rentOutput, RentList[i], monthCounter, isCredit));
                            }
                            else if (RentList[i].StartDate.Year == SelectedYear
                                && monthCounter >= RentList[i].StartDate.Month)
                            {
                                rentOutput.Blocks.Add(BuildNewPlanTable(rentOutput, RentList[i], monthCounter, isCredit));
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
                                rentOutput.Blocks.Add(BuildNewPlanTable(rentOutput, RentList[i], monthCounter, isCredit));
                            }

                        }

                    }
                }
                else
                {
                    rentOutput.Blocks.Add(BuildNewPlanTable(rentOutput, RentList[i], -1, isCredit));
                }


                if (i + 1 < RentList.Count && RentList[i + 1].HasOtherCosts == false
                    || i + 1 < RentList.Count && RentList[i + 1].HasCredits == false)
                {
                    rentOutput.Blocks.Add(new Paragraph(new Run($"rent end:\t\t{RentList[i + 1].StartDate - TimeSpan.FromDays(1):d}")));

                }
            }
            
            rentOutput.Blocks.Add(BuildRoomDetailsOther(isCredit));
     
            return rentOutput;
        }


        private Section BuildNewPlanTable(Section rentOutput, RentViewModel rentViewModel, int monthCounter, bool isCredit = false)
        {
            if (isCredit)
            {
                rentOutput.Blocks.Add(CostsAndCreditsPlanTable(rentViewModel, rentViewModel.Credits, monthCounter));
            }
            else
            {
                rentOutput.Blocks.Add(CostsAndCreditsPlanTable(rentViewModel, rentViewModel.FinancialTransactionItemViewModels, monthCounter));
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


                    Table headerTable = new PrintOutputBase().GetOutputTableForFlat;
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
                                    rentOutput.Blocks.Add(RentPlanTable(RentList[i], monthCounter));
                                }
                                else if (RentList[i].StartDate.Year == SelectedYear
                                    && monthCounter >= RentList[i].StartDate.Month)
                                {
                                    rentOutput.Blocks.Add(RentPlanTable(RentList[i], monthCounter));
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
                                    rentOutput.Blocks.Add(RentPlanTable(RentList[i], monthCounter));
                                }

                            }

                        }
                    }
                    else
                    {
                        rentOutput.Blocks.Add(RentPlanTable(RentList[i]));

                    }

                }

            }

            rentOutput.Blocks.Add(BuildRoomDetails());


            return rentOutput;
        }


        private Section BuildRoomDetails()
        {
            Section roomsOutput = new Section();

            Paragraph p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };

            roomsOutput.Blocks.Add(p);

            p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };
            p.Inlines.Add(new Run($"Rent Plan Rooms {SelectedYear}: ")
            { FontWeight = FontWeights.Bold, FontSize = 16.0 });
            p.Inlines.Add(new Run($"{BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
            //p.Style = headerParagraph;
            roomsOutput.Blocks.Add(p);

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
                                roomsOutput.Blocks.Add(RentPlanTableRooms(RentList[i], monthCounter));
                            }
                            else if (RentList[i].StartDate.Year == SelectedYear
                                && monthCounter >= RentList[i].StartDate.Month)
                            {
                                roomsOutput.Blocks.Add(RentPlanTableRooms(RentList[i], monthCounter));
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
                                roomsOutput.Blocks.Add(RentPlanTableRooms(RentList[i], monthCounter));
                            }

                        }

                    }
                }
                else
                {
                    roomsOutput.Blocks.Add(RentPlanTableRooms(RentList[i]));

                }

            }

            return roomsOutput;
        }


        private Section BuildRoomDetailsOther(bool isCredit)
        {
            Section roomsOutput = new Section();

            Paragraph p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };

            roomsOutput.Blocks.Add(p);

            if (isCredit)
            {
                p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };
                p.Inlines.Add(new Run($"Credit Plan Rooms {SelectedYear}: ")
                { FontWeight = FontWeights.Bold, FontSize = 16.0 });
            }
            else
            {
                p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };
                p.Inlines.Add(new Run($"Other Costs Plan Rooms {SelectedYear}: ")
                { FontWeight = FontWeights.Bold, FontSize = 16.0 });
            }

            p.Inlines.Add(new Run($"{BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
            roomsOutput.Blocks.Add(p);

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
                                roomsOutput.Blocks.Add(CreditsAndOtherCostsPlanTableRooms(RentList[i], isCredit, monthCounter));
                            }
                            else if (RentList[i].StartDate.Year == SelectedYear
                                && monthCounter >= RentList[i].StartDate.Month)
                            {
                                roomsOutput.Blocks.Add(CreditsAndOtherCostsPlanTableRooms(RentList[i], isCredit, monthCounter));
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
                                roomsOutput.Blocks.Add(CreditsAndOtherCostsPlanTableRooms(RentList[i], isCredit, monthCounter));
                            }

                        }

                    }
                }
                else
                {
                    roomsOutput.Blocks.Add(CreditsAndOtherCostsPlanTableRooms(RentList[i], isCredit));
                }

            }

            return roomsOutput;
        }
        

        private Block CreditsAndOtherCostsPlanTableRooms(RentViewModel rentViewModel, bool isCredit, int monthCounter = -1)
        {
            Section output = new Section();

            if (isCredit)
            {
                output.Blocks.Add(OtherCostsPlanTableRooms(rentViewModel, rentViewModel.Credits, monthCounter));
            }
            else
            {
                output.Blocks.Add(OtherCostsPlanTableRooms(rentViewModel, rentViewModel.FinancialTransactionItemViewModels, monthCounter));
            }

            return output;
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

            foreach (BillingViewModel item in _FlatViewModel.AnnualBillings)
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


        private Block CostsAndCreditsPlanTable(RentViewModel rentViewModel, ObservableCollection<IFinancialTransactionItem> FTIs, int monthCounter = -1)
        {
            Table otherPlanFlatTable = new PrintOutputBase().GetOutputTableForFlat;

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            double result = 0.0;

            foreach (FinancialTransactionItemRentViewModel fti in FTIs)
            {
                if (fti.Duration == Enums.TransactionDurationTypes.Ongoing)
                {
                    dataRowGroup.Rows.Add(OutputTableRow(rentViewModel, fti.TransactionSum, fti.TransactionItem, monthCounter));

                    result += fti.TransactionSum;
                }
                else
                {
                    if (fti.StartDate.Year == SelectedYear && fti.StartDate.Month + fti.Rates > 12)
                    {
                        dataRowGroup.Rows.Add(OutputTableRow(rentViewModel, fti.TransactionSum, fti.TransactionItem, monthCounter));

                        result += fti.TransactionSum;
                    }
                    else if (fti.StartDate.Year == SelectedYear && fti.StartDate.Month + fti.Rates <= 12)
                    {
                        if (monthCounter < fti.StartDate.Month)
                        {
                            continue;
                        }

                        if (fti.StartDate.Month == monthCounter)
                        {
                            dataRowGroup.Rows.Add(OutputTableRow(rentViewModel, fti.TransactionSum, fti.TransactionItem, monthCounter));

                            result += fti.TransactionSum;
                        }

                        if (monthCounter > fti.StartDate.Month && monthCounter < fti.StartDate.Month + fti.Rates)
                        {
                            dataRowGroup.Rows.Add(OutputTableRow(rentViewModel, fti.TransactionSum, fti.TransactionItem, monthCounter));

                            result += fti.TransactionSum;
                        }
                    }
                }
            }

            dataRowGroup.Rows.Add(OutputTableRow(rentViewModel, result, "sum", monthCounter, true));

            otherPlanFlatTable.RowGroups.Add(dataRowGroup);

            return otherPlanFlatTable;
        }


        private Table OutputTableRooms()
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


        private TableRow OutputTableRow(RentViewModel viewModel, double payment, string item, int month = -1, bool FontWeightBold = false)
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


        private TableRow OutputTableRowRooms(RentViewModel viewModel, string roomname, double payment, string item, int month = -1, bool FontWeightBold = false)
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
                Paragraph paymentParagraph = new Paragraph(new Run($"{payment:C2}\n"))
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


        private void FillDetailOptionsComboBoxItemSource()
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


        private Block OtherCostsPlanTableRooms(RentViewModel rentViewModel, ObservableCollection<IFinancialTransactionItem> FTIs, int month = -1)
        {
            Table rentPlanRoomsTable = OutputTableRooms();

            rentPlanRoomsTable.RowGroups.Add(TableRowGroupRoomHeader());

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            foreach (RoomCostShareRent item in rentViewModel.RoomCostShares)
            {
                double result = 0.0;

                foreach (FinancialTransactionItemRentViewModel fti in FTIs)
                {
                    double sum = 0.0;

                    if (fti.CostShareTypes == Enums.TransactionShareTypesRent.Equal)
                    {
                        sum = item.EqualShareRatio() * fti.TransactionSum;
                    }
                    else if (fti.CostShareTypes == Enums.TransactionShareTypesRent.Area)
                    {
                        sum = item.RentedAreaShareRatio() * fti.TransactionSum;
                    }

                    if (fti.Duration == Enums.TransactionDurationTypes.Ongoing)
                    {
                        dataRowGroup.Rows.Add(OutputTableRowRooms(rentViewModel, item.RoomName, sum, fti.TransactionItem, month));

                        result += sum;
                    }
                    else
                    {
                        if (fti.StartDate.Year == SelectedYear && fti.StartDate.Month + fti.Rates > 12)
                        {
                            dataRowGroup.Rows.Add(OutputTableRowRooms(rentViewModel, item.RoomName, sum, fti.TransactionItem, month));

                            result += sum;
                        }
                        else if (fti.StartDate.Year == SelectedYear && fti.StartDate.Month + fti.Rates <= 12)
                        {
                            if (month < fti.StartDate.Month)
                            {
                                continue;
                            }

                            if (fti.StartDate.Month == month)
                            {
                                dataRowGroup.Rows.Add(OutputTableRowRooms(rentViewModel, item.RoomName, sum, fti.TransactionItem, month));

                                result += sum;
                            }

                            if (month > fti.StartDate.Month && month < fti.StartDate.Month + fti.Rates)
                            {
                                dataRowGroup.Rows.Add(OutputTableRowRooms(rentViewModel, item.RoomName, sum, fti.TransactionItem, month));

                                result += sum;
                            }
                        }
                    }
                }

                dataRowGroup.Rows.Add(OutputTableRowRooms(rentViewModel, item.RoomName, result, "sum", month, true));

            }


            rentPlanRoomsTable.RowGroups.Add(dataRowGroup);

            return rentPlanRoomsTable;
        }


        private Block RentPlanTable(RentViewModel viewModel, int month = -1)
        {
            Table rentPlanFlatTable = new PrintOutputBase().GetOutputTableForFlat;

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            dataRowGroup.Rows.Add(OutputTableRow(viewModel, viewModel.ColdRent, viewModel.Rent.ColdRent.TransactionItem, month));
            dataRowGroup.Rows.Add(OutputTableRow(viewModel, viewModel.FixedCostsAdvance, viewModel.Rent.FixedCostsAdvance.TransactionItem, month));
            dataRowGroup.Rows.Add(OutputTableRow(viewModel, viewModel.HeatingCostsAdvance, viewModel.Rent.HeatingCostsAdvance.TransactionItem, month));

            if (OtherOutputSelected)
            {
                dataRowGroup.Rows.Add(OutputTableRow(viewModel, viewModel.OtherFTISum, "other", month));
            }

            if (CreditOutputSelected)
            {
                dataRowGroup.Rows.Add(OutputTableRow(viewModel, -1 * viewModel.CreditSum, "credit", month));
            }

            if (OtherOutputSelected || CreditOutputSelected)
            {
                if (OtherOutputSelected && !CreditOutputSelected)
                {
                    dataRowGroup.Rows.Add(OutputTableRow(viewModel, viewModel.CompleteCosts, "sum", month, true));
                }

                if (!OtherOutputSelected && CreditOutputSelected)
                {
                    double reducedsum = viewModel.CostsTotal - viewModel.CreditSum;

                    dataRowGroup.Rows.Add(OutputTableRow(viewModel, reducedsum, "sum", month, true));
                }

                if (OtherOutputSelected && CreditOutputSelected)
                {
                    double reducedsum = viewModel.CompleteCosts - viewModel.CreditSum;

                    dataRowGroup.Rows.Add(OutputTableRow(viewModel, reducedsum, "sum", month, true));
                }
            }
            else
            {
                dataRowGroup.Rows.Add(OutputTableRow(viewModel, viewModel.CostsTotal, "sum", month, true));
            }

            rentPlanFlatTable.RowGroups.Add(dataRowGroup);

            return rentPlanFlatTable;
        }


        private Block RentPlanTableRooms(RentViewModel viewModel, int month = -1)
        {
            Table rentPlanRoomsTable = OutputTableRooms();

            rentPlanRoomsTable.RowGroups.Add(TableRowGroupRoomHeader());

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            foreach (RoomCostShareRent item in viewModel.RoomCostShares)
            {
                Paragraph newSegment = new Paragraph(new Run($"{item.RoomName} {item.RoomArea}m²"))
                { Background = new SolidColorBrush(Colors.LightGray) };

                TableRow roomSeparatorTableRow = new TableRow();

                roomSeparatorTableRow.Cells.Add(new TableCell(newSegment) { ColumnSpan = 4, Background = new SolidColorBrush(Colors.LightGray) });
                
                dataRowGroup.Rows.Add(roomSeparatorTableRow);

                dataRowGroup.Rows.Add(OutputTableRowRooms(viewModel, item.RoomName, item.RentShare, viewModel.Rent.ColdRent.TransactionItem, month));

                dataRowGroup.Rows.Add(OutputTableRowRooms(viewModel, item.RoomName, item.FixedCostsAdvanceShare, viewModel.Rent.FixedCostsAdvance.TransactionItem, month));

                dataRowGroup.Rows.Add(OutputTableRowRooms(viewModel, item.RoomName, item.HeatingCostsAdvanceShare, viewModel.Rent.HeatingCostsAdvance.TransactionItem, month));


                if (OtherOutputSelected)
                {
                    dataRowGroup.Rows.Add(OutputTableRowRooms(viewModel, item.RoomName, item.OtherCostsShare, "other", month));
                }

                if (CreditOutputSelected)
                {
                    dataRowGroup.Rows.Add(OutputTableRowRooms(viewModel, item.RoomName, -101.01, "credit", month));
                }

                if (OtherOutputSelected || CreditOutputSelected)
                {
                    if (OtherOutputSelected && !CreditOutputSelected)
                    {
                        dataRowGroup.Rows.Add(OutputTableRowRooms(viewModel, item.RoomName, item.CompleteCostShare, "sum", month, true));
                    }

                    if (!OtherOutputSelected && CreditOutputSelected)
                    {
                        double reducedsum = item.PriceShare - 101.01;

                        dataRowGroup.Rows.Add(OutputTableRowRooms(viewModel, item.RoomName, reducedsum, "sum", month, true));
                    }

                    if (OtherOutputSelected && CreditOutputSelected)
                    {
                        double reducedsum = item.CompleteCostShare - 101.01;

                        dataRowGroup.Rows.Add(OutputTableRowRooms(viewModel, item.RoomName, reducedsum, "sum", month, true));
                    }
                }
                else
                {
                    dataRowGroup.Rows.Add(OutputTableRowRooms(viewModel, item.RoomName, item.PriceShare, "sum", month, true));
                }


                TableRow tableRow = new TableRow();
                tableRow.Cells.Add(new TableCell(new Paragraph()));

                dataRowGroup.Rows.Add(tableRow);
            }

            rentPlanRoomsTable.RowGroups.Add(dataRowGroup);

            return rentPlanRoomsTable;
        }


        private void SearchForBillingViewModel()
        {
            BillingViewModel = null;

            foreach (BillingViewModel billingViewModel in FlatViewModel.AnnualBillings)
            {
                if (billingViewModel.Year == _SelectedYear)
                {
                    BillingViewModel = billingViewModel;
                    break;
                }

            }
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

            if (_FlatManagementViewModel.SelectedItem != null)
            {
                _FlatViewModel = _FlatManagementViewModel.SelectedItem;
                _AccountingViewModel.FlatManagement.SelectedItem.PropertyChanged += SelectedItem_PropertyChanged;
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


        private void _FlatManagementViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
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