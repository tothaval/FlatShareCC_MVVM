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
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using SharedLivingCostCalculator.Commands;
using System.Collections.ObjectModel;
using SharedLivingCostCalculator.Models.Financial;

namespace SharedLivingCostCalculator.ViewModels
{
    public class PrintViewModel : BaseViewModel
    {

        // Properties & Fields
        #region Properties & Fields

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

        private Section BuildAddressDetails(Style headerParagraph, Style textParagraph)
        {
            Section section = new Section();

            string ReportHeader = "";
            string rooms = "";

            try
            {
                ReportHeader = Application.Current.FindResource("IDF_Address").ToString();
                rooms = Application.Current.Resources["IDF_Rooms"].ToString();
            }
            catch (Exception)
            {
            }

            Paragraph p = new Paragraph(new Run(ReportHeader));
            p.Style = headerParagraph;
            section.Blocks.Add(p);

            p = new Paragraph(new Run(FlatViewModel.Address));
            p.Style = textParagraph;
            section.Blocks.Add(p);

            p = new Paragraph(new Run($"{_FlatViewModel.Area}m², {_FlatViewModel.RoomCount} {rooms}"));
            p.Style = textParagraph;
            section.Blocks.Add(p);

            return section;
        }


        private string BuildCreditDetails()
        {
            string rentOutput = string.Empty;

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
                    }


                    rentOutput += $"rent begin:\t\t{RentList[i].StartDate:d}";



