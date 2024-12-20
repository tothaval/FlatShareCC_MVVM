/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PrintOutputBase
 * 
 *  helper class for creating flow document compatible objects for output in PrintViewModel
 */
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System.Windows;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Data;
using SharedLivingCostCalculator.Interfaces.Contract;
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.ViewModels;
using System.Drawing;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.Interfaces.Financial;

namespace SharedLivingCostCalculator.Utility.PrintViewHelperFunctions
{
    public class PrintOutputBase
    {

        // Properties & Fields
        #region Properties & Fields

        private FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;

        private PrintViewModel _PrintViewModel { get; }


        public int SelectedYear { get; set; }

        #endregion


        // Constructors
        #region Constructors

        public PrintOutputBase(PrintViewModel printViewModel, FlatViewModel flatViewModel, int selectedYear)
        {
            _PrintViewModel = printViewModel;
            _FlatViewModel = flatViewModel;
            SelectedYear = selectedYear;
        }

        #endregion


        // Methods
        #region Methods

        public TableRow BillingOutputTableRow(
            BillingViewModel viewModel,
            double payment,
            string item,
            string shareTypeString = "",
            bool FontWeightBold = false,
            bool Currency = true,
            bool HasTopLine = true
            )
        {
            TableRow dataRow = new TableRow();

            TableCell StartTime = new TableCell();
            StartTime.TextAlignment = TextAlignment.Right;

            TableCell EndTime = new TableCell();
            EndTime.TextAlignment = TextAlignment.Right;

            TableCell Item = new TableCell();

            TableCell ShareType = new TableCell();

            //StartTime.Blocks.Add(new Paragraph(new Run($"{viewModel.StartDate:d}-{viewModel.EndDate:d}")) { Margin = new Thickness(0, 0, 10, 0) });
            StartTime.Blocks.Add(new Paragraph(new Run($"{viewModel.StartDate:d}")) { Margin = new Thickness(0, 0, 10, 0) });
            EndTime.Blocks.Add(new Paragraph(new Run($"{viewModel.EndDate:d}")) { Margin = new Thickness(0, 0, 10, 0) });

            Item.Blocks.Add(new Paragraph( new Run(item)));

            ShareType.Blocks.Add(new Paragraph(new Run(shareTypeString)));

            dataRow.Cells.Add(StartTime);
            dataRow.Cells.Add(EndTime);
            dataRow.Cells.Add(Item);
            dataRow.Cells.Add(ShareType);
            dataRow.Cells.Add(ConfiguratePaymentCell(payment, FontWeightBold, Currency, HasTopLine));

            return dataRow;
        }


        public TableRow BillingOutputTableRowRooms(
            BillingViewModel viewModel,
            double payment,
            string item,
            string shareType = "",
            bool FontWeightBold = false,
            bool Currency = true,
            bool HasTopLine = true
            )
        {
            TableRow dataRow = new TableRow();


            TableCell Item = new TableCell();
            Item.ColumnSpan = 3;

            TableCell ShareType = new TableCell();


            Item.Blocks.Add(new Paragraph(new Run(item)));
            ShareType.Blocks.Add(new Paragraph(new Run(shareType)));

            dataRow.Cells.Add(Item);
            dataRow.Cells.Add(ShareType);
            dataRow.Cells.Add(ConfiguratePaymentCell(payment, FontWeightBold, Currency, HasTopLine));

            return dataRow;
        }


        public string BuildAddressDetails()
        {
            //Section section = new Section();

            //string ReportHeader = "";
            string rooms = "";

            try
            {
                //ReportHeader = Application.Current.FindResource("IDF_Address").ToString();
                object? res = Application.Current.Resources["IDF_Rooms"];

                if (res != null)
                {
                    rooms = Application.Current.Resources["IDF_Rooms"].ToString();
                }

            }
            catch (Exception)
            {
            }

            //Paragraph p = new Paragraph(new Run(ReportHeader));
            //p.Style = headerParagraph;
            //section.Blocks.Add(p);

            //p = new Paragraph(new Run(_FlatViewModel.Address));
            //p.Style = textParagraph;
            //section.Blocks.Add(p);

            //p = new Paragraph(new Run($"{_FlatViewModel.Area}m², {_FlatViewModel.RoomCount} {rooms}"));
            //p.Style = textParagraph;
            //section.Blocks.Add(p);

            return $"{FlatViewModel.Address},\n\t{_FlatViewModel.Area}m²,\n\t{_FlatViewModel.RoomCount} {rooms}";
        }


