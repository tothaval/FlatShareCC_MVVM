/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BillingPrintOutput
 * 
 *  helper class for creating annual billing related output in PrintViewModel
 */
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.Interfaces.Contract;

namespace SharedLivingCostCalculator.Utility.PrintViewHelperFunctions
{
    public class BillingPrintOutput
    {

        // Properties & Fields
        #region Properties & Fields

        private BillingViewModel _BillingViewModel { get; }


        private int _SelectedYear { get; set; }


        private bool _ShowTenant { get; set; }

        #endregion


        // Constructors
        #region Constructors

        public BillingPrintOutput(BillingViewModel billingViewModel, int selectedYear, bool showTenant)
        {
            _BillingViewModel = billingViewModel;
            _SelectedYear = selectedYear;
            _ShowTenant = showTenant;
        }

        #endregion


        // Methods
        #region Methods

        private Table BillingOutputTableForRooms()
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


        private TableRowGroup BillingOutputRoomsHasPayments(TableRowGroup dataRowGroup, RoomCostShareBilling roomCostShareBilling, PrintOutputBase print)
        {
            dataRowGroup.Rows.Add(print.SeparatorTextTableRow("contract costs", true));
            dataRowGroup.Rows.Add(print.TableRowBillingHeader());

            //if (_BillingViewModel.GetFlatViewModel().RentUpdates.Count > 0)
            //{

            //    dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
            //        _BillingViewModel,
            //        roomCostShareBilling.RoomName,
            //        roomCostShareBilling.RentCostsAnnualShare,
            //        _BillingViewModel.GetFlatViewModel().RentUpdates[0].Rent.ColdRent.TransactionItem));
            //}

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.FixedCostsAnnualCostsShare,
                    _BillingViewModel.Billing.TotalFixedCostsPerPeriod.TransactionItem));

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.HeatingCostsAnnualCostsShare,
                    _BillingViewModel.Billing.TotalHeatingCostsPerPeriod.TransactionItem));

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.ExtraCostsAnnualShare,
                "sum", true));

            dataRowGroup.Rows.Add(print.SeparatorTextTableRow("contract costs balance", true));
            dataRowGroup.Rows.Add(print.TableRowBillingHeader());

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.Advances,
                "payments"));

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                -1 * roomCostShareBilling.TotalContractCostsShare,
                "contract costs"));

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.Advances - roomCostShareBilling.ExtraCostsAnnualShare,
                "balance", true));

            dataRowGroup.Rows.Add(print.SeparatorLineTableRow(true));
            dataRowGroup.Rows.Add(print.SeparatorTextTableRow("all costs", true));
            dataRowGroup.Rows.Add(print.TableRowBillingHeader());

            //if (_BillingViewModel.GetFlatViewModel().RentUpdates.Count > 0)
            //{
            //    dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
            //        _BillingViewModel,
            //        roomCostShareBilling.RoomName,
            //        roomCostShareBilling.RentCostsAnnualShare,
            //        _BillingViewModel.GetFlatViewModel().RentUpdates[0].Rent.ColdRent.TransactionItem));
            //}

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.FixedCostsAnnualCostsShare,
                _BillingViewModel.Billing.TotalFixedCostsPerPeriod.TransactionItem));

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.HeatingCostsAnnualCostsShare,
                    _BillingViewModel.Billing.TotalHeatingCostsPerPeriod.TransactionItem));

            foreach (FinancialTransactionItemBillingViewModel item in roomCostShareBilling.FinancialTransactionItemViewModels)
            {
                dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    item.TransactionSum,
                    item.TransactionItem));
            }

            foreach (FinancialTransactionItemRentViewModel item in roomCostShareBilling.Credits)
            {
                dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    -1 * item.TransactionSum,
                    item.TransactionItem));
            }


            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                 _BillingViewModel,
                 roomCostShareBilling.RoomName,
                 roomCostShareBilling.TotalCostsAnnualCostsShareNoRent,
                 "sum", true));


            dataRowGroup.Rows.Add(print.SeparatorTextTableRow("all costs balance", true));
            dataRowGroup.Rows.Add(print.TableRowBillingHeader());

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.Advances,
                "payments"));

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                -1 * roomCostShareBilling.TotalCostsAnnualCostsShareNoRent,
                "all costs"));

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.Advances - roomCostShareBilling.TotalCostsAnnualCostsShareNoRent,
                "balance", true));

            return dataRowGroup;
        }


        private TableRowGroup BillingOutputRoomsNoPayments(TableRowGroup dataRowGroup, RoomCostShareBilling roomCostShareBilling, PrintOutputBase print)
        {
            dataRowGroup.Rows.Add(print.SeparatorTextTableRow("contract costs", true));
            dataRowGroup.Rows.Add(print.TableRowBillingHeader());

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.FixedCostsAnnualCostsShare,
                    _BillingViewModel.Billing.TotalFixedCostsPerPeriod.TransactionItem));

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.HeatingCostsAnnualCostsShare,
                    _BillingViewModel.Billing.TotalHeatingCostsPerPeriod.TransactionItem));

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.ExtraCostsAnnualShare,
                "sum", true));


            dataRowGroup.Rows.Add(print.SeparatorTextTableRow("contract costs balance", true));
            dataRowGroup.Rows.Add(print.TableRowBillingHeader());

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.Advances,
                "advances"));

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                -1 * roomCostShareBilling.ExtraCostsAnnualShare,
                "contract costs"));

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.Advances - roomCostShareBilling.ExtraCostsAnnualShare,
                "balance", true));



            dataRowGroup.Rows.Add(print.SeparatorLineTableRow(true));
            dataRowGroup.Rows.Add(print.SeparatorTextTableRow("all costs", true));
            dataRowGroup.Rows.Add(print.TableRowBillingHeader());

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.FixedCostsAnnualCostsShare,
                _BillingViewModel.Billing.TotalFixedCostsPerPeriod.TransactionItem));

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.HeatingCostsAnnualCostsShare,
                    _BillingViewModel.Billing.TotalHeatingCostsPerPeriod.TransactionItem));

            foreach (FinancialTransactionItemBillingViewModel item in roomCostShareBilling.FinancialTransactionItemViewModels)
            {
                dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    item.TransactionSum,
                    item.TransactionItem));
            }

            foreach (FinancialTransactionItemRentViewModel item in roomCostShareBilling.Credits)
            {
                dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    -1 * item.TransactionSum,
                    item.TransactionItem));
            }

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                 _BillingViewModel,
                 roomCostShareBilling.RoomName,
                 roomCostShareBilling.TotalCostsAnnualCostsShare,
                 "sum", true));

            dataRowGroup.Rows.Add(print.SeparatorTextTableRow("all costs balance", true));
            dataRowGroup.Rows.Add(print.TableRowBillingHeader());

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.Advances,
                "advances"));

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                -1 * roomCostShareBilling.TotalCostsAnnualCostsShare,
                "all costs"));

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.Advances - roomCostShareBilling.TotalCostsAnnualCostsShare,
                "balance\n", true));

            return dataRowGroup;
        }


        private Table BillingOutputTableRooms(RoomCostShareBilling roomCostShareBilling)
        {
            PrintOutputBase print = new PrintOutputBase(_BillingViewModel.GetFlatViewModel(), _SelectedYear);

            Table billingTableRooms = BillingOutputTableForRooms();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            if (_BillingViewModel.HasPayments)
            {
                dataRowGroup = BillingOutputRoomsHasPayments(dataRowGroup, roomCostShareBilling, print);
            }
            else
            {
                dataRowGroup = BillingOutputRoomsNoPayments(dataRowGroup, roomCostShareBilling, print);
            }

            billingTableRooms.RowGroups.Add(dataRowGroup);

            return billingTableRooms;
        }


        private TableRow BillingOutputTableRow(BillingViewModel viewModel, double payment, string item, bool FontWeightBold = false, bool Currency = true)
        {
            TableRow dataRow = new TableRow();

            TableCell StartTime = new TableCell();
            StartTime.TextAlignment = TextAlignment.Right;

            TableCell EndTime = new TableCell();
            EndTime.TextAlignment = TextAlignment.Right;

            TableCell Item = new TableCell();
            TableCell Payment = new TableCell();
            Payment.TextAlignment = TextAlignment.Right;

            //StartTime.Blocks.Add(new Paragraph(new Run($"{viewModel.StartDate:d}-{viewModel.EndDate:d}")) { Margin = new Thickness(0, 0, 10, 0) });
            StartTime.Blocks.Add(new Paragraph(new Run($"{viewModel.StartDate:d}")) { Margin = new Thickness(0, 0, 10, 0) });
            EndTime.Blocks.Add(new Paragraph(new Run($"{viewModel.EndDate:d}")) { Margin = new Thickness(0, 0, 10, 0) });

            Item.Blocks.Add(new Paragraph(new Run(item)));

            Paragraph paymentParagraph = new Paragraph();

            if (FontWeightBold)
            {
                if (Currency)
                {
                    paymentParagraph = new Paragraph(new Run($"{payment:C2}"))
                    {
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new Thickness(0, 1, 0, 0),
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 10, 0)
                    };
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

            Payment.Blocks.Add(paymentParagraph);

            dataRow.Cells.Add(StartTime);
            dataRow.Cells.Add(EndTime);
            dataRow.Cells.Add(Item);
            dataRow.Cells.Add(Payment);

            return dataRow;
        }


        private TableRow BillingOutputTableRowRooms(BillingViewModel viewModel, string roomname, double payment, string item, bool FontWeightBold = false)
        {
            TableRow dataRow = new TableRow();

            TableCell Year = new TableCell();
            Year.TextAlignment = TextAlignment.Left;

            TableCell RoomName = new TableCell();
            TableCell Item = new TableCell();

            TableCell Payment = new TableCell();
            Payment.TextAlignment = TextAlignment.Right;

            Year.Blocks.Add(new Paragraph(new Run()));
            RoomName.Blocks.Add(new Paragraph(new Run($"{viewModel.StartDate.Year}")));
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


        private Block BillingPlanTable(BillingViewModel viewModel)
        {
            PrintOutputBase print = new PrintOutputBase(viewModel.GetFlatViewModel(), _SelectedYear);

            Table billingPlanTable = print.OutputTableForFlatBilling();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            dataRowGroup.Rows.Add(print.SeparatorTextTableRow("consumption"));

            foreach (ConsumptionItemViewModel item in viewModel.ConsumptionItemViewModels)
            {
                dataRowGroup.Rows.Add(BillingOutputTableRow(
                    viewModel,
                    item.ConsumedUnits,
                    item.ConsumptionCause,
                    false,
                    false));
            }

            if (viewModel.HasPayments)
            {
                dataRowGroup.Rows.Add(print.SeparatorTextTableRow("advances"));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalAdvancePerPeriod, "calculated advances"));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.ActualAdvancePerPeriod, "actual advances"));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalPayments, "payments"));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalPayments - viewModel.ActualAdvancePerPeriod, "payments - advances"));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalPayments - viewModel.TotalRentCosts, "payments - rent costs"));

                dataRowGroup.Rows.Add(print.SeparatorLineTableRow(true));
                dataRowGroup.Rows.Add(print.SeparatorTextTableRow("contract costs"));

                //if (viewModel.GetFlatViewModel().RentUpdates.Count > 0)
                //{
                //    dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalRentCosts, viewModel.GetFlatViewModel().RentUpdates[0].Rent.ColdRent.TransactionItem));
                //}
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalFixedCostsPerPeriod, viewModel.Billing.TotalFixedCostsPerPeriod.TransactionItem));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalHeatingCostsPerPeriod, viewModel.Billing.TotalHeatingCostsPerPeriod.TransactionItem));

                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalCostsPerPeriod, "sum", true));

                dataRowGroup.Rows.Add(print.SeparatorTextTableRow("contract costs balance"));

                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalPayments, "payments"));

                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, -1 * viewModel.TotalCostsPerPeriodIncludingRent, "contract costs"));

                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalPayments - viewModel.TotalCostsPerPeriodIncludingRent, "balance", true));

                dataRowGroup.Rows.Add(print.SeparatorLineTableRow(true));
                dataRowGroup.Rows.Add(print.SeparatorTextTableRow("all costs"));

                if (viewModel.GetFlatViewModel().RentUpdates.Count > 0)
                {
                    dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalRentCosts, viewModel.GetFlatViewModel().RentUpdates[0].Rent.ColdRent.TransactionItem));
                }
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalFixedCostsPerPeriod, viewModel.Billing.TotalFixedCostsPerPeriod.TransactionItem));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalHeatingCostsPerPeriod, viewModel.Billing.TotalHeatingCostsPerPeriod.TransactionItem));

                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.OtherFTISum, "other"));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, -1 * viewModel.CreditSum, "credit"));

                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalCostsPerPeriodIncludingRent, "sum", true));

                dataRowGroup.Rows.Add(print.SeparatorTextTableRow("all costs balance"));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalPayments, "payments"));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, -1 * viewModel.TotalCostsNoRent, "all costs"));

                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalPayments - viewModel.TotalCostsNoRent, "balance", true));
            }
            else
            {

                dataRowGroup.Rows.Add(print.SeparatorTextTableRow("advances"));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalAdvancePerPeriod, "calculated advances"));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.ActualAdvancePerPeriod, "actual advances"));


                dataRowGroup.Rows.Add(print.SeparatorLineTableRow(true));
                dataRowGroup.Rows.Add(print.SeparatorTextTableRow("contract costs"));

                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalFixedCostsPerPeriod, viewModel.Billing.TotalFixedCostsPerPeriod.TransactionItem));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalHeatingCostsPerPeriod, viewModel.Billing.TotalHeatingCostsPerPeriod.TransactionItem));

                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalCostsPerPeriod, "sum", true));

                
                dataRowGroup.Rows.Add(print.SeparatorTextTableRow("contract costs balance"));

                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.ActualAdvancePerPeriod, "actual advances"));

                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, -1 * viewModel.TotalCostsPerPeriod, "contract costs"));

                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.ActualAdvancePerPeriod - viewModel.TotalCostsPerPeriod, "balance", true));

                dataRowGroup.Rows.Add(print.SeparatorLineTableRow(true));
                dataRowGroup.Rows.Add(print.SeparatorTextTableRow("all costs"));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalFixedCostsPerPeriod, viewModel.Billing.TotalFixedCostsPerPeriod.TransactionItem));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalHeatingCostsPerPeriod, viewModel.Billing.TotalHeatingCostsPerPeriod.TransactionItem));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.OtherFTISum, "other"));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, -1 * viewModel.CreditSum, "credit"));

                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalCostsNoRent, "sum", true));


                dataRowGroup.Rows.Add(print.SeparatorTextTableRow("all costs balance"));
                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.ActualAdvancePerPeriod, "advances"));

                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, -1 * viewModel.TotalCostsNoRent, "all costs"));

                dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.ActualAdvancePerPeriod - viewModel.TotalCostsNoRent, "balance", true));


            }

            billingPlanTable.RowGroups.Add(dataRowGroup);

            return billingPlanTable;
        }


        public Section BuildBillingDetails()
        {
            Section billingOutput = new Section();

            if (_BillingViewModel != null)
            {
                billingOutput.Blocks.Add(BillingPlanTable(_BillingViewModel));

                foreach (RoomViewModel item in _BillingViewModel.FlatViewModel.Rooms)
                {
                    RoomCostShareBilling roomCostShareBilling = new RoomCostShareBilling(item.Room, _BillingViewModel);

                    billingOutput.Blocks.Add(BuildRoomSeparatorLine(roomCostShareBilling));


                    billingOutput.Blocks.Add(OutputConsumptionTableRooms(roomCostShareBilling));

                    billingOutput.Blocks.Add(BillingOutputTableRooms(roomCostShareBilling));

                }
            }

            return billingOutput;
        }


        private Paragraph BuildRoomSeparatorLine(IRoomCostShare roomCostShare)
        {
            if (_ShowTenant)
            {
                return new Paragraph(new Run($"{roomCostShare.RoomName} {roomCostShare.RoomArea}m²\n{roomCostShare.Tenant}"))
                { Background = new SolidColorBrush(Colors.LightGray) };
            }

            return new Paragraph(new Run($"{roomCostShare.RoomName} {roomCostShare.RoomArea}m²"))
            { Background = new SolidColorBrush(Colors.LightGray) };
        }


        private TableRow ConsumptionaOutputTableRowRooms(BillingViewModel viewModel, string roomname, double consumption, double totalConsumption, string item)
        {
            TableRow dataRow = new TableRow();

            TableCell Year = new TableCell();
            Year.TextAlignment = TextAlignment.Left;

            TableCell RoomName = new TableCell();
            TableCell Item = new TableCell();

            TableCell Consumption = new TableCell();
            Consumption.TextAlignment = TextAlignment.Right;

            Year.Blocks.Add(new Paragraph(new Run()));
            RoomName.Blocks.Add(new Paragraph(new Run($"{viewModel.StartDate.Year}")));
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


        private Block OutputConsumptionTableRooms(RoomCostShareBilling roomCostShareBilling)
        {
            PrintOutputBase print = new PrintOutputBase(_BillingViewModel.GetFlatViewModel(), _SelectedYear);

            Table billingTableRooms = BillingOutputTableForRooms();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            //dataRowGroup.Rows.Add(SeparatorLineTableRow(true));
            dataRowGroup.Rows.Add(print.SeparatorTextTableRow("consumption", true));

            dataRowGroup.Rows.Add(ConsumptionaOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.HeatingUnitsTotalConsumptionShare,
                _BillingViewModel.Billing.ConsumptionItems[0].ConsumedUnits,
                _BillingViewModel.Billing.TotalHeatingCostsPerPeriod.TransactionItem));

            foreach (RoomConsumptionViewModel roomConsumptionViewModel in roomCostShareBilling.ConsumptionItemViewModels)
            {
                if (roomConsumptionViewModel.RoomArea == roomCostShareBilling.RoomArea
                    && roomConsumptionViewModel.RoomName.Equals(roomCostShareBilling.RoomName))
                {

                    dataRowGroup.Rows.Add(ConsumptionaOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.RoomName,
                        roomConsumptionViewModel.TotalConsumptionValue,
                        roomConsumptionViewModel.ConsumptionItemViewModel.ConsumedUnits,
                        roomConsumptionViewModel.ConsumptionCause));
                }

            }


            billingTableRooms.RowGroups.Add(dataRowGroup);

            return billingTableRooms;
        }

        #endregion


    }
}
// EOF