                    if (SelectedDetailOption.Equals("TimeScale"))
                    {
                        for (int monthCounter = 1; monthCounter < 13; monthCounter++)
                        {
                            // if StartDate.Year < SelectedYear
                            //&& RentList[i + 1].StartDate.Year != RentList[i].StartDate.Year
                            //&& monthCounter >= RentList[i].StartDate.Month

                            if (i + 1 < RentList.Count)
                            {

                                // später im Ablauf, um Nachfolger zu berücksichtigen

                                if (RentList[i + 1].StartDate.Month - 1 < monthCounter)
                                {
                                    break;
                                }

                                if (RentList[i].StartDate.Year < SelectedYear)
                                {
                                    rentOutput += $"{monthCounter:00}/{SelectedYear} sum\t\t{RentList[i].CreditSum:C2}\n" +
                                        $"------------------------------------------\n";

                                    foreach (FinancialTransactionItemRentViewModel item in RentList[i].Credits)
                                    {
                                        rentOutput += $"{monthCounter:00}/{SelectedYear} {item.TransactionItem}\t\t{item.TransactionSum:C2}\n";
                                    }

                                    rentOutput += "\n\n\n";

                                }
                                else if (RentList[i].StartDate.Year == SelectedYear
                                    && monthCounter >= RentList[i].StartDate.Month)
                                {
                                    rentOutput += $"{monthCounter:00}/{SelectedYear} sum\t\t{RentList[i].CreditSum:C2}\n" +
                                                         $"------------------------------------------\n";

                                    foreach (FinancialTransactionItemRentViewModel item in RentList[i].Credits)
                                    {
                                        rentOutput += $"{monthCounter:00}/{SelectedYear} {item.TransactionItem}\t\t{item.TransactionSum:C2}\n";
                                    }

                                    rentOutput += "\n\n\n";
                                }


                            }
                            else
                            {
                                if (!RentList[i].HasCredits)
                                {
                                    rentOutput += $"end:\t\t\t{RentList[i].StartDate - TimeSpan.FromDays(1):d}\n\n";
                                    continue;
                                }

                                if (RentList[i].StartDate.Year == SelectedYear && monthCounter < RentList[i].StartDate.Month)
                                {
                                    continue;
                                }
                                else
                                {
                                    rentOutput += $"{monthCounter:00}/{SelectedYear} sum\t\t{RentList[i].CreditSum:C2}\n" +
                                                         $"------------------------------------------\n";

                                    foreach (FinancialTransactionItemRentViewModel item in RentList[i].Credits)
                                    {
                                        rentOutput += $"{monthCounter:00}/{SelectedYear} {item.TransactionItem}\t\t{-1 * item.TransactionSum:C2}\n";
                                    }

                                    rentOutput += "\n\n\n";
                                }

                            }
                        }


                        if (i + 1 < RentList.Count && RentList[i + 1].HasCredits == false)
                        {
                            rentOutput += $"rent end:\t\t\t{RentList[i + 1].StartDate - TimeSpan.FromDays(1):d}\n\n";
                        }

                    }
                    else
                    {


                        rentOutput += $"{SelectedYear} price\t\t{RentList[i].CreditSum:C2}\n" +
                                             $"------------------------------------------\n";

                        foreach (FinancialTransactionItemRentViewModel item in RentList[i].Credits)
                        {
                            rentOutput += $"{SelectedYear} {item.TransactionItem}\t\t{item.TransactionSum:C2}\n";
                        }

                        rentOutput += "\n\n";

                        if (i + 1 < RentList.Count && RentList[i + 1].HasCredits == false)
                        {
                            rentOutput += $"rent end:\t\t{RentList[i + 1].StartDate - TimeSpan.FromDays(1):d}\n";

                            rentOutput += "\n\n";
                        }

                    }
                }
            }

            return rentOutput;
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

            ActiveFlowDocument.Blocks.Add(BuildAddressDetails(headerParagraph, textParagraph));

            if (RentOutputSelected)
            {
                Paragraph p = new Paragraph();
                ActiveFlowDocument.Blocks.Add(p);

                p = new Paragraph(new Run($"Rent Plan Flat {SelectedYear}")) { Margin = new Thickness(0, 20, 0, 20) };
                p.Style = headerParagraph;
                ActiveFlowDocument.Blocks.Add(p);

                ActiveFlowDocument.Blocks.Add(BuildRentDetails());
            }

            if (RoomsOutputSelected)
            {
                Paragraph p = new Paragraph();
                ActiveFlowDocument.Blocks.Add(p);

                p = new Paragraph(new Run($"Rent Plan Rooms {SelectedYear}")) { Margin = new Thickness(0, 20, 0, 20) };
                p.Style = headerParagraph;
                ActiveFlowDocument.Blocks.Add(p);

                ActiveFlowDocument.Blocks.Add(BuildRoomDetails());
            }

            if (OtherOutputSelected)
            {
                Paragraph p = new Paragraph(new Run($"Other Costs Plan Flat {SelectedYear}"));
                p.Style = headerParagraph;
                ActiveFlowDocument.Blocks.Add(p);

                p = new Paragraph(new Run(BuildOtherCostDetails()));
                p.Style = textParagraph;
                ActiveFlowDocument.Blocks.Add(p);
            }

            if (CreditOutputSelected)
            {
                Paragraph p = new Paragraph(new Run($"Credit Plan Flat {SelectedYear}"));
                p.Style = headerParagraph;
                ActiveFlowDocument.Blocks.Add(p);

                p = new Paragraph(new Run(BuildCreditDetails()));
                p.Style = textParagraph;
                ActiveFlowDocument.Blocks.Add(p);
            }


        }


        private Block RentPlanFlatTable(RentViewModel viewModel, int month = -1)
        {
            Table rentPlanFlatTable = new Table();

            TableColumn DateColumn = new TableColumn() { Width = new GridLength(100) };
            TableColumn ItemColumn = new TableColumn() { Width = new GridLength(300) };
            TableColumn CostColumn = new TableColumn() { Width = new GridLength(100) };

            rentPlanFlatTable.Columns.Add(DateColumn);
            rentPlanFlatTable.Columns.Add(ItemColumn);
            rentPlanFlatTable.Columns.Add(CostColumn);

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            TableRow dataRow_rent = new TableRow();
            TableRow dataRow_fixed = new TableRow();
            TableRow dataRow_heating = new TableRow();
            TableRow dataRow_other = new TableRow();
            TableRow dataRow_credits = new TableRow();
            TableRow dataRow_sum = new TableRow();

            TableCell dataCell_rentDueTime = new TableCell();
            dataCell_rentDueTime.TextAlignment = TextAlignment.Right;

            TableCell dataCell_rentItem = new TableCell();

            TableCell dataCell_rentPayment = new TableCell();
            dataCell_rentPayment.TextAlignment = TextAlignment.Right;

            TableCell dataCell_fixedDueTime = new TableCell();
            dataCell_fixedDueTime.TextAlignment = TextAlignment.Right;

            TableCell dataCell_fixedItem = new TableCell();
            TableCell dataCell_fixedPayment = new TableCell();
            dataCell_fixedPayment.TextAlignment = TextAlignment.Right;

            TableCell dataCell_heatingDueTime = new TableCell();
            dataCell_heatingDueTime.TextAlignment = TextAlignment.Right;

            TableCell dataCell_heatingItem = new TableCell();
            TableCell dataCell_heatingPayment = new TableCell();
            dataCell_heatingPayment.TextAlignment = TextAlignment.Right;

            if (month == -1)
            {
                dataCell_rentDueTime.Blocks.Add(new Paragraph(new Run($"{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
            }
            else
            {
                dataCell_rentDueTime.Blocks.Add(new Paragraph(new Run($"{month}/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
            }
            dataCell_rentItem.Blocks.Add(new Paragraph(new Run($"rent")));
            dataCell_rentPayment.Blocks.Add(new Paragraph(new Run($"{viewModel.ColdRent:C2}")));

            if (month == -1)
            {
                dataCell_fixedDueTime.Blocks.Add(new Paragraph(new Run($"{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
            }
            else
            {
                dataCell_fixedDueTime.Blocks.Add(new Paragraph(new Run($"{month}/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
            }
            dataCell_fixedItem.Blocks.Add(new Paragraph(new Run($"fixed")));
            dataCell_fixedPayment.Blocks.Add(new Paragraph(new Run($"{viewModel.FixedCostsAdvance:C2}")));

            if (month == -1)
            {
                dataCell_heatingDueTime.Blocks.Add(new Paragraph(new Run($"{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
            }
            else
            {
                dataCell_heatingDueTime.Blocks.Add(new Paragraph(new Run($"{month}/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
            }
            dataCell_heatingItem.Blocks.Add(new Paragraph(new Run($"heating")));
            dataCell_heatingPayment.Blocks.Add(new Paragraph(new Run($"{viewModel.HeatingCostsAdvance:C2}")));

            dataRow_rent.Cells.Add(dataCell_rentDueTime);
            dataRow_rent.Cells.Add(dataCell_rentItem);
            dataRow_rent.Cells.Add(dataCell_rentPayment);

            dataRow_fixed.Cells.Add(dataCell_fixedDueTime);
            dataRow_fixed.Cells.Add(dataCell_fixedItem);
            dataRow_fixed.Cells.Add(dataCell_fixedPayment);

            dataRow_heating.Cells.Add(dataCell_heatingDueTime);
            dataRow_heating.Cells.Add(dataCell_heatingItem);
            dataRow_heating.Cells.Add(dataCell_heatingPayment);

            dataRowGroup.Rows.Add(dataRow_rent);
            dataRowGroup.Rows.Add(dataRow_fixed);
            dataRowGroup.Rows.Add(dataRow_heating);

            if (OtherOutputSelected)
            {
                TableCell dataCell_otherDueTime = new TableCell();
                dataCell_otherDueTime.TextAlignment = TextAlignment.Right;

                TableCell dataCell_otherItem = new TableCell();

                TableCell dataCell_otherPayment = new TableCell();
                dataCell_otherPayment.TextAlignment = TextAlignment.Right;

                if (month == -1 && viewModel.StartDate.Year == SelectedYear)
                {
                    dataCell_otherDueTime.Blocks.Add(new Paragraph(new Run($"> {viewModel.StartDate.Month}/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
                }
                else if (month == -1 && viewModel.StartDate.Year < SelectedYear)
                {
                    dataCell_otherDueTime.Blocks.Add(new Paragraph(new Run($"> 1/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
                }
                else
                {
                    dataCell_otherDueTime.Blocks.Add(new Paragraph(new Run($"{month}/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
                }
                dataCell_otherItem.Blocks.Add(new Paragraph(new Run($"other")));
                dataCell_otherPayment.Blocks.Add(new Paragraph(new Run($"{viewModel.OtherFTISum:C2}")));

                dataRow_other.Cells.Add(dataCell_otherDueTime);
                dataRow_other.Cells.Add(dataCell_otherItem);
                dataRow_other.Cells.Add(dataCell_otherPayment);

                dataRowGroup.Rows.Add(dataRow_other);
            }

            if (CreditOutputSelected)
            {
                TableCell dataCell_creditDueTime = new TableCell();
                dataCell_creditDueTime.TextAlignment = TextAlignment.Right;

                TableCell dataCell_creditItem = new TableCell();

                TableCell dataCell_creditPayment = new TableCell();
                dataCell_creditPayment.TextAlignment = TextAlignment.Right;

                if (month == -1 && viewModel.StartDate.Year == SelectedYear)
                {
                    dataCell_creditDueTime.Blocks.Add(new Paragraph(new Run($"> {viewModel.StartDate.Month}/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
                }
                else if (month == -1 && viewModel.StartDate.Year < SelectedYear)
                {
                    dataCell_creditDueTime.Blocks.Add(new Paragraph(new Run($"> 1/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
                }
                else
                {
                    dataCell_creditDueTime.Blocks.Add(new Paragraph(new Run($"{month}/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
                }
                dataCell_creditItem.Blocks.Add(new Paragraph(new Run($"credit")));
                dataCell_creditPayment.Blocks.Add(new Paragraph(new Run($"{-1 * viewModel.CreditSum:C2}")));

                dataRow_credits.Cells.Add(dataCell_creditDueTime);
                dataRow_credits.Cells.Add(dataCell_creditItem);
                dataRow_credits.Cells.Add(dataCell_creditPayment);

                dataRowGroup.Rows.Add(dataRow_credits);
            }


            TableCell dataCell_sumDueTime = new TableCell();
            dataCell_sumDueTime.TextAlignment = TextAlignment.Right;

            TableCell dataCell_sumItem = new TableCell();

            TableCell dataCell_sumPayment = new TableCell();
            dataCell_sumPayment.TextAlignment = TextAlignment.Right;

            dataCell_sumPayment.FontWeight = FontWeights.Bold;

            if (month == -1 && viewModel.StartDate.Year == SelectedYear)
            {
                dataCell_sumDueTime.Blocks.Add(new Paragraph(new Run($"> {viewModel.StartDate.Month}/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
            }
            else if (month == -1 && viewModel.StartDate.Year < SelectedYear)
            {
                dataCell_sumDueTime.Blocks.Add(new Paragraph(new Run($"> 1/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
            }
            else
            {
                dataCell_sumDueTime.Blocks.Add(new Paragraph(new Run($"{month}/{SelectedYear}")) { Margin = new Thickness(0, 0, 10, 0) });
            }
            dataCell_sumItem.Blocks.Add(new Paragraph(new Run($"sum")));

            if (OtherOutputSelected || CreditOutputSelected)
            {
                if (OtherOutputSelected && !CreditOutputSelected)
                {
                    dataCell_sumPayment.Blocks.Add(new Paragraph(new Run($"{viewModel.CompleteCosts:C2}")));
                }

                if (!OtherOutputSelected && CreditOutputSelected)
                {
                    double reducedsum = viewModel.CostsTotal - viewModel.CreditSum;

                    dataCell_sumPayment.Blocks.Add(new Paragraph(new Run($"{reducedsum:C2}")));
                }

                if (OtherOutputSelected && CreditOutputSelected)
                {
                    double reducedsum = viewModel.CompleteCosts - viewModel.CreditSum;

                    dataCell_sumPayment.Blocks.Add(new Paragraph(new Run($"{reducedsum:C2}")));
                }
            }
            else
            {
                dataCell_sumPayment.Blocks.Add(new Paragraph(new Run($"{viewModel.CostsTotal:C2}")));
            }

            dataRow_sum.Cells.Add(dataCell_sumDueTime);
            dataRow_sum.Cells.Add(dataCell_sumItem);
            dataRow_sum.Cells.Add(dataCell_sumPayment);

            dataRowGroup.Rows.Add(dataRow_sum);

            rentPlanFlatTable.RowGroups.Add(dataRowGroup);

            return rentPlanFlatTable;
        }


        private Block RentPlanRoomsTable(RentViewModel viewModel, int month = -1)
        {
            Table rentPlanFlatTable = new Table();

            TableColumn DateColumn = new TableColumn();
            TableColumn ItemColumn = new TableColumn();
            TableColumn CostColumn = new TableColumn();

            rentPlanFlatTable.Columns.Add(DateColumn);
            rentPlanFlatTable.Columns.Add(ItemColumn);
            rentPlanFlatTable.Columns.Add(CostColumn);

            TableRowGroup headerRowGroup = new TableRowGroup();

            headerRowGroup.Style = Application.Current.FindResource("HeaderRowStyle") as Style;

            TableRow headerRow = new TableRow();
            TableCell headerCell_Column0 = new TableCell();
            TableCell headerCell_DueTime = new TableCell();
            TableCell headerCell_Item = new TableCell();
            TableCell headerCell_Costs = new TableCell();

            headerCell_Column0.Blocks.Add(new Paragraph(new Run("Room")));
            headerCell_DueTime.Blocks.Add(new Paragraph(new Run("Time")));
            headerCell_Item.Blocks.Add(new Paragraph(new Run("Item")));
            headerCell_Costs.Blocks.Add(new Paragraph(new Run("Costs")));

            headerRow.Cells.Add(headerCell_Column0);
            headerRow.Cells.Add(headerCell_DueTime);
            headerRow.Cells.Add(headerCell_Item);
            headerRow.Cells.Add(headerCell_Costs);

            headerRowGroup.Rows.Add(headerRow);

            rentPlanFlatTable.RowGroups.Add(headerRowGroup);

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            foreach (RoomCostShareRent item in viewModel.RoomCostShares)
            {
                TableRow dataRow_header = new TableRow();
                TableRow dataRow_rent = new TableRow();
                TableRow dataRow_fixed = new TableRow();
                TableRow dataRow_heating = new TableRow();

                TableCell dataCell_RoomName = new TableCell();
                TableCell dataCell_DueTime = new TableCell();
                TableCell dataCell_Item = new TableCell();
                TableCell dataCell_Payment = new TableCell();
                dataCell_Payment.FontWeight = FontWeights.Bold;

                dataCell_RoomName.Blocks.Add(new Paragraph(new Run(item.RoomName)));

                dataCell_DueTime.Blocks.Add(new Paragraph(new Run($"{SelectedYear}")));

                dataCell_Item.Blocks.Add(new Paragraph(new Run($"sum")));

                dataCell_Payment.Blocks.Add(new Paragraph(new Run($"{item.PriceShare:C2}")));

                dataRow_header.Cells.Add(dataCell_RoomName);
                dataRow_header.Cells.Add(dataCell_DueTime);
                dataRow_header.Cells.Add(dataCell_Item);
                dataRow_header.Cells.Add(dataCell_Payment);

                TableCell dataCell_rent0 = new TableCell();
                TableCell dataCell_rentDueTime = new TableCell();
                TableCell dataCell_rentItem = new TableCell();
                TableCell dataCell_rentPayment = new TableCell();

                TableCell dataCell_fixed0 = new TableCell();
                TableCell dataCell_fixedDueTime = new TableCell();
                TableCell dataCell_fixedItem = new TableCell();
                TableCell dataCell_fixedPayment = new TableCell();

                TableCell dataCell_heating0 = new TableCell();
                TableCell dataCell_heatingDueTime = new TableCell();
                TableCell dataCell_heatingItem = new TableCell();
                TableCell dataCell_heatingPayment = new TableCell();

                dataCell_rentDueTime.Blocks.Add(new Paragraph(new Run($"{SelectedYear}")));
                dataCell_rentItem.Blocks.Add(new Paragraph(new Run($"rent")));
                dataCell_rentPayment.Blocks.Add(new Paragraph(new Run($"{item.RentShare:C2}")));

                dataCell_fixedDueTime.Blocks.Add(new Paragraph(new Run($"{SelectedYear}")));
                dataCell_fixedItem.Blocks.Add(new Paragraph(new Run($"fixed")));
                dataCell_fixedPayment.Blocks.Add(new Paragraph(new Run($"{item.FixedCostsAdvanceShare:C2}")));

                dataCell_heatingDueTime.Blocks.Add(new Paragraph(new Run($"{SelectedYear}")));
                dataCell_heatingItem.Blocks.Add(new Paragraph(new Run($"heating")));
                dataCell_heatingPayment.Blocks.Add(new Paragraph(new Run($"{item.HeatingCostsAdvanceShare:C2}")));

                dataRow_rent.Cells.Add(dataCell_rent0);
                dataRow_rent.Cells.Add(dataCell_rentDueTime);
                dataRow_rent.Cells.Add(dataCell_rentItem);
                dataRow_rent.Cells.Add(dataCell_rentPayment);

                dataRow_fixed.Cells.Add(dataCell_fixed0);
                dataRow_fixed.Cells.Add(dataCell_fixedDueTime);
                dataRow_fixed.Cells.Add(dataCell_fixedItem);
                dataRow_fixed.Cells.Add(dataCell_fixedPayment);

                dataRow_heating.Cells.Add(dataCell_heating0);
                dataRow_heating.Cells.Add(dataCell_heatingDueTime);
                dataRow_heating.Cells.Add(dataCell_heatingItem);
                dataRow_heating.Cells.Add(dataCell_heatingPayment);

                dataRowGroup.Rows.Add(dataRow_header);
                dataRowGroup.Rows.Add(dataRow_rent);
                dataRowGroup.Rows.Add(dataRow_fixed);
                dataRowGroup.Rows.Add(dataRow_heating);

                TableRow tableRow = new TableRow();
                tableRow.Cells.Add(new TableCell(new Paragraph()));

                dataRowGroup.Rows.Add(tableRow);

                //TableRow dataRow = new TableRow();
                //TableRow dataRow = new TableRow();

            }


            //string output = $"{month:00}/{SelectedYear} rent\t\t{viewmodel.ColdRent:C2}\n" +
            //                $"{month:00}/{SelectedYear} fixed\t\t{viewmodel.FixedCostsAdvance:C2}\n" +
            //                $"{month:00}/{SelectedYear} heating\t{viewmodel.HeatingCostsAdvance:C2}\n";


            rentPlanFlatTable.RowGroups.Add(dataRowGroup);

            return rentPlanFlatTable;
        }


        private string BuildOtherCostDetails()
        {
            string rentOutput = string.Empty;

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
                    }


                    rentOutput += $"rent begin:\t\t{RentList[i].StartDate:d}";


                    if (SelectedDetailOption.Equals("TimeScale"))
                    {
                        for (int monthCounter = 1; monthCounter < 13; monthCounter++)
                        {
                            // if StartDate.Year < SelectedYear
                            //&& RentList[i + 1].StartDate.Year != RentList[i].StartDate.Year
                            //&& monthCounter >= RentList[i].StartDate.Month

                            if (i + 1 < RentList.Count)
                            {

                                // später im Ablauf, um Nachfolger zu berücksichtigen

                                if (RentList[i + 1].StartDate.Month - 1 < monthCounter)
                                {
                                    break;
                                }

                                if (RentList[i].StartDate.Year < SelectedYear)
                                {
                                    rentOutput += $"{monthCounter:00}/{SelectedYear} price\t\t{RentList[i].OtherFTISum:C2}\n" +
                                        $"------------------------------------------\n";

                                    foreach (FinancialTransactionItemRentViewModel item in RentList[i].FinancialTransactionItemViewModels)
                                    {
                                        rentOutput += $"{monthCounter:00}/{SelectedYear} {item.TransactionItem}\t\t{item.TransactionSum:C2}\n";
                                    }

                                    rentOutput += "\n\n\n";

                                }
                                else if (RentList[i].StartDate.Year == SelectedYear
                                    && monthCounter >= RentList[i].StartDate.Month)
                                {
                                    rentOutput += $"{monthCounter:00}/{SelectedYear} price\t\t{RentList[i].OtherFTISum:C2}\n" +
                                                         $"------------------------------------------\n";

                                    foreach (FinancialTransactionItemRentViewModel item in RentList[i].FinancialTransactionItemViewModels)
                                    {
                                        rentOutput += $"{monthCounter:00}/{SelectedYear} {item.TransactionItem}\t\t{item.TransactionSum:C2}\n";
                                    }

                                    rentOutput += "\n\n\n";
                                }


                            }
                            else
                            {
                                if (!RentList[i].HasOtherCosts)
                                {
                                    rentOutput += $"end:\t\t\t{RentList[i].StartDate - TimeSpan.FromDays(1):d}\n\n";
                                    continue;
                                }

                                if (RentList[i].StartDate.Year == SelectedYear && monthCounter < RentList[i].StartDate.Month)
                                {
                                    continue;
                                }
                                else
                                {
                                    rentOutput += $"{monthCounter:00}/{SelectedYear} price\t\t{RentList[i].OtherFTISum:C2}\n" +
                                                         $"------------------------------------------\n";

                                    foreach (FinancialTransactionItemRentViewModel item in RentList[i].FinancialTransactionItemViewModels)
                                    {
                                        rentOutput += $"{monthCounter:00}/{SelectedYear} {item.TransactionItem}\t\t{item.TransactionSum:C2}\n";
                                    }

                                    rentOutput += "\n\n\n";
                                }

                            }
                        }


                        if (i + 1 < RentList.Count && RentList[i + 1].HasOtherCosts == false)
                        {
                            rentOutput += $"rent end:\t\t\t{RentList[i + 1].StartDate - TimeSpan.FromDays(1):d}\n\n";
                        }

                    }
                    else
                    {


                        rentOutput += $"{SelectedYear} price\t\t{RentList[i].OtherFTISum:C2}\n" +
                                             $"------------------------------------------\n";

                        foreach (FinancialTransactionItemRentViewModel item in RentList[i].FinancialTransactionItemViewModels)
                        {
                            rentOutput += $"{SelectedYear} {item.TransactionItem}\t\t{item.TransactionSum:C2}\n";
                        }

                        rentOutput += "\n\n";

                        if (i + 1 < RentList.Count && RentList[i + 1].HasOtherCosts == false)
                        {
                            rentOutput += $"rent end:\t\t{RentList[i + 1].StartDate - TimeSpan.FromDays(1):d}\n";

                            rentOutput += "\n\n";
                        }

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


                    Table headerTable = new Table();

                    TableColumn DateColumn = new TableColumn() { Width = new GridLength(100) };
                    TableColumn ItemColumn = new TableColumn() { Width = new GridLength(300) };
                    TableColumn CostColumn = new TableColumn() { Width = new GridLength(100) };

                    headerTable.Columns.Add(DateColumn);
                    headerTable.Columns.Add(ItemColumn);
                    headerTable.Columns.Add(CostColumn);

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

                    headerTable.RowGroups.Add(headerRowGroup);

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


                    Table headerTable = new Table();

                    TableColumn DateColumn = new TableColumn() { Width = new GridLength(100) };
                    TableColumn ItemColumn = new TableColumn() { Width = new GridLength(300) };
                    TableColumn CostColumn = new TableColumn() { Width = new GridLength(100) };

                    headerTable.Columns.Add(DateColumn);
                    headerTable.Columns.Add(ItemColumn);
                    headerTable.Columns.Add(CostColumn);

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

                    headerTable.RowGroups.Add(headerRowGroup);

                    roomsOutput.Blocks.Add(headerTable);

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