        public Section BuildHeader(string headerText)
        {
            Section s = new Section();

            Paragraph p = new Paragraph(new Run($"\n{headerText}\n"))
            {
                Margin = new Thickness(0, 20, 0, 20),
                FontWeight = FontWeights.Bold,
                FontSize = 14.0,
                Background = new SolidColorBrush(Colors.LightGray)
            };

            s.Blocks.Add(p);

            if (_PrintViewModel.ContractDataOutputSelected)
            {
                p = new Paragraph(new Run($"{BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 12.0 });

                s.Blocks.Add(p);

                if (_PrintViewModel.RoomAreaDataSelected)
                {
                    s.Blocks.Add(BuildRoomAreaData());
                }
            }

            return s;
        }


        public Section BuildRoomAreaData()
        {
            Section s = new Section();

            s.Blocks.Add(
                new Paragraph(new Run($"Room Area Data"))
                {
                    FontFamily = new FontFamily("Verdana"),
                    FontWeight = FontWeights.Normal,
                    FontSize = 14.0,
                    Background = new SolidColorBrush(Colors.LightGray)
                }
                );

            s.Blocks.Add(RoomAreaTable());

            return s;
        }


        public TableRowGroup BuildTaxDisplay(TableRowGroup dataRowGroup, RentViewModel viewModel, int month, double taxationTarget, bool isSummary)
        {
            if (_PrintViewModel.IncludeTaxesSelected)
            {
                double tax = _PrintViewModel.TaxValue / 100;
                double taxedSum = 0.0;

                if (_PrintViewModel.SelectedTaxOption == TaxOptionTypes.Taxed)
                {
                    taxedSum = taxationTarget - taxationTarget / (tax + 1);

                    if (isSummary)
                    {
                        dataRowGroup.Rows.Add(OutputTableRow(taxationTarget, "sum", "", true, false));
                    }
                    else
                    {
                        dataRowGroup.Rows.Add(OutputTableRow(taxationTarget, "sum", "", true));
                    }

                    dataRowGroup.Rows.Add(OutputTableRow(taxedSum, $"including {_PrintViewModel.TaxValue}% taxes", ""));
                }
                else
                {
                    taxedSum = tax * taxationTarget;
                    double withTaxes = taxedSum + taxationTarget;

                    if (isSummary)
                    {
                        dataRowGroup.Rows.Add(OutputTableRow(taxationTarget, "sum", "", false, false));
                    }
                    else
                    {
                        dataRowGroup.Rows.Add(OutputTableRow(taxationTarget, "sum", "", true));
                    }
                    dataRowGroup.Rows.Add(OutputTableRow(taxedSum, $"tax {_PrintViewModel.TaxValue}%", ""));

                    dataRowGroup.Rows.Add(OutputTableRow( withTaxes, "taxed sum", "", true));
                }
            }
            else
            {
                if (isSummary)
                {
                    dataRowGroup.Rows.Add(OutputTableRow(taxationTarget, "sum", "", true, false));
                }
                else
                {
                    dataRowGroup.Rows.Add(OutputTableRow(taxationTarget, "sum", "", true));
                }
            }

            dataRowGroup.Rows.Add(SeparatorLineTableRow(true));

            return dataRowGroup;
        }


        public TableRowGroup BuildTaxDisplayBilling(TableRowGroup dataRowGroup, BillingViewModel viewModel, double taxationTarget, bool isSummary)
        {
            if (_PrintViewModel.IncludeTaxesSelected)
            {
                double tax = _PrintViewModel.TaxValue / 100;
                double taxedSum = 0.0;

                if (_PrintViewModel.SelectedTaxOption == TaxOptionTypes.Taxed)
                {
                    taxedSum = taxationTarget - taxationTarget / (tax + 1);

                    if (isSummary)
                    {
                        dataRowGroup.Rows.Add(
                            BillingOutputTableRow(
                                viewModel,
                                taxationTarget,
                                "sum",
                                "",
                                true,
                                true,
                                false));
                    }
                    else
                    {
                        dataRowGroup.Rows.Add(
                            BillingOutputTableRow(
                                viewModel,
                                taxationTarget,
                                "sum",
                                "",
                                true,
                                true,
                                true));
                    }

                    dataRowGroup.Rows.Add(
                        BillingOutputTableRow(
                            viewModel,
                            taxedSum,
                            $"including {_PrintViewModel.TaxValue}% taxes",
                            "",
                            false,
                            true,
                            false));
                }
                else
                {
                    taxedSum = tax * taxationTarget;
                    double withTaxes = taxedSum + taxationTarget;

                    if (isSummary)
                    {
                        dataRowGroup.Rows.Add(
                            BillingOutputTableRow(
                                viewModel,
                                taxationTarget,
                                "sum",
                                "",
                                false,
                                true,
                                false));
                    }
                    else
                    {
                        dataRowGroup.Rows.Add(
                            BillingOutputTableRow(
                                viewModel,
                                taxationTarget,
                                "sum",
                                "",
                                true,
                                true,
                                true));
                    }

                    dataRowGroup.Rows.Add(
                        BillingOutputTableRow(
                            viewModel,
                            taxedSum,
                            $"tax {_PrintViewModel.TaxValue}%",
                            "",
                            false,
                            true,
                            false));

                    dataRowGroup.Rows.Add(
                        BillingOutputTableRow(
                            viewModel,
                            withTaxes,
                            "taxed sum",
                            "",
                            true,
                            true,
                            true));
                }
            }
            else
            {
                if (isSummary)
                {
                    dataRowGroup.Rows.Add(
                        BillingOutputTableRow(
                            viewModel,
                            taxationTarget,
                            "sum",
                            "",
                            true,
                            true,
                            false));
                }
                else
                {
                    dataRowGroup.Rows.Add(
                        BillingOutputTableRow(
                            viewModel,
                            taxationTarget,
                            "sum",
                            "",
                            true,
                            true,
                            true));
                }
            }

            return dataRowGroup;
        }


        public TableRowGroup BuildTaxDisplayRooms(TableRowGroup dataRowGroup, RentViewModel viewModel, int month, string roomName, double taxationTarget, bool isSummary)
        {
            if (_PrintViewModel.IncludeTaxesSelected)
            {
                double tax = _PrintViewModel.TaxValue / 100;
                double taxedSum = 0.0;

                if (_PrintViewModel.SelectedTaxOption == TaxOptionTypes.Taxed)
                {
                    taxedSum = tax * taxationTarget;

                    if (isSummary)
                    {
                        dataRowGroup.Rows.Add(OutputTableRow(taxationTarget, roomName, "", true, false));
                    }
                    else
                    {
                        dataRowGroup.Rows.Add(OutputTableRow(taxationTarget, "sum", "", true));
                    }

                    dataRowGroup.Rows.Add(OutputTableRow(taxedSum, $"including {_PrintViewModel.TaxValue}% taxes", ""));
                }
                else
                {
                    taxedSum = (tax + 1) * taxationTarget - taxationTarget;

                    double withTaxes = taxedSum + taxationTarget;

                    if (isSummary)
                    {
                        dataRowGroup.Rows.Add(OutputTableRow(taxationTarget, $"sum {roomName}", "", false, false));
                    }
                    else
                    {
                        dataRowGroup.Rows.Add(OutputTableRow(taxationTarget, "sum", "", true));
                    }
                    dataRowGroup.Rows.Add(OutputTableRow(taxedSum, $"tax {_PrintViewModel.TaxValue}%", ""));

                    dataRowGroup.Rows.Add(OutputTableRow(withTaxes, "taxed sum", "", true));
                }
            }
            else
            {
                if (isSummary)
                {
                    dataRowGroup.Rows.Add(OutputTableRow(taxationTarget, $"sum {roomName}", "", true, false));
                }
                else
                {
                    dataRowGroup.Rows.Add(OutputTableRow(taxationTarget, "sum", "", true));
                }
            }

            dataRowGroup.Rows.Add(SeparatorLineTableRow(true));

            return dataRowGroup;
        }


        public TableRowGroup BuildTaxDisplayRoomsBilling(TableRowGroup dataRowGroup, BillingViewModel viewModel, string roomName, double taxationTarget, bool isSummary)
        {
            if (_PrintViewModel.IncludeTaxesSelected)
            {
                double tax = _PrintViewModel.TaxValue / 100;
                double taxedSum = 0.0;

                if (_PrintViewModel.SelectedTaxOption == TaxOptionTypes.Taxed)
                {
                    taxedSum = tax * taxationTarget;

                    if (isSummary)
                    {
                        dataRowGroup.Rows.Add(
                            BillingOutputTableRowRooms(
                                viewModel,
                                taxationTarget,
                                roomName,
                                "",
                                true,
                                true,
                                false
                                )
                            );
                    }
                    else
                    {
                        dataRowGroup.Rows.Add(
                            BillingOutputTableRowRooms(
                                viewModel,
                                taxationTarget,
                                roomName,
                                "",
                                true,
                                true,
                                true));
                    }

                    dataRowGroup.Rows.Add(
                        BillingOutputTableRowRooms(
                            viewModel,
                            taxedSum,
                            $"including {_PrintViewModel.TaxValue}% taxes",
                            "",
                            false,
                            true,
                            false));
                }
                else
                {
                    taxedSum = (tax + 1) * taxationTarget - taxationTarget;

                    double withTaxes = taxedSum + taxationTarget;

                    if (isSummary)
                    {
                        dataRowGroup.Rows.Add(
                            BillingOutputTableRowRooms(
                                viewModel,
                                taxationTarget,
                                roomName,
                                "",
                                true,
                                true,
                                false));
                    }
                    else
                    {
                        dataRowGroup.Rows.Add(
                            BillingOutputTableRowRooms(
                                viewModel,
                                taxationTarget,
                                roomName,
                                "",
                                true,
                                true,
                                true));
                    }

                    dataRowGroup.Rows.Add(
                        BillingOutputTableRowRooms(
                            viewModel,
                            taxedSum,
                            $"tax {_PrintViewModel.TaxValue}%",
                            "",
                            false,
                            true,
                            false));

                    dataRowGroup.Rows.Add(
                        BillingOutputTableRowRooms(
                            viewModel,
                            withTaxes,
                            "taxed sum",
                            "",
                            true,
                            true,
                            true));
                }
            }
            else
            {
                if (isSummary)
                {
                    dataRowGroup.Rows.Add(
                        BillingOutputTableRowRooms(
                            viewModel,
                            taxationTarget,
                            roomName,
                            "",
                            true,
                            true,
                            false));
                }
                else
                {
                    dataRowGroup.Rows.Add(
                        BillingOutputTableRowRooms(
                            viewModel,
                            taxationTarget,
                            roomName,
                            "",
                            true,
                            true,
                            true));
                }
            }

            return dataRowGroup;
        }


        public Section? BuildValueChangeHeader(Section documentContext, ObservableCollection<RentViewModel> RentList, int iterator, string text, bool isOverview)
        {
            if (RentList[iterator].StartDate.Year < SelectedYear)
            {
                if (iterator + 1 < RentList.Count)
                {
                    if (RentList[iterator + 1].StartDate > RentList[iterator].StartDate && RentList[iterator + 1].StartDate.Year < SelectedYear)
                    {
                        return null;
                    }
                    else
                    {
                        documentContext = WriteRentChangeToFlowDocument(documentContext, RentList[iterator], text, isOverview);
                    }
                }
                else
                {
                    documentContext = WriteRentChangeToFlowDocument(documentContext, RentList[iterator], text, isOverview);
                }
            }
            else
            {
                if (iterator == 0)
                {
                    documentContext = WriteRentChangeToFlowDocument(documentContext, RentList[iterator], text, isOverview);
                }
                else
                {
                    documentContext = WriteRentChangeToFlowDocument(documentContext, RentList[iterator], text, isOverview);
                }
            }

            return documentContext;
        }


        public TableCell ConfiguratePaymentCell(double payment, bool FontWeightBold, bool Currency, bool HasTopLine)
        {
            TableCell Payment = new TableCell();
            Payment.TextAlignment = TextAlignment.Right;

            Paragraph paymentParagraph = new Paragraph();

            if (FontWeightBold)
            {
                //dataRow.Background = new SolidColorBrush(Colors.Silver);
                Payment.Background = new SolidColorBrush(Colors.Silver);

                if (Currency)
                {
                    paymentParagraph = new Paragraph(new Run($"{payment:C2}"))
                    {
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new Thickness(0, 0, 0, 0),
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 10, 0)
                    };

                    if (HasTopLine)
                    {
                        paymentParagraph.BorderThickness = new Thickness(0, 1, 0, 0);
                    }
                }
                else
                {
                    paymentParagraph = new Paragraph(new Run($"{payment:N2}"))
                    {
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new Thickness(0, 1, 0, 0),
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 10, 0)
                    };
                }

            }
            else
            {
                if (Currency)
                {
                    paymentParagraph = new Paragraph(new Run($"{payment:C2}"))
                    {
                        Margin = new Thickness(0, 0, 10, 0)
                    };
                }
                else
                {
                    paymentParagraph = new Paragraph(new Run($"{payment:N2}"))
                    {
                        Margin = new Thickness(0, 0, 10, 0)
                    };
                }
            }

            if (payment < 0)
            {
                paymentParagraph.Foreground = new SolidColorBrush(Colors.Red);
            }

            Payment.Blocks.Add(paymentParagraph);

            return Payment;
        }


