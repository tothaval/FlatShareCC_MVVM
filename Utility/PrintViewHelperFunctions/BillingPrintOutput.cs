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
using SharedLivingCostCalculator.ViewModels;

namespace SharedLivingCostCalculator.Utility.PrintViewHelperFunctions
{
    public class BillingPrintOutput
    {

        // Properties & Fields
        #region Properties & Fields

        private BillingViewModel _BillingViewModel { get; }


        private PrintOutputBase _Print { get; }


        private PrintViewModel _PrintViewModel { get; }


        private int _SelectedYear { get; set; }


        private bool _ShowTenant { get; set; }

        #endregion


        // Constructors
        #region Constructors

        public BillingPrintOutput(PrintViewModel printViewModel, BillingViewModel billingViewModel, int selectedYear, bool showTenant)
        {
            _PrintViewModel = printViewModel;
            _BillingViewModel = billingViewModel;
            _SelectedYear = selectedYear;
            _ShowTenant = showTenant;

            _Print = new PrintOutputBase(_PrintViewModel, _BillingViewModel.FlatViewModel, _SelectedYear);
        }

        #endregion


        // Methods
        #region Methods

        private TableRowGroup AdvanceDifference(TableRowGroup dataRowGroup, BillingViewModel viewModel)
        {
            dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("advances"));
            dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, viewModel.ActualAdvancePerPeriod, "actual advances"));
            dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, viewModel.TotalAdvancePerPeriod, "calculated advances"));
            dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, viewModel.AdvanceDifference, "difference", true));

            return dataRowGroup;
        }


        private TableRowGroup AllCosts(TableRowGroup dataRowGroup, BillingViewModel viewModel)
        {
            double advance = 0.0;

            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs"));

                double sum = 0.0;

                (dataRowGroup, sum) = ContractCostsDisplay(dataRowGroup, viewModel, sum);


                foreach (FinancialTransactionItemBillingViewModel item in viewModel.FinancialTransactionItemViewModels)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, item.TransactionSum, item.TransactionItem));

                    sum += item.TransactionSum;
                }

                dataRowGroup = _Print.BuildTaxDisplayBilling(dataRowGroup, viewModel, sum, false);

                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs balance"));

                if (viewModel.HasPayments)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(
                        viewModel,
                        viewModel.TotalPayments,
                        "payments"));

                    advance += viewModel.TotalPayments;
                }
                else
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel,
                        viewModel.ActualAdvancePerPeriod,
                        "actual advances"));

                    advance += viewModel.ActualAdvancePerPeriod;
                }


                if (_PrintViewModel.ContractCostsIncludeCreditsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, -1 * viewModel.CreditSum, "credit"));

                }

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, -1 * viewModel.TotalCostsPerPeriodIncludingRent, "contract costs"));

                foreach (FinancialTransactionItemRentViewModel item in viewModel.Credits)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, item.TransactionSum, item.TransactionItem));

                    sum -= item.TransactionSum;
                }

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, advance - sum, "balance", true));
            }
            else
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs balance"));

                if (viewModel.HasPayments)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(
                        viewModel,
                        viewModel.TotalPayments,
                        "payments"));

                    advance += viewModel.TotalPayments;
                }
                else
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel,
                        viewModel.ActualAdvancePerPeriod,
                        "actual advances"));

                    advance += viewModel.ActualAdvancePerPeriod;
                }

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, -1 * viewModel.TotalCostsPerPeriodIncludingRent, "contract costs"));

                if (_PrintViewModel.ContractCostsIncludeCreditsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, -1 * viewModel.CreditSum, "credit"));

                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, advance - viewModel.TotalCostsPerPeriodIncludingRent + viewModel.CreditSum, "balance", true));
                }
                else
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, advance - viewModel.TotalCostsPerPeriodIncludingRent, "balance", true));
                }
            }


            return dataRowGroup;
        }


        private TableRowGroup AllCostsRooms(TableRowGroup dataRowGroup, RoomCostShareBilling roomCostShareBilling)
        {
            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs", true));

                double sum = 0.0;

                (dataRowGroup, sum) = ContractCostsRoomsDisplay(dataRowGroup, roomCostShareBilling, sum);

                foreach (FinancialTransactionItemBillingViewModel item in roomCostShareBilling.FinancialTransactionItemViewModels)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.RoomName,
                        item.TransactionSum,
                        item.TransactionItem));

                    sum += item.TransactionSum;
                }

                dataRowGroup = _Print.BuildTaxDisplayRoomsBilling(
                    dataRowGroup, _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    sum,
                    false);

                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs balance", true));
                dataRowGroup.Rows.Add(_Print.TableRowBillingHeader());

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.Advances,
                    "advances"));

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    -1 * sum,
                    "all costs"));


                foreach (FinancialTransactionItemRentViewModel item in roomCostShareBilling.Credits)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.RoomName,
                        -1 * item.TransactionSum,
                        item.TransactionItem));

                    sum -= item.TransactionSum;
                }

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.Advances - sum,
                    "balance", true));
            }
            else
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs balance", true));
                dataRowGroup.Rows.Add(_Print.TableRowBillingHeader());

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.Advances,
                    "advances"));

                if (_PrintViewModel.RentCostsOnBillingBalanceSelected)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.RoomName,
                        -1 * roomCostShareBilling.TotalCostsAnnualCostsShare,
                        "all costs"));

                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.RoomName,
                        roomCostShareBilling.CreditShare,
                        "credit"));

                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.RoomName,
                        roomCostShareBilling.Advances - roomCostShareBilling.TotalCostsAnnualCostsShare +
                        roomCostShareBilling.CreditShare,
                        "balance", true));
                }
                else
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.RoomName,
                        -1 * roomCostShareBilling.TotalCostsAnnualCostsShareNoRent,
                        "all costs"));

                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.RoomName,
                        roomCostShareBilling.CreditShare,
                        "credit"));

                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.RoomName,
                        roomCostShareBilling.Advances - roomCostShareBilling.TotalCostsAnnualCostsShareNoRent +
                        roomCostShareBilling.CreditShare,
                        "balance", true));
                }

            }

            dataRowGroup.Rows.Add(_Print.SeparatorLineTableRow(true));

            return dataRowGroup;
        }


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


        private Table BillingOutputTableRooms(RoomCostShareBilling roomCostShareBilling)
        {
            Table billingTableRooms = BillingOutputTableForRooms();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            if (_PrintViewModel.ContractCostsSelected)
            {
                dataRowGroup = ContractCostsRooms(dataRowGroup, roomCostShareBilling);
            }

            if (_PrintViewModel.AllCostsSelected)
            {

                dataRowGroup = AllCostsRooms(dataRowGroup, roomCostShareBilling);
            }

            if (_PrintViewModel.RentCostsOutputOnBillingSelected)
            {
                dataRowGroup = NewCostsRoomsFromCosts(dataRowGroup, roomCostShareBilling);
            }

            billingTableRooms.RowGroups.Add(dataRowGroup);

            return billingTableRooms;
        }


        private Block BillingPlanTable(BillingViewModel viewModel)
        {
            Table billingPlanTable = _Print.OutputTableForFlatBilling();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            dataRowGroup = AdvanceDifference(dataRowGroup, viewModel);

            if (_PrintViewModel.ConsumptionSelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("consumption"));

                foreach (ConsumptionItemViewModel item in viewModel.ConsumptionItemViewModels)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(
                        viewModel,
                        item.ConsumedUnits,
                        item.ConsumptionCause,
                        false,
                        false));
                }
            }

            if (_PrintViewModel.ContractCostsSelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorLineTableRow(true));
                dataRowGroup = ContractCosts(dataRowGroup, viewModel);
            }

            if (_PrintViewModel.AllCostsSelected)
            {
                dataRowGroup = AllCosts(dataRowGroup, viewModel);
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
                    

                    if (_PrintViewModel.ConsumptionSelected)
                    {
                        billingOutput.Blocks.Add(OutputConsumptionTableRooms(roomCostShareBilling));
                    }

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


        private TableRowGroup ContractCosts(TableRowGroup dataRowGroup, BillingViewModel viewModel)
        {

            double advance = 0.0;

            if (!_PrintViewModel.DisplaySummarySelected)
            {
                double sum = 0.0;

                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs"));

                (dataRowGroup, sum) = ContractCostsDisplay(dataRowGroup, viewModel, sum);

                dataRowGroup = _Print.BuildTaxDisplayBilling(dataRowGroup, viewModel, sum, false);

                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs balance"));

                if (viewModel.HasPayments)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(
                        viewModel,
                        viewModel.TotalPayments,
                        "payments"));

                    advance += viewModel.TotalPayments;
                }
                else
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel,
                        viewModel.ActualAdvancePerPeriod,
                        "actual advances"));

                    advance += viewModel.ActualAdvancePerPeriod;
                }


                if (_PrintViewModel.ContractCostsIncludeCreditsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, -1 * viewModel.CreditSum, "credit"));

                }

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, -1 * viewModel.TotalCostsPerPeriodIncludingRent, "contract costs"));

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, advance - sum, "balance", true));

                dataRowGroup.Rows.Add(_Print.SeparatorLineTableRow(true));
            }
            else
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs balance"));

                if (viewModel.HasPayments)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(
                        viewModel,
                        viewModel.TotalPayments,
                        "payments"));

                    advance += viewModel.TotalPayments;
                }
                else
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel,
                        viewModel.ActualAdvancePerPeriod,
                        "actual advances"));

                    advance += viewModel.ActualAdvancePerPeriod;
                }

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, -1 * viewModel.TotalCostsPerPeriodIncludingRent, "contract costs"));

                if (_PrintViewModel.ContractCostsIncludeCreditsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, -1 * viewModel.CreditSum, "credit"));
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, advance - viewModel.TotalCostsPerPeriodIncludingRent + viewModel.CreditSum, "balance", true));
                }
                else
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, advance - viewModel.TotalCostsPerPeriodIncludingRent, "balance", true));
                }

            }

            return dataRowGroup;
        }


        private (TableRowGroup, double) ContractCostsDisplay(TableRowGroup dataRowGroup, BillingViewModel viewModel, double sum)
        {
            if (_BillingViewModel.HasPayments && _PrintViewModel.RentCostsOnBillingBalanceSelected)
            {
                if (_BillingViewModel.GetFlatViewModel().RentUpdates.Count > 0)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(
                        viewModel,
                        viewModel.TotalRentCosts,
                        viewModel.GetFlatViewModel().RentUpdates[0].Rent.ColdRent.TransactionItem)
                        );

                    sum += viewModel.TotalRentCosts;
                }
            }

            dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(
                viewModel,
                viewModel.TotalFixedCostsPerPeriod,
                viewModel.Billing.TotalFixedCostsPerPeriod.TransactionItem)
                );

            sum += viewModel.TotalFixedCostsPerPeriod;

            dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(
                viewModel,
                viewModel.ColdWaterCosts,
                viewModel.Billing.ColdWaterCosts.TransactionItem)
                );

            sum += viewModel.ColdWaterCosts;


            dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(
                viewModel,
                viewModel.WarmWaterCosts,
                viewModel.Billing.WarmWaterCosts.TransactionItem)
                );

            sum += viewModel.WarmWaterCosts;


            dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(
                viewModel,
                viewModel.BasicHeatingCosts,
                viewModel.Billing.BasicHeatingCosts.TransactionItem)
                );

            sum += viewModel.BasicHeatingCosts;


            dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(
                viewModel,
                viewModel.ConsumptionHeatingCosts,
                viewModel.Billing.ConsumptionHeatingCosts.TransactionItem)
                );

            sum += viewModel.ConsumptionHeatingCosts;

            return (dataRowGroup, sum);
        }


        private TableRowGroup ContractCostsRooms(TableRowGroup dataRowGroup, RoomCostShareBilling roomCostShareBilling)
        {
            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs", true));

                double sum = 0.0;

                (dataRowGroup, sum) = ContractCostsRoomsDisplay(dataRowGroup, roomCostShareBilling, sum);

                dataRowGroup = _Print.BuildTaxDisplayRoomsBilling(
                    dataRowGroup, _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    sum, false);

                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs balance", true));
                dataRowGroup.Rows.Add(_Print.TableRowBillingHeader());

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.Advances,
                    "advances"));

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    -1 * sum,
                    "contract costs"));

                if (_PrintViewModel.ContractCostsIncludeCreditsSelected)
                {
                    foreach (FinancialTransactionItemRentViewModel item in roomCostShareBilling.Credits)
                    {
                        dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                            _BillingViewModel,
                            roomCostShareBilling.RoomName,
                            item.TransactionSum,
                            item.TransactionItem));

                        sum -= item.TransactionSum;
                    }
                }

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.Advances - sum,
                    "balance", true));
            }
            else
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs balance", true));
                dataRowGroup.Rows.Add(_Print.TableRowBillingHeader());

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.Advances,
                    "advances"));

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    -1 * roomCostShareBilling.TotalContractCostsShare,
                    "contract costs"));

                if (_PrintViewModel.ContractCostsIncludeCreditsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.RoomName,
                        roomCostShareBilling.CreditShare,
                        "credit"));

                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.RoomName,
                        roomCostShareBilling.Advances - roomCostShareBilling.TotalContractCostsShare + roomCostShareBilling.CreditShare,
                        "balance", true));
                }
                else
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.RoomName,
                        roomCostShareBilling.Advances - roomCostShareBilling.TotalContractCostsShare,
                        "balance", true));
                }
            }

            dataRowGroup.Rows.Add(_Print.SeparatorLineTableRow(true));

            return dataRowGroup;
        }


        private (TableRowGroup, double) ContractCostsRoomsDisplay(TableRowGroup dataRowGroup, RoomCostShareBilling roomCostShareBilling, double sum)
        {
            dataRowGroup.Rows.Add(_Print.TableRowBillingHeader());

            if (_BillingViewModel.HasPayments && _PrintViewModel.RentCostsOnBillingBalanceSelected)
            {
                if (_BillingViewModel.GetFlatViewModel().RentUpdates.Count > 0)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.RoomName,
                        roomCostShareBilling.RentCostsAnnualShare,
                        _BillingViewModel.GetFlatViewModel().RentUpdates[0].Rent.ColdRent.TransactionItem));

                    sum += roomCostShareBilling.RentCostsAnnualShare;
                }
            }

            // pro rata
            dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.ProRataAmounts,
                _BillingViewModel.Billing.TotalFixedCostsPerPeriod.TransactionItem));

            sum += roomCostShareBilling.ProRataAmounts;

            // cold water
            dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.ColdWaterCostsShare,
                _BillingViewModel.Billing.ColdWaterCosts.TransactionItem));

            sum += roomCostShareBilling.ColdWaterCostsShare;

            // warm water
            dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.WarmWaterCostsShare,
                _BillingViewModel.Billing.WarmWaterCosts.TransactionItem));

            sum += roomCostShareBilling.WarmWaterCostsShare;

            // basic heating costs
            dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.BasicHeatingCostsShare,
                _BillingViewModel.Billing.TotalHeatingCostsPerPeriod.TransactionItem));

            sum += roomCostShareBilling.BasicHeatingCostsShare;

            // consumption heating costs
            dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.ConsumptionHeatingCostsShare,
                _BillingViewModel.Billing.ConsumptionHeatingCosts.TransactionItem));

            sum += roomCostShareBilling.ConsumptionHeatingCostsShare;

            return (dataRowGroup, sum);
        }


        private TableRowGroup NewCostsRoomsFromCosts(TableRowGroup dataRowGroup, RoomCostShareBilling roomCostShareBilling)
        {
            double months = _BillingViewModel.GetMonths();

            double newPrice = roomCostShareBilling.ProRataAmounts + roomCostShareBilling.FixedAmountShare;

            double newPricePerMonth = newPrice / months;

            dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("new monthly advances based on annual billing costs", true));
            if (!_PrintViewModel.DisplaySummarySelected)
            {                
                dataRowGroup.Rows.Add(_Print.TableRowBillingHeader());

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.ProRataAmounts,
                    "fixed"));

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.FixedAmountShare,
                    "heating"));


                dataRowGroup = _Print.BuildTaxDisplayRoomsBilling(
                    dataRowGroup,
                    _BillingViewModel,
                    "annual costs",
                    newPrice,
                    false);

                dataRowGroup.Rows.Add(_Print.SeparatorLineTableRow(true));

                dataRowGroup = _Print.BuildTaxDisplayRoomsBilling(
                    dataRowGroup,
                    _BillingViewModel,
                    "new monthly advance",
                    newPricePerMonth,
                    true);
            }
            else
            {
                dataRowGroup = _Print.BuildTaxDisplayRoomsBilling(
                    dataRowGroup,
                    _BillingViewModel,
                    $"annual costs",
                    newPrice,
                    true);

                dataRowGroup.Rows.Add(_Print.SeparatorLineTableRow(true));

                dataRowGroup = _Print.BuildTaxDisplayRoomsBilling(
                    dataRowGroup,
                    _BillingViewModel, $"new monthly advance",
                    newPricePerMonth,
                    true);
            }

            return dataRowGroup;
        }


        private Block OutputConsumptionTableRooms(RoomCostShareBilling roomCostShareBilling)
        {
            PrintOutputBase print = new PrintOutputBase(_PrintViewModel, _BillingViewModel.GetFlatViewModel(), _SelectedYear);

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