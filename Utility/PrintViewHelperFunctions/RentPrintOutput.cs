/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RentPrintOutput
 * 
 *  helper class for creating rent related output in PrintViewModel
 */
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels;
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Models.Contract;

namespace SharedLivingCostCalculator.Utility.PrintViewHelperFunctions
{
    public class RentPrintOutput
    {

        // Properties & Fields
        #region Properties & Fields

        private FlatViewModel _FlatViewModel { get; }


        private PrintOutputBase _Print { get; }


        private PrintViewModel _PrintViewModel { get; }


        private int _SelectedYear { get; }


        private bool _ShowTenant { get; set; }

        #endregion


        // Constructors
        #region Constructors

        public RentPrintOutput(PrintViewModel printViewModel, FlatViewModel flatViewModel, int selectedYear, bool showTenant)
        {
            _PrintViewModel = printViewModel;
            _FlatViewModel = flatViewModel;
            _SelectedYear = selectedYear;
            _ShowTenant = showTenant;

            _Print = new PrintOutputBase(_FlatViewModel, _SelectedYear);
        }

        #endregion


        // Methods
        #region Methods

        private TableRowGroup AllCosts(TableRowGroup dataRowGroup, RentViewModel viewModel, int month)
        {
            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs"));
                dataRowGroup.Rows.Add(_Print.TableRowRentHeader());

                dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.ColdRent, viewModel.Rent.ColdRent.TransactionItem, month));
                dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.Advance, viewModel.Rent.Advance.TransactionItem, month));

                dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.OtherFTISum, "other", month));
                dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, -1 * viewModel.CreditSum, "credit", month));

                if (_PrintViewModel.IncludeTaxesSelected)
                {
                    double tax = _PrintViewModel.TaxValue / 100;
                    double taxedSum = 0.0;

                    if (_PrintViewModel.SelectedTaxOption == TaxOptionTypes.Taxed)
                    {
                        taxedSum = tax * viewModel.CostsAndCredits;

                        dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.CostsAndCredits, "sum", month, true));

                        dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, taxedSum, $"including {_PrintViewModel.TaxValue}% taxes", month));
                    }
                    else
                    {
                        taxedSum = (tax + 1) * viewModel.CostsAndCredits - viewModel.CostsAndCredits;

                        double withTaxes = taxedSum + viewModel.CostsAndCredits;

                        dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.CostsAndCredits, "sum", month, true));

                        dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, taxedSum, $"tax {_PrintViewModel.TaxValue}%", month));

                        dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, withTaxes, "taxed sum", month, true));
                    }
                }
                else
                {
                    dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.CostsAndCredits, "sum", month, true));
                }


                if (_PrintViewModel.AnnualRentCostsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs until end of year"));
                    dataRowGroup.Rows.Add(_Print.TableRowRentHeader());

                    dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.FirstYearRent, viewModel.Rent.ColdRent.TransactionItem, month));
                    dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.FirstYearAdvance, viewModel.Rent.Advance.TransactionItem, month));

                    dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.FirstYearOtherFTISum, "other"));

                    dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.FirstYearCompleteCosts, "sum", -1, true));
                }

                dataRowGroup.Rows.Add(_Print.SeparatorLineTableRow(true));
            }
            else
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs summary"));

                dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.CostsAndCredits, "sum", month, true, false));
            }

            return dataRowGroup;
        }


        public Section BuildRentDetails(DataOutputProgressionTypes SelectedDetailOption)
        {
            Section rentOutput = new Section();

            ObservableCollection<RentViewModel> RentList = new PrintOutputBase(_FlatViewModel, _SelectedYear).FindRelevantRentViewModels();

            for (int i = 0; i < RentList.Count; i++)
            {

                if (RentList[i].StartDate.Year < _SelectedYear)
                {
                    if (i + 1 < RentList.Count)
                    {
                        if (RentList[i + 1].StartDate > RentList[i].StartDate && RentList[i + 1].StartDate.Year < _SelectedYear)
                        {
                            continue;
                        }
                        else
                        {
                            rentOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 0, 0, 0), FontWeight = FontWeights.Bold });
                        }
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

                Table headerTable = _Print.OutputTableForFlat();

                if (SelectedDetailOption == DataOutputProgressionTypes.TimeChange)
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

                            if (RentList[i].StartDate.Year < _SelectedYear)
                            {
                                rentOutput.Blocks.Add(RentPlanTable(RentList[i], monthCounter));
                            }
                            else if (RentList[i].StartDate.Year == _SelectedYear
                                && monthCounter >= RentList[i].StartDate.Month)
                            {
                                rentOutput.Blocks.Add(RentPlanTable(RentList[i], monthCounter));
                            }

                        }
                        else
                        {
                            if (RentList[i].StartDate.Year == _SelectedYear && monthCounter < RentList[i].StartDate.Month)
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

            rentOutput.Blocks.Add(BuildRoomDetails(SelectedDetailOption));


            return rentOutput;
        }


        private Section BuildRoomDetails(DataOutputProgressionTypes SelectedDetailOption)
        {
            Section roomsOutput = new Section();

            Paragraph p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };

            roomsOutput.Blocks.Add(p);

            p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };
            p.Inlines.Add(new Run($"Rent Plan Rooms {_SelectedYear}: ")
            { FontWeight = FontWeights.Bold, FontSize = 16.0 });
            p.Inlines.Add(new Run($"{_Print.BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
            //p.Style = headerParagraph;
            roomsOutput.Blocks.Add(p);

            ObservableCollection<RentViewModel> RentList = _Print.FindRelevantRentViewModels();

            for (int i = 0; i < RentList.Count; i++)
            {
                if (RentList[i].StartDate.Year < _SelectedYear)
                {
                    if (i + 1 < RentList.Count)
                    {
                        if (RentList[i + 1].StartDate > RentList[i].StartDate && RentList[i + 1].StartDate.Year < _SelectedYear)
                        {
                            continue;
                        }
                        else
                        {
                            roomsOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 0, 0, 0), FontWeight = FontWeights.Bold });
                        }
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

                if (SelectedDetailOption == DataOutputProgressionTypes.TimeChange)
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

                            if (RentList[i].StartDate.Year < _SelectedYear)
                            {
                                roomsOutput.Blocks.Add(RentPlanTableRooms(RentList[i], monthCounter));
                            }
                            else if (RentList[i].StartDate.Year == _SelectedYear
                                && monthCounter >= RentList[i].StartDate.Month)
                            {
                                roomsOutput.Blocks.Add(RentPlanTableRooms(RentList[i], monthCounter));
                            }


                        }
                        else
                        {
                            if (RentList[i].StartDate.Year == _SelectedYear && monthCounter < RentList[i].StartDate.Month)
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


        private TableRowGroup ContractCosts(TableRowGroup dataRowGroup, RentViewModel viewModel, int month)
        {
            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs"));
                dataRowGroup.Rows.Add(_Print.TableRowRentHeader());

                dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.ColdRent, viewModel.Rent.ColdRent.TransactionItem, month));
                dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.Advance, viewModel.Rent.Advance.TransactionItem, month));

                if (_PrintViewModel.IncludeTaxesSelected)
                {
                    double tax = _PrintViewModel.TaxValue / 100;
                    double taxedSum = 0.0;

                    if (_PrintViewModel.SelectedTaxOption == TaxOptionTypes.Taxed)
                    {
                        taxedSum = tax * viewModel.CostsTotal;

                        dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.CostsTotal, "sum", month, true));

                        dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, taxedSum, $"including {_PrintViewModel.TaxValue}% taxes", month));
                    }
                    else
                    {
                        taxedSum = (tax + 1) * viewModel.CostsTotal - viewModel.CostsTotal;

                        double withTaxes = taxedSum + viewModel.CostsTotal;

                        dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.CostsTotal, "sum", month, true));

                        dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, taxedSum, $"tax {_PrintViewModel.TaxValue}%", month));

                        dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, withTaxes, "taxed sum", month, true));
                    }
                }
                else
                {
                    dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.CostsTotal, "sum", month, true));
                }

                if (_PrintViewModel.AnnualRentCostsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs until end of year"));
                    dataRowGroup.Rows.Add(_Print.TableRowRentHeader());

                    dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.FirstYearRent, viewModel.Rent.ColdRent.TransactionItem));
                    dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.FirstYearAdvance, viewModel.Rent.Advance.TransactionItem));

                    dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.FirstYearCostsTotal, "sum", -1, true));
                }

                dataRowGroup.Rows.Add(_Print.SeparatorLineTableRow(true));
            }
            else
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs summary"));

                dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.CostsTotal, "sum", month, true, false));
            }

            return dataRowGroup;
        }


        private Block RentPlanTable(RentViewModel viewModel, int month = -1)
        {
            Table rentPlanFlatTable = _Print.OutputTableForFlat();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            if (_PrintViewModel.ContractCostsSelected)
            {
                dataRowGroup = ContractCosts(dataRowGroup, viewModel, month);
            }

            if (_PrintViewModel.AllCostsSelected)
            {
                dataRowGroup = AllCosts(dataRowGroup, viewModel, month);
            }

            rentPlanFlatTable.RowGroups.Add(dataRowGroup);

            return rentPlanFlatTable;
        }


        private Block RentPlanTableRooms(RentViewModel viewModel, int month = -1)
        {
            Table rentPlanRoomsTable = _Print.OutputTableRooms();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            if (viewModel.RoomCostShares != null)
            {

                if (_PrintViewModel.DisplaySummarySelected)
                {
                    if (_PrintViewModel.ContractCostsSelected)
                    {
                        dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs summary", true));

                        foreach (RoomCostShareRent item in viewModel.RoomCostShares)
                        {
                            dataRowGroup = ContractCostsRooms(dataRowGroup, viewModel, item, month);
                        }
                    }

                    if (_PrintViewModel.AllCostsSelected)
                    {
                        dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs summary", true));

                        foreach (RoomCostShareRent item in viewModel.RoomCostShares)
                        {
                            dataRowGroup = AllCostsRooms(dataRowGroup, viewModel, item, month);
                        }
                    }
                }
                else
                {
                    foreach (RoomCostShareRent item in viewModel.RoomCostShares)
                    {
                        dataRowGroup.Rows.Add(_Print.RoomSeparatorTableRow(item, _ShowTenant));


                        if (_PrintViewModel.ContractCostsSelected)
                        {
                            dataRowGroup = ContractCostsRooms(dataRowGroup, viewModel, item, month);
                        }

                        if (_PrintViewModel.AllCostsSelected)
                        {
                            dataRowGroup = AllCostsRooms(dataRowGroup, viewModel, item, month);
                        }
                    }
                }

            }

            rentPlanRoomsTable.RowGroups.Add(dataRowGroup);

            return rentPlanRoomsTable;
        }


        private TableRowGroup AllCostsRooms(TableRowGroup dataRowGroup, RentViewModel viewModel, RoomCostShareRent roomCostShareRent, int month)
        {

            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorLineTableRow(true));
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs", true));

                dataRowGroup.Rows.Add(_Print.TableRowRoomHeader());

                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.RentShare, viewModel.Rent.ColdRent.TransactionItem, month));

                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.AdvanceShare, viewModel.Rent.Advance.TransactionItem, month));

                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.OtherCostsShare, "other", month));

                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.CreditShare, "credit", month));


                if (_PrintViewModel.IncludeTaxesSelected)
                {
                    double tax = _PrintViewModel.TaxValue / 100;
                    double taxedSum = 0.0;

                    if (_PrintViewModel.SelectedTaxOption == TaxOptionTypes.Taxed)
                    {
                        taxedSum = tax * roomCostShareRent.CostAndCreditShare;

                        dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.CostAndCreditShare, "sum", month, true));

                        dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, taxedSum, $"including {_PrintViewModel.TaxValue}% taxes", month));
                    }
                    else
                    {
                        taxedSum = (tax + 1) * roomCostShareRent.CostAndCreditShare - roomCostShareRent.CostAndCreditShare;

                        double withTaxes = taxedSum + roomCostShareRent.CostAndCreditShare;

                        dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.CostAndCreditShare, "sum", month, true));

                        dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, taxedSum, $"including {_PrintViewModel.TaxValue}% taxes", month));

                        dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, withTaxes, "taxed sum", month, true));
                    }
                }
                else
                {
                    dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.CostAndCreditShare, "sum", month, true));
                }

                if (_PrintViewModel.AnnualRentCostsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs until end of year", true));

                    dataRowGroup.Rows.Add(_Print.TableRowRoomHeader());

                    dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.FirstYearRentShare, viewModel.Rent.ColdRent.TransactionItem, month));

                    dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.FirstYearAdvanceShare, viewModel.Rent.Advance.TransactionItem, month));

                    dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.FirstYearOtherCostsShare, "other", month));

                    dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.FirstYearCompleteCostShare, "sum", month, true));

                }

            }
            else
            {
                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.CostAndCreditShare, roomCostShareRent.RoomName, month, true, false));
            }

            return dataRowGroup;
        }


        private TableRowGroup ContractCostsRooms(TableRowGroup dataRowGroup, RentViewModel viewModel, RoomCostShareRent roomCostShareRent, int month)
        {

            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorLineTableRow(true));

                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs", true));

                dataRowGroup.Rows.Add(_Print.TableRowRoomHeader());

                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.RentShare, viewModel.Rent.ColdRent.TransactionItem, month));

                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.AdvanceShare, viewModel.Rent.Advance.TransactionItem, month));

                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.PriceShare, "sum", month, true));

                if (_PrintViewModel.AnnualRentCostsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs until end of year", true));

                    dataRowGroup.Rows.Add(_Print.TableRowRoomHeader());

                    dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.FirstYearRentShare, viewModel.Rent.ColdRent.TransactionItem, month));

                    dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.FirstYearAdvanceShare, viewModel.Rent.Advance.TransactionItem, month));

                    dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.FirstYearContractCostsShare, "sum", month, true));
                }
            }
            else
            {
                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.PriceShare, roomCostShareRent.RoomName, month, true, false));
            }


            return dataRowGroup;
        }

        #endregion


    }
}
// EOF