        public ObservableCollection<RentViewModel> FillRentList()
        {
            ObservableCollection<RentViewModel> RentList = new ObservableCollection<RentViewModel>();

            if (_PrintViewModel.PrintMostRecentSelected)
            {
                RentList.Add(new Compute().SearchForMostRecentRentViewModel(_FlatViewModel));
            }
            else if (_PrintViewModel.PrintSelectedItemSelected)
            {
                if (_PrintViewModel.SelectedRentChange != null)
                {
                    RentList.Add(_PrintViewModel.SelectedRentChange); 
                }
            }
            else
            {
                RentList = new Compute().FindRelevantRentViewModels(_FlatViewModel, SelectedYear);
            }

            return RentList;
        }

              
        public Table OutputTable()
        {
            Table dataOutputTable = new Table();

            TableColumn StartDateColumn = new TableColumn() { Width = new GridLength(100) };
            TableColumn EndDateColumn = new TableColumn() { Width = new GridLength(100) };
            TableColumn ItemColumn = new TableColumn() { Width = new GridLength(200) };
            TableColumn ShareTypeColumn = new TableColumn() { Width = new GridLength(100) };
            TableColumn CostColumn = new TableColumn() { Width = new GridLength(100) };

            dataOutputTable.Columns.Add(StartDateColumn);
            dataOutputTable.Columns.Add(EndDateColumn);
            dataOutputTable.Columns.Add(ItemColumn);
            dataOutputTable.Columns.Add(ShareTypeColumn);
            dataOutputTable.Columns.Add(CostColumn);

            return dataOutputTable;
        }


