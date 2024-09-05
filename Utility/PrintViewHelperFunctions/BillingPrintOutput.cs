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
            dataRowGroup.Rows.Add(_Print.TableRowBillingHeaderFlat(false));

            dataRowGroup.Rows.Add(
                _Print.BillingOutputTableRow(
                    viewModel,
                    viewModel.ActualAdvancePerPeriod,
                    "actual advances"
                    )
                );

            dataRowGroup.Rows.Add(
                _Print.BillingOutputTableRow(
                    viewModel,
                    viewModel.TotalAdvancePerPeriod,
                    "calculated advances"
                    )                
                );

            dataRowGroup.Rows.Add(
                _Print.BillingOutputTableRow(
                    viewModel,
                    viewModel.AdvanceDifference,
                    "difference",
                    "",
                    true
                    )
                );

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

                if (_PrintViewModel.DetailedNonContractItemsSelected)
                {
                    foreach (FinancialTransactionItemBillingViewModel item in viewModel.FinancialTransactionItemViewModels)
                    {
                        dataRowGroup.Rows.Add(
                            _Print.BillingOutputTableRow(
                                viewModel,
                                item.TransactionSum,
                                item.TransactionItem,
                                item.TransactionShareTypes.ToString()
                                )
                            );

                        sum += item.TransactionSum;
                    } 
                }
                else
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            viewModel.OtherFTISum,
                            "other costs"
                            )
                        );

                    sum += viewModel.OtherFTISum;
                }

                dataRowGroup = _Print.BuildTaxDisplayBilling(dataRowGroup, viewModel, sum, false);

                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs balance"));
                dataRowGroup.Rows.Add(_Print.TableRowHeader(false));

                if (viewModel.HasPayments)
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            viewModel.TotalPayments,
                            "payments"
                            )
                        );

                    advance += viewModel.TotalPayments;
                }
                else
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            viewModel.ActualAdvancePerPeriod,
                            "actual advances"
                            )
                        );

                    advance += viewModel.ActualAdvancePerPeriod;
                }


                if (_PrintViewModel.ContractCostsIncludeCreditsSelected)
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            -1 * viewModel.CreditSum,
                            "credit"
                            )
                        );

                }

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRow(viewModel, -1 * viewModel.TotalCostsPerPeriodIncludingRent, "contract costs"));

                if (_PrintViewModel.DetailedNonContractItemsSelected)
                {
                    foreach (FinancialTransactionItemRentViewModel item in viewModel.Credits)
                    {
                        dataRowGroup.Rows.Add(
                            _Print.BillingOutputTableRow(
                                viewModel,
                                -1 * item.TransactionSum,
                                item.TransactionItem,
                                item.TransactionShareTypes.ToString()
                                )
                            );

                        sum -= item.TransactionSum;
                    } 
                }
                else
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            -1 * viewModel.CreditSum,
                            "credit"
                            )
                        );
                }

                dataRowGroup.Rows.Add(
                    _Print.BillingOutputTableRow(
                        viewModel,
                        advance
                        - sum,
                        "balance",
                        "",
                        true
                        )
                    );
            }
            else
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs balance"));

                if (viewModel.HasPayments)
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            viewModel.TotalPayments,
                            "payments"
                            )
                        );

                    advance += viewModel.TotalPayments;
                }
                else
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            viewModel.ActualAdvancePerPeriod,
                            "actual advances"
                            )
                        );

                    advance += viewModel.ActualAdvancePerPeriod;
                }

                dataRowGroup.Rows.Add(
                    _Print.BillingOutputTableRow(
                        viewModel,
                        -1 * viewModel.TotalCostsPerPeriodIncludingRent,
                        "contract costs"
                        )
                    );

                if (_PrintViewModel.ContractCostsIncludeCreditsSelected)
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            -1 * viewModel.CreditSum,
                            "credit"
                            )
                        );

                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            advance - viewModel.TotalCostsPerPeriodIncludingRent + viewModel.CreditSum,
                            "balance",
                            "",
                            true
                            )
                        );
                }
                else
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            advance - viewModel.TotalCostsPerPeriodIncludingRent,
                            "balance",
                            "",
                            true
                            )
                        );
                }
            }


            return dataRowGroup;
        }


        private TableRowGroup AllCostsRooms(TableRowGroup dataRowGroup, RoomCostShareBilling roomCostShareBilling)
        {
            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs"));

                double sum = 0.0;

                (dataRowGroup, sum) = ContractCostsRoomsDisplay(dataRowGroup, roomCostShareBilling, sum);

                if (_PrintViewModel.DetailedNonContractItemsSelected)
                {
                    foreach (FinancialTransactionItemBillingViewModel item in roomCostShareBilling.FinancialTransactionItemViewModels)
                    {
                        dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                            _BillingViewModel,
                            item.TransactionSum,
                            item.TransactionItem,
                            item.TransactionShareTypes.ToString()));

                        sum += item.TransactionSum;
                    } 
                }
                else
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.OtherCostsAnnualCostsShare,
                        "other costs"));

                    sum += roomCostShareBilling.OtherCostsAnnualCostsShare;
                }

                dataRowGroup = _Print.BuildTaxDisplayRoomsBilling(
                    dataRowGroup, _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    sum,
                    false);

                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs balance"));
                dataRowGroup.Rows.Add(_Print.TableRowHeader(false));

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.Advances,
                    "advances"));

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    -1 * sum,
                    "all costs"));


                if (_PrintViewModel.DetailedNonContractItemsSelected)
                {
                    foreach (FinancialTransactionItemRentViewModel item in roomCostShareBilling.Credits)
                    {
                        dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                            _BillingViewModel,
                            -1 * item.TransactionSum,
                            item.TransactionItem,
                            item.TransactionShareTypes.ToString()));

                        sum -= item.TransactionSum;
                    } 
                }
                else
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        -1 * roomCostShareBilling.OtherCostsAnnualCostsShare,
                        "credits"));

                    sum -= roomCostShareBilling.CreditShare;
                }

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.Advances - sum,
                    "balance", "", true));
            }
            else
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs balance"));
                dataRowGroup.Rows.Add(_Print.TableRowHeader(false));

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.Advances,
                    "advances"));

                if (_PrintViewModel.RentCostsOnBillingBalanceSelected)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        -1 * roomCostShareBilling.TotalCostsAnnualCostsShare,
                        "all costs"));

                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.CreditShare,
                        "credit"));

                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.Advances - roomCostShareBilling.TotalCostsAnnualCostsShare +
                        roomCostShareBilling.CreditShare,
                        "balance", "", true));
                }
                else
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        -1 * roomCostShareBilling.TotalCostsAnnualCostsShareNoRent,
                        "all costs"));

                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.CreditShare,
                        "credit"));

                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.Advances - roomCostShareBilling.TotalCostsAnnualCostsShareNoRent +
                        roomCostShareBilling.CreditShare,
                        "balance", "", true));
                }

            }

            dataRowGroup.Rows.Add(_Print.SeparatorLineTableRow(true));

            return dataRowGroup;
        }


        private Table BillingOutputTableRooms(RoomCostShareBilling roomCostShareBilling)
        {
            Table billingTableRooms = _Print.OutputTableRooms();

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
            Table billingPlanTable = _Print.OutputTable();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            dataRowGroup = AdvanceDifference(dataRowGroup, viewModel);

            if (_PrintViewModel.ConsumptionSelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("consumption"));
                dataRowGroup.Rows.Add(_Print.TableRowBillingHeaderConsumption(true));

                foreach (ConsumptionItemViewModel item in viewModel.ConsumptionItemViewModels)
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            item.ConsumedUnits,
                            item.ConsumptionCause,
                            "",
                            false,
                            false
                            )
                        );
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

                billingOutput.Blocks.Add(_Print.BuildHeader($"Annual Billing {_BillingViewModel.Year}"));

                if (_PrintViewModel.PrintAllSelected || _PrintViewModel.PrintFlatSelected)
                {
                    billingOutput.Blocks.Add(BillingPlanTable(_BillingViewModel)); 
                }

                if (_PrintViewModel.PrintExcerptSelected || _PrintViewModel.PrintAllSelected || _PrintViewModel.PrintRoomsSelected)
                {
                    double contractCostsSum = 0.0;
                    double allCostsSum = 0.0;

                    foreach (RoomViewModel item in _BillingViewModel.FlatViewModel.Rooms)
                    {
                        bool printThisRoom = new Compute().PrintThisRoom(_PrintViewModel, item);

                        RoomCostShareBilling roomCostShareBilling = new RoomCostShareBilling(item.Room, _BillingViewModel);

                        if (printThisRoom)
                        {
                            billingOutput.Blocks.Add(_Print.RoomSeparatorLine(roomCostShareBilling, _ShowTenant, $"Annual Billing {_BillingViewModel.Year}"));

                            if (_PrintViewModel.ConsumptionSelected)
                            {
                                billingOutput.Blocks.Add(OutputConsumptionTableRooms(roomCostShareBilling));

                            }

                            billingOutput.Blocks.Add(BillingOutputTableRooms(roomCostShareBilling));
                        }


                        if (_PrintViewModel.ContractCostsSelected)
                        {
                            contractCostsSum += Math.Round(roomCostShareBilling.TotalContractCostsShare, 2);
                        }

                        if (_PrintViewModel.AllCostsSelected)
                        {
                            allCostsSum += Math.Round(roomCostShareBilling.TotalCostsAnnualCostsShare, 2);
                        }

                    }

                    if (_PrintViewModel.ContractCostsSelected)
                    {
                        Table table = new Table();

                        TableRowGroup tableRowGroup = new TableRowGroup();
                        tableRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

                        tableRowGroup = _Print.WriteCheckToTableRowGroup(
                            tableRowGroup,
                            contractCostsSum,
                            _BillingViewModel.TotalCostsPerPeriod,
                            "check: contract costs",
                            false);

                        table.RowGroups.Add(tableRowGroup);

                        billingOutput.Blocks.Add(table);
                    }

                    if (_PrintViewModel.AllCostsSelected)
                    {
                        Table table = new Table();

                        TableRowGroup tableRowGroup = new TableRowGroup();
                        tableRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

                        tableRowGroup = _Print.WriteCheckToTableRowGroup(
                            tableRowGroup,
                            allCostsSum,
                            _BillingViewModel.TotalCostsNoRent,
                            "check: all costs",
                            false);

                        table.RowGroups.Add(tableRowGroup);

                        billingOutput.Blocks.Add(table);
                    }
                }
            }

            return billingOutput;
        }


        private TableRow ConsumptionaOutputTableRowRooms(BillingViewModel viewModel, string roomname, double consumption, double totalConsumption, string item)
        {
            TableRow dataRow = new TableRow();

            TableCell Item = new TableCell();
            Item.ColumnSpan = 4;

            TableCell Consumption = new TableCell();
            Consumption.TextAlignment = TextAlignment.Right;

            Item.Blocks.Add(new Paragraph(new Run(item)));

            Paragraph paymentParagraph = new Paragraph(new Run($"{consumption:N2} ({consumption / totalConsumption * 100:N2}%)"))
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(0, 0, 10, 0)
            };

            Consumption.Blocks.Add(paymentParagraph);

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
                dataRowGroup.Rows.Add(_Print.TableRowBillingHeaderFlat(false));

                if (viewModel.HasPayments)
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            viewModel.TotalPayments,
                            "payments"
                            )
                        );

                    advance += viewModel.TotalPayments;
                }
                else
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            viewModel.ActualAdvancePerPeriod,
                            "actual advances"
                            )
                        );

                    advance += viewModel.ActualAdvancePerPeriod;
                }


                if (_PrintViewModel.ContractCostsIncludeCreditsSelected)
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            -1 * viewModel.CreditSum,
                            "credit"
                            )
                        );

                }

                dataRowGroup.Rows.Add(
                    _Print.BillingOutputTableRow(
                        viewModel,
                        -1 * viewModel.TotalCostsPerPeriodIncludingRent,
                        "contract costs"
                        )
                    );

                dataRowGroup.Rows.Add(
                    _Print.BillingOutputTableRow(
                        viewModel,
                        advance - sum,
                        "balance",
                        "",
                        true
                        )
                    );

                dataRowGroup.Rows.Add(_Print.SeparatorLineTableRow(true));
            }
            else
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs balance"));

                if (viewModel.HasPayments)
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            viewModel.TotalPayments,
                            "payments"
                            )
                        );

                    advance += viewModel.TotalPayments;
                }
                else
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            viewModel.ActualAdvancePerPeriod,
                            "actual advances"
                            )
                        );

                    advance += viewModel.ActualAdvancePerPeriod;
                }

                dataRowGroup.Rows.Add(
                    _Print.BillingOutputTableRow(
                        viewModel,
                        -1 * viewModel.TotalCostsPerPeriodIncludingRent,
                        "contract costs"
                        )
                    );

                if (_PrintViewModel.ContractCostsIncludeCreditsSelected)
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            -1 * viewModel.CreditSum,
                            "credit"
                            )
                        );

                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            advance - viewModel.TotalCostsPerPeriodIncludingRent + viewModel.CreditSum,
                            "balance",
                            "",
                            true
                            )
                        );
                }
                else
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            advance - viewModel.TotalCostsPerPeriodIncludingRent,
                            "balance",
                            "",
                            true
                            )
                        );
                }

            }

            return dataRowGroup;
        }


        private (TableRowGroup, double) ContractCostsDisplay(TableRowGroup dataRowGroup, BillingViewModel viewModel, double sum)
        {

            dataRowGroup.Rows.Add(_Print.TableRowBillingHeaderFlat(true));

            if (_BillingViewModel.HasPayments && _PrintViewModel.RentCostsOnBillingBalanceSelected)
            {
                if (_BillingViewModel.GetFlatViewModel().RentUpdates.Count > 0)
                {
                    dataRowGroup.Rows.Add(
                        _Print.BillingOutputTableRow(
                            viewModel,
                            viewModel.TotalRentCosts,
                            viewModel.GetFlatViewModel().RentUpdates[0].Rent.ColdRent.TransactionItem,
                            viewModel.GetFlatViewModel().RentUpdates[0].Rent.ColdRent.TransactionShareTypes.ToString()
                            )
                        );

                    sum += viewModel.TotalRentCosts;
                }
            }

            dataRowGroup.Rows.Add(
                _Print.BillingOutputTableRow(
                    viewModel,
                    viewModel.ProRataCosts,
                    viewModel.Billing.ProRataCosts.TransactionItem,
                    viewModel.Billing.ProRataCosts.TransactionShareTypes.ToString()
                    )
                );

            sum += viewModel.ProRataCosts;

            dataRowGroup.Rows.Add(
                _Print.BillingOutputTableRow(
                    viewModel,
                    viewModel.ColdWaterCosts,
                    viewModel.Billing.ColdWaterCosts.TransactionItem,
                    viewModel.Billing.ColdWaterCosts.TransactionShareTypes.ToString()
                    )
                );

            sum += viewModel.ColdWaterCosts;


            dataRowGroup.Rows.Add(
                _Print.BillingOutputTableRow(
                    viewModel,
                    viewModel.WarmWaterCosts,
                    viewModel.Billing.WarmWaterCosts.TransactionItem,
                    viewModel.Billing.WarmWaterCosts.TransactionShareTypes.ToString()
                    )
                );

            sum += viewModel.WarmWaterCosts;


            dataRowGroup.Rows.Add(
                _Print.BillingOutputTableRow(
                    viewModel,
                    viewModel.BasicHeatingCosts,
                    viewModel.Billing.BasicHeatingCosts.TransactionItem,
                    viewModel.Billing.BasicHeatingCosts.TransactionShareTypes.ToString()
                    )
                );

            sum += viewModel.BasicHeatingCosts;


            dataRowGroup.Rows.Add(
                _Print.BillingOutputTableRow(
                    viewModel,
                    viewModel.ConsumptionHeatingCosts,
                    viewModel.Billing.ConsumptionHeatingCosts.TransactionItem,
                    viewModel.Billing.ConsumptionHeatingCosts.TransactionShareTypes.ToString()
                    )
                );

            sum += viewModel.ConsumptionHeatingCosts;

            return (dataRowGroup, sum);
        }


        private TableRowGroup ContractCostsRooms(TableRowGroup dataRowGroup, RoomCostShareBilling roomCostShareBilling)
        {
            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs"));

                double sum = 0.0;

                (dataRowGroup, sum) = ContractCostsRoomsDisplay(dataRowGroup, roomCostShareBilling, sum);

                dataRowGroup = _Print.BuildTaxDisplayRoomsBilling(
                    dataRowGroup, _BillingViewModel,
                    roomCostShareBilling.RoomName,
                    sum, false);

                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs balance"));
                dataRowGroup.Rows.Add(_Print.TableRowHeader(false));

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.Advances,
                    "advances"));

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    -1 * sum,
                    "contract costs"));

                if (_PrintViewModel.ContractCostsIncludeCreditsSelected)
                {
                    foreach (FinancialTransactionItemRentViewModel item in roomCostShareBilling.Credits)
                    {
                        dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                            _BillingViewModel,
                            item.TransactionSum,
                            item.TransactionItem,
                            item.TransactionShareTypes.ToString()));

                        sum -= item.TransactionSum;
                    }
                }

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.Advances - sum,
                    "balance", "", true));
            }
            else
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs balance"));
                dataRowGroup.Rows.Add(_Print.TableRowHeader(false));

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.Advances,
                    "advances"));

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    -1 * roomCostShareBilling.TotalContractCostsShare,
                    "contract costs"));

                if (_PrintViewModel.ContractCostsIncludeCreditsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.CreditShare,
                        "credit"));

                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.Advances - roomCostShareBilling.TotalContractCostsShare + roomCostShareBilling.CreditShare,
                        "balance", "", true));
                }
                else
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.Advances - roomCostShareBilling.TotalContractCostsShare,
                        "balance", "", true));
                }
            }

            dataRowGroup.Rows.Add(_Print.SeparatorLineTableRow(true));

            return dataRowGroup;
        }


        private (TableRowGroup, double) ContractCostsRoomsDisplay(TableRowGroup dataRowGroup, RoomCostShareBilling roomCostShareBilling, double sum)
        {
            dataRowGroup.Rows.Add(_Print.TableRowHeader(true));

            if (_BillingViewModel.HasPayments && _PrintViewModel.RentCostsOnBillingBalanceSelected)
            {
                if (_BillingViewModel.GetFlatViewModel().RentUpdates.Count > 0)
                {
                    dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                        _BillingViewModel,
                        roomCostShareBilling.RentCostsAnnualShare,
                        _BillingViewModel.GetFlatViewModel().RentUpdates[0].Rent.ColdRent.TransactionItem,
                        _BillingViewModel.GetFlatViewModel().RentUpdates[0].Rent.ColdRent.TransactionShareTypes.ToString()));

                    sum += roomCostShareBilling.RentCostsAnnualShare;
                }
            }

            // pro rata
            dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.ProRataAmounts,
                _BillingViewModel.Billing.ProRataCosts.TransactionItem,
                _BillingViewModel.Billing.ProRataCosts.TransactionShareTypes.ToString()));

            sum += roomCostShareBilling.ProRataAmounts;

            // cold water
            dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.ColdWaterCostsShare,
                _BillingViewModel.Billing.ColdWaterCosts.TransactionItem,
                _BillingViewModel.Billing.ColdWaterCosts.TransactionShareTypes.ToString()));

            sum += roomCostShareBilling.ColdWaterCostsShare;

            // warm water
            dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.WarmWaterCostsShare,
                _BillingViewModel.Billing.WarmWaterCosts.TransactionItem,
                _BillingViewModel.Billing.WarmWaterCosts.TransactionShareTypes.ToString()));

            sum += roomCostShareBilling.WarmWaterCostsShare;

            // basic heating costs
            dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.BasicHeatingCostsShare,
                _BillingViewModel.Billing.BasicHeatingCosts.TransactionItem,
                _BillingViewModel.Billing.BasicHeatingCosts.TransactionShareTypes.ToString()));

            sum += roomCostShareBilling.BasicHeatingCostsShare;

            // consumption heating costs
            dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.ConsumptionHeatingCostsShare,
                _BillingViewModel.Billing.ConsumptionHeatingCosts.TransactionItem,
                _BillingViewModel.Billing.ConsumptionHeatingCosts.TransactionShareTypes.ToString()));

            sum += roomCostShareBilling.ConsumptionHeatingCostsShare;

            return (dataRowGroup, sum);
        }


        private TableRowGroup NewCostsRoomsFromCosts(TableRowGroup dataRowGroup, RoomCostShareBilling roomCostShareBilling)
        {
            double months = _BillingViewModel.GetMonths();

            double newPrice = roomCostShareBilling.ProRataAmounts + roomCostShareBilling.FixedAmountShare;

            double newPricePerMonth = newPrice / months;

            dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("new monthly advances based on annual billing costs"));
            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.TableRowHeader(false));

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
                    roomCostShareBilling.ProRataAmounts,
                    "fixed",
                    ""));

                dataRowGroup.Rows.Add(_Print.BillingOutputTableRowRooms(
                    _BillingViewModel,
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

            Table billingTableRooms = _Print.OutputTableRooms();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            //dataRowGroup.Rows.Add(SeparatorLineTableRow(true));
            dataRowGroup.Rows.Add(print.SeparatorTextTableRow("consumption"));
            dataRowGroup.Rows.Add(print.TableRowBillingHeaderConsumption(false));

            dataRowGroup.Rows.Add(ConsumptionaOutputTableRowRooms(
                _BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.HeatingUnitsTotalConsumptionShare,
                _BillingViewModel.Billing.ConsumptionItems[0].ConsumedUnits,
                _BillingViewModel.Billing.FixedAmountCosts.TransactionItem));

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