        public Table OutputTableRoomAreaData()
        {

            Table dataOutputTable = new Table();

            TableColumn RoomNameColumn = new TableColumn() { Width = new GridLength(120) };
            TableColumn RoomAreaColumn = new TableColumn() { Width = new GridLength(120) };
            TableColumn SharedAreaColumn = new TableColumn() { Width = new GridLength(120) };
            TableColumn TotalAreaColumn = new TableColumn() { Width = new GridLength(120) };
            TableColumn RoomAreaPercentageColumn = new TableColumn() { Width = new GridLength(120) };

            dataOutputTable.Columns.Add(RoomNameColumn);
            dataOutputTable.Columns.Add(RoomAreaColumn);
            dataOutputTable.Columns.Add(SharedAreaColumn);
            dataOutputTable.Columns.Add(TotalAreaColumn);
            dataOutputTable.Columns.Add(RoomAreaPercentageColumn);

            return dataOutputTable;
        }


        public Table OutputTableRooms()
        {
            Table dataOutputTable = new Table();

            TableColumn DateColumn = new TableColumn() { Width = new GridLength(50) };
            TableColumn RoomNameColumn = new TableColumn() { Width = new GridLength(150) };
            TableColumn ItemColumn = new TableColumn() { Width = new GridLength(200) };
            TableColumn ShareTypeColumn = new TableColumn() { Width = new GridLength(100) };
            TableColumn CostColumn = new TableColumn() { Width = new GridLength(100) };

            dataOutputTable.Columns.Add(DateColumn);
            dataOutputTable.Columns.Add(RoomNameColumn);
            dataOutputTable.Columns.Add(ItemColumn);
            dataOutputTable.Columns.Add(ShareTypeColumn);
            dataOutputTable.Columns.Add(CostColumn);

            return dataOutputTable;
        }


        public TableRow OutputTableRow(double payment, string item, string shareTypeString, bool FontWeightBold = false, bool HasTopLine = true, bool Currency = true)
        {
            TableRow dataRow = new TableRow();

            TableCell Item = new TableCell();
            Item.ColumnSpan = 3;

            TableCell ShareType = new TableCell();

            ShareType.Blocks.Add(new Paragraph(new Run(shareTypeString)));

            Item.Blocks.Add(new Paragraph(new Run(item)));

            dataRow.Cells.Add(Item);
            dataRow.Cells.Add(ShareType);
            dataRow.Cells.Add(ConfiguratePaymentCell(payment, FontWeightBold, Currency, HasTopLine));

            return dataRow;
        }


        public TableRow OutputTableRowCheck(double payment, string item, bool FontWeightBold = false, bool HasTopLine = true, bool Currency=true)
        {
            TableRow dataRow = new TableRow();

            TableCell Item = new TableCell();
            Item.ColumnSpan = 4;            

            Item.Blocks.Add(new Paragraph(new Run(item)));

            dataRow.Cells.Add(Item);
            dataRow.Cells.Add(ConfiguratePaymentCell(payment, FontWeightBold, Currency, HasTopLine));

            return dataRow;
        }


        public TableRow OutputTableRowRoomAreaData(RoomViewModel roomViewModel, FlatViewModel flatViewModel)
        {
            TableRow dataRow = new TableRow();

            TableCell RoomNameCell = new TableCell();

            TableCell RoomAreaCell = new TableCell();
            RoomAreaCell.TextAlignment = TextAlignment.Right;

            TableCell SharedAreaShareCell = new TableCell();
            SharedAreaShareCell.TextAlignment = TextAlignment.Right;

            TableCell TotalAreaShareCell = new TableCell();
            TotalAreaShareCell.TextAlignment = TextAlignment.Right;

            TableCell AreaPercentageCell = new TableCell();
            AreaPercentageCell.TextAlignment = TextAlignment.Right;

            double sharedAreaShare = flatViewModel.SharedArea / flatViewModel.RoomCount;
            double rentedAreaShare = roomViewModel.RoomArea + sharedAreaShare;
            double areaSharePercentage = rentedAreaShare / FlatViewModel.Area * 100;

            RoomNameCell.Blocks.Add(new Paragraph(new Run($"{roomViewModel.RoomName}")));

            RoomAreaCell.Blocks.Add(new Paragraph(new Run($"{roomViewModel.RoomArea:N2}")));
            SharedAreaShareCell.Blocks.Add(new Paragraph(new Run($"{sharedAreaShare:N2}")));
            TotalAreaShareCell.Blocks.Add(new Paragraph(new Run($"{rentedAreaShare:N2}")) { FontWeight = FontWeights.Bold, FontSize = 12.0 });
            AreaPercentageCell.Blocks.Add(new Paragraph(new Run($"{areaSharePercentage:N2}%")) { Margin = new Thickness(0, 0, 10, 0) });

            dataRow.Cells.Add(RoomNameCell);
            dataRow.Cells.Add(RoomAreaCell);
            dataRow.Cells.Add(SharedAreaShareCell);
            dataRow.Cells.Add(TotalAreaShareCell);
            dataRow.Cells.Add(AreaPercentageCell);

            return dataRow;
        }


        public TableRow OutputTableRowRoomAreaDataHeader()
        {
            TableRow dataRow = new TableRow();

            TableCell RoomNameCell = new TableCell();

            TableCell RoomAreaCell = new TableCell();
            RoomAreaCell.TextAlignment = TextAlignment.Right;

            TableCell SharedAreaShareCell = new TableCell();
            SharedAreaShareCell.TextAlignment = TextAlignment.Right;

            TableCell TotalAreaShareCell = new TableCell();
            TotalAreaShareCell.TextAlignment = TextAlignment.Right;

            TableCell AreaPercentageCell = new TableCell();
            AreaPercentageCell.TextAlignment = TextAlignment.Right;

            RoomNameCell.Blocks.Add(new Paragraph(new Run($"Room")) { FontWeight = FontWeights.Bold, FontSize = 14.0 });
            RoomAreaCell.Blocks.Add(new Paragraph(new Run($"Room Area")) { FontWeight = FontWeights.Bold, FontSize = 14.0 });
            SharedAreaShareCell.Blocks.Add(new Paragraph(new Run($"Area Share")) { FontWeight = FontWeights.Bold, FontSize = 14.0 });
            TotalAreaShareCell.Blocks.Add(new Paragraph(new Run($"Rented Area")) { FontWeight = FontWeights.Bold, FontSize = 14.0 });
            AreaPercentageCell.Blocks.Add(new Paragraph(new Run($"Area %")) { FontWeight = FontWeights.Bold, FontSize = 14.0, Margin = new Thickness(0, 0, 10, 0) });

            dataRow.Cells.Add(RoomNameCell);
            dataRow.Cells.Add(RoomAreaCell);
            dataRow.Cells.Add(SharedAreaShareCell);
            dataRow.Cells.Add(TotalAreaShareCell);
            dataRow.Cells.Add(AreaPercentageCell);

            return dataRow;
        }


        private Block RoomAreaTable()
        {
            Table roomAreaTable = OutputTableRoomAreaData();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            dataRowGroup.Rows.Add(OutputTableRowRoomAreaDataHeader());

            foreach (RoomViewModel item in FlatViewModel.Rooms)
            {
                dataRowGroup.Rows.Add(OutputTableRowRoomAreaData(item, FlatViewModel));
            }

            roomAreaTable.RowGroups.Add(dataRowGroup);

            return roomAreaTable;
        }


        public Paragraph RoomSeparatorLine(IRoomCostShare roomCostShare, bool showTenant, string cause)
        {
            if (showTenant)
            {
                return new Paragraph(new Run($"\n{roomCostShare.RoomName} {roomCostShare.RoomArea}m²\n{cause}\n{roomCostShare.Tenant}\n")){
                    Background = new SolidColorBrush(Colors.LightGray),
                    FontFamily = new FontFamily("Verdana"),
                    FontWeight = FontWeights.Bold,
                    FontSize = 14.0,
                    BreakPageBefore = true};
            }

            return new Paragraph(new Run($"\n{roomCostShare.RoomName} {roomCostShare.RoomArea}m²\n{cause}\n\n")){
                Background = new SolidColorBrush(Colors.LightGray),
                FontFamily = new FontFamily("Verdana"),
                FontWeight = FontWeights.Bold,
                FontSize = 14.0,
                BreakPageBefore = true
            };
        }


        public TableRow RoomSeparatorTableRow(IRoomCostShare roomCostShare, bool showTenant, string cause)
        {
            if (showTenant)
            {
                return SeparatorTextTableRow(
                    $"\n{roomCostShare.RoomName} {roomCostShare.RoomArea}m²\n{cause}\n{roomCostShare.Tenant}\n",
                    false, 14.0,
                    FontWeights.Bold
                    );
            }

            return SeparatorTextTableRow($"\n{roomCostShare.RoomName} {roomCostShare.RoomArea}m²\n{cause}\n\n",
                false, 14.0,
                FontWeights.Bold);
        }


        public TableRow SeparatorLineTableRow(bool whitespace = false)
        {
            TableRow tableRow = new TableRow();

            TableCell cell = new TableCell() { ColumnSpan = 4 };

            if (whitespace)
            {
                cell.Blocks.Add(new Paragraph(new Run("\n")));
            }
            else
            {
                cell.Blocks.Add(new Paragraph(new Run("\n")) { Background = new SolidColorBrush(Colors.LightGray) });
            }

            tableRow.Cells.Add(cell);

            return tableRow;
        }


        public Paragraph SeparatorTextParagraph(string text)
        {
            return new Paragraph(new Run(text)) { Background = new SolidColorBrush(Colors.LightGray), FontSize = 16.0 };
        }


        public TableRow SeparatorTextTableRow(string text, bool sub = false, double FontSize = 14.0 , FontWeight? fontWeight = null)
        {
            TableRow tableRow = new TableRow();

            TableCell cell = new TableCell();
            TableCell emptycell = new TableCell();

            if (fontWeight != null)
            {
                cell.Blocks.Add(new Paragraph(new Run(text)) { 
                    Background = new SolidColorBrush(Colors.LightGray),
                    FontFamily = new FontFamily("Verdana"),
                    FontSize = FontSize,
                    FontWeight = (FontWeight)fontWeight });
            }
            else
            {
            cell.Blocks.Add(new Paragraph(new Run(text)) {
                Background = new SolidColorBrush(Colors.LightGray),
                FontFamily = new FontFamily("Verdana"),
                FontSize = FontSize });
            }

            if (sub)
            {
                cell.ColumnSpan = 4;

                tableRow.Cells.Add(emptycell);
                tableRow.Cells.Add(cell);
            }
            else
            {
                cell.ColumnSpan = 5;

                tableRow.Cells.Add(cell);
            }


            return tableRow;
        }


        public TableRow TableRowHeader(bool hasShareType)
        {
            TableRow headerRow = new TableRow();

            TableCell headerCell_Item = new TableCell();
            headerCell_Item.ColumnSpan = 3;

            TableCell headerCell_ShareType = new TableCell();
            TableCell headerCell_Costs = new TableCell();

            headerCell_Item.Blocks.Add(new Paragraph(new Run("Item")) { FontWeight = FontWeights.Bold, FontSize = 12 });

            if (hasShareType)
            {
                headerCell_ShareType.Blocks.Add(new Paragraph(new Run("Share Type")) { FontWeight = FontWeights.Bold, FontSize = 12 });
            }
            
            headerCell_Costs.Blocks.Add(new Paragraph(new Run("Costs")) { FontWeight = FontWeights.Bold, FontSize = 12 });

            headerRow.Cells.Add(headerCell_Item);
            headerRow.Cells.Add(headerCell_ShareType);
            headerRow.Cells.Add(headerCell_Costs);

            return headerRow;
        }


        public TableRow TableRowBillingHeaderConsumption(bool isFlat)
        {
            TableRow headerRow = new TableRow();

            TableCell headerCell_Item = new TableCell();

            TableCell headerCell_Consumption = new TableCell();

            headerCell_Item.Blocks.Add(new Paragraph(new Run("Item")) { FontWeight = FontWeights.Bold, FontSize = 12 });
            headerCell_Consumption.Blocks.Add(new Paragraph(new Run("Consumption")) { FontWeight = FontWeights.Bold, FontSize = 12 });

            if (isFlat)
            {
                headerCell_Item.ColumnSpan = 2;

                TableCell headerCell_Begin = new TableCell();
                headerCell_Begin.Blocks.Add(new Paragraph(new Run("Begin")) { FontWeight = FontWeights.Bold, FontSize = 12 });

                TableCell headerCell_End = new TableCell();
                headerCell_End.Blocks.Add(new Paragraph(new Run("End")) { FontWeight = FontWeights.Bold, FontSize = 12 });

                headerRow.Cells.Add(headerCell_Begin);
                headerRow.Cells.Add(headerCell_End);
                headerRow.Cells.Add(headerCell_Item);
                headerRow.Cells.Add(headerCell_Consumption);
            }
            else
            {
                headerCell_Item.ColumnSpan = 4;
                headerRow.Cells.Add(headerCell_Item);
                headerRow.Cells.Add(headerCell_Consumption);
            }

            return headerRow;
        }


        public TableRow TableRowBillingHeaderFlat(bool hasShareType)
        {
            TableRow headerRow = new TableRow();

            TableCell headerCell_Begin = new TableCell();
            TableCell headerCell_End = new TableCell();

            TableCell headerCell_Item = new TableCell();

            TableCell headerCell_ShareType = new TableCell();
            TableCell headerCell_Costs = new TableCell();

            headerCell_Begin.Blocks.Add(new Paragraph(new Run("Begin")) { FontWeight = FontWeights.Bold, FontSize = 12 });
            headerCell_End.Blocks.Add(new Paragraph(new Run("End")) { FontWeight = FontWeights.Bold, FontSize = 12 });
            headerCell_Item.Blocks.Add(new Paragraph(new Run("Item")) { FontWeight = FontWeights.Bold, FontSize = 12 });

            if (hasShareType)
            {
                headerCell_ShareType.Blocks.Add(new Paragraph(new Run("Share Type")) { FontWeight = FontWeights.Bold, FontSize = 12 }); 
            }
            
            headerCell_Costs.Blocks.Add(new Paragraph(new Run("Costs")) { FontWeight = FontWeights.Bold, FontSize = 12 });

            headerRow.Cells.Add(headerCell_Begin);
            headerRow.Cells.Add(headerCell_End);
            headerRow.Cells.Add(headerCell_Item);
            headerRow.Cells.Add(headerCell_ShareType);
            headerRow.Cells.Add(headerCell_Costs);

            return headerRow;
        }


        public TableRowGroup WriteCheckToTableRowGroup(
            TableRowGroup dataRowGroup,
            double roomValues,
            double contractValue,
            string headerText,
            bool isSub = true
            )
        {
            dataRowGroup.Rows.Add(SeparatorTextTableRow(headerText, isSub));
            dataRowGroup.Rows.Add(OutputTableRowCheck(roomValues, "room values"));
            dataRowGroup.Rows.Add(OutputTableRowCheck(contractValue, "contract value"));
            dataRowGroup.Rows.Add(OutputTableRowCheck(roomValues - contractValue, "balance"));

            dataRowGroup.Rows.Add(SeparatorLineTableRow(true));

            return dataRowGroup;
        }


        private Section WriteRentChangeToFlowDocument(Section documentContext, RentViewModel rentViewModel, string text, bool isOverview)
        {

            if (!isOverview)
            {
                string rentChangeInfo = $"\n{text}\t\t{rentViewModel.StartDate:d}\n";

                documentContext.Blocks.Add(new Paragraph(new Run(rentChangeInfo)) { 
                    Margin = new Thickness(0, 20, 0, 0),
                    FontFamily = new FontFamily("Verdana"),
                    FontWeight = FontWeights.Bold,
                    FontSize = 14.0,
                    Background = new SolidColorBrush(Colors.LightGray) });
            }
            else
            {
                string rentChangeInfo = $"\n{text}\t{rentViewModel.StartDate:d}\n";

                documentContext.Blocks.Add(new Paragraph(new Run(rentChangeInfo))
                { Background = new SolidColorBrush(Colors.LightGray),
                    FontFamily = new FontFamily("Verdana"),
                    FontWeight = FontWeights.Normal,
                    FontSize = 14.0 });               
            }

            return documentContext;
        }

        #endregion

    }
}
// EOF