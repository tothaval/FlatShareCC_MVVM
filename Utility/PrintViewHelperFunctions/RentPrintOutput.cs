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
using System.Windows.Media.Animation;

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

            _Print = new PrintOutputBase(_PrintViewModel, _FlatViewModel, _SelectedYear);
        }

        #endregion


        // Methods
        #region Methods

        private TableRowGroup AllCosts(TableRowGroup dataRowGroup, RentViewModel viewModel, int month)
        {
            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs"));

                dataRowGroup = ContractCostsDisplay(dataRowGroup, viewModel, month);

                dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.OtherFTISum, "other", month));
                dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, -1 * viewModel.CreditSum, "credit", month));

                dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, viewModel, month, viewModel.CostsAndCredits, false);


                if (_PrintViewModel.AnnualRentCostsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs until end of year"));

                    dataRowGroup = ContractCostsDisplayUntilEndOfYear(dataRowGroup, viewModel, month);

                    dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.FirstYearOtherFTISum, "other"));

                    dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, viewModel, -1, viewModel.FirstYearCompleteCosts, false);
                }

                dataRowGroup.Rows.Add(_Print.SeparatorLineTableRow(true));
            }
            else
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs summary"));

                dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, viewModel, month, viewModel.CostsAndCredits, true);
            }

            return dataRowGroup;
        }


        private TableRowGroup AllCostsRooms(TableRowGroup dataRowGroup, RentViewModel viewModel, RoomCostShareRent roomCostShareRent, int month)
        {

            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs", true));

                dataRowGroup = ContractCostsRoomsDisplay(dataRowGroup, viewModel, roomCostShareRent, month);

                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.OtherCostsShare, "other", month));

                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, -1* roomCostShareRent.CreditShare, "credit", month));

                dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, viewModel, month, roomCostShareRent.RoomName, roomCostShareRent.CostAndCreditShare, false);

                if (_PrintViewModel.AnnualRentCostsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs until end of year", true));

                    dataRowGroup = ContractCostsRoomsDisplayUntilEndOfYear(dataRowGroup, viewModel, roomCostShareRent, month);

                    dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.FirstYearOtherCostsShare, "other", month));

                    dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, viewModel, month, roomCostShareRent.RoomName, roomCostShareRent.FirstYearCompleteCostShare, false);
                }
            }
            else
            {
                dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, viewModel, month, roomCostShareRent.RoomName, roomCostShareRent.CostAndCreditShare, true);

            }

            return dataRowGroup;
        }


        /// <summary>
        /// prints output rows according to selected data output progression
        /// and adds them to the documentContext
        /// </summary>
        /// <param name="documentContext">the context within the flow document where this should be added</param>
        /// <param name="RentList"></param>
        /// <param name="SelectedDetailOption"></param>
        /// <param name="iterator"></param>
        /// <returns>documentContext</returns>
        private Section BuildDataOutputProgression(Section documentContext, ObservableCollection<RentViewModel> RentList, DataOutputProgressionTypes SelectedDetailOption, int iterator, bool isFlat)
        {
            if (SelectedDetailOption == DataOutputProgressionTypes.TimeChange)
            {
                for (int monthCounter = 1; monthCounter < 13; monthCounter++)
                {

                    if (iterator + 1 < RentList.Count)
                    {
                        // später im Ablauf, um Nachfolger zu berücksichtigen

                        if (RentList[iterator + 1].StartDate.Month - 1 < monthCounter)
                        {
                            break;
                        }

                        if (RentList[iterator].StartDate.Year < _SelectedYear)
                        {
                            if (isFlat)
                            {
                                documentContext.Blocks.Add(RentPlanTable(RentList[iterator], monthCounter));
                            }
                            else
                            {
                                documentContext.Blocks.Add(RentPlanTableRooms(RentList[iterator], monthCounter));
                            }

                        }
                        else if (RentList[iterator].StartDate.Year == _SelectedYear
                            && monthCounter >= RentList[iterator].StartDate.Month)
                        {
                            if (isFlat)
                            {
                                documentContext.Blocks.Add(RentPlanTable(RentList[iterator], monthCounter));
                            }
                            else
                            {
                                documentContext.Blocks.Add(RentPlanTableRooms(RentList[iterator], monthCounter));
                            }
                        }

                    }
                    else
                    {
                        if (RentList[iterator].StartDate.Year == _SelectedYear && monthCounter < RentList[iterator].StartDate.Month)
                        {
                            continue;
                        }
                        else
                        {
                            if (isFlat)
                            {
                                documentContext.Blocks.Add(RentPlanTable(RentList[iterator], monthCounter));
                            }
                            else
                            {
                                documentContext.Blocks.Add(RentPlanTableRooms(RentList[iterator], monthCounter));
                            }
                        }

                    }

                }
            }
            else
            {
                if (isFlat)
                {
                    documentContext.Blocks.Add(RentPlanTable(RentList[iterator]));
                }
                else
                {
                    documentContext.Blocks.Add(RentPlanTableRooms(RentList[iterator]));
                }

            }

            return documentContext;
        }


        public Section BuildRentDetails(DataOutputProgressionTypes SelectedDetailOption)
        {
            Section rentOutput = new Section();

            ObservableCollection<RentViewModel> RentList = new PrintOutputBase(_PrintViewModel ,_FlatViewModel, _SelectedYear).FindRelevantRentViewModels();

            for (int i = 0; i < RentList.Count; i++)
            {
                Section? valueChangeHeader = _Print.BuildValueChangeHeader(rentOutput, RentList, i);

                if (valueChangeHeader == null)
                {
                    continue;
                }

                rentOutput = valueChangeHeader;

                rentOutput = BuildDataOutputProgression(rentOutput, RentList, SelectedDetailOption, i, true);
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
                Section? valueChangeHeader = _Print.BuildValueChangeHeader(roomsOutput, RentList, i);

                if (valueChangeHeader == null)
                {
                    continue;
                }

                roomsOutput = valueChangeHeader;

                roomsOutput = BuildDataOutputProgression(roomsOutput, RentList, SelectedDetailOption, i, false);
            }

            return roomsOutput;
        }


        private TableRowGroup ContractCosts(TableRowGroup dataRowGroup, RentViewModel viewModel, int month)
        {
            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs"));

                dataRowGroup = ContractCostsDisplay(dataRowGroup, viewModel, month);

                dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, viewModel, month, viewModel.CostsTotal, false);

                if (_PrintViewModel.AnnualRentCostsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs until end of year"));

                    dataRowGroup = ContractCostsDisplayUntilEndOfYear(dataRowGroup, viewModel, month);

                    dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, viewModel, month, viewModel.FirstYearCostsTotal, false);
                }
            }
            else
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs summary"));

                dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, viewModel, month, viewModel.CostsTotal, true);
            }

            return dataRowGroup;
        }


        /// <summary>
        /// creates table header and cold rent and advance rows in print output
        /// for the entire flat.
        /// </summary>
        /// <param name="dataRowGroup">the surrounding table context</param>
        /// <param name="viewModel">the viewmodel that shall be displayed</param>
        /// <param name="month">-1 if ValueChange is selected as data output progression, else the month that shall be displayed</param>
        /// <returns>the modified surrounding table context</returns>
        private TableRowGroup ContractCostsDisplay(TableRowGroup dataRowGroup, RentViewModel viewModel, int month)
        {
            dataRowGroup.Rows.Add(_Print.TableRowRentHeader());

            dataRowGroup.Rows.Add(
                _Print.OutputTableRow(
                    viewModel,
                    viewModel.ColdRent,
                    viewModel.Rent.ColdRent.TransactionItem,
                    month));

            dataRowGroup.Rows.Add(
                _Print.OutputTableRow(
                    viewModel,
                    viewModel.Advance,
                    viewModel.Rent.Advance.TransactionItem,
                    month));

            return dataRowGroup;
        }


        /// <summary>
        /// creates table header, cold rent and advance rows in print output
        /// for the entire flat, displaying costs until the end of the selected year
        /// 
        /// on TimeChange data output progression selected, the displayed values
        /// will currently not shrink with higher month value, as one might expect.
        /// </summary>
        /// <param name="dataRowGroup">the surrounding table context</param>
        /// <param name="viewModel">the viewmodel that shall be displayed</param>
        /// <param name="month">-1 if ValueChange is selected as data output progression, else the month that shall be displayed</param>
        /// <returns>the modified surrounding table context</returns>
        private TableRowGroup ContractCostsDisplayUntilEndOfYear(TableRowGroup dataRowGroup, RentViewModel viewModel, int month)
        {
            dataRowGroup.Rows.Add(_Print.TableRowRentHeader());

            dataRowGroup.Rows.Add(
                _Print.OutputTableRow(
                    viewModel,
                    viewModel.FirstYearRent,
                    viewModel.Rent.ColdRent.TransactionItem,
                    month));

            dataRowGroup.Rows.Add(
                _Print.OutputTableRow(
                    viewModel,
                    viewModel.FirstYearAdvance,
                    viewModel.Rent.Advance.TransactionItem,
                    month));

            return dataRowGroup;
        }


        private TableRowGroup ContractCostsRooms(TableRowGroup dataRowGroup, RentViewModel viewModel, RoomCostShareRent roomCostShareRent, int month)
        {

            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs", true));

                dataRowGroup = ContractCostsRoomsDisplay(dataRowGroup, viewModel, roomCostShareRent, month);

                dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, viewModel, month, roomCostShareRent.RoomName, roomCostShareRent.PriceShare, false);

                if (_PrintViewModel.AnnualRentCostsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs until end of year", true));

                    dataRowGroup = ContractCostsRoomsDisplayUntilEndOfYear(dataRowGroup, viewModel, roomCostShareRent, month);

                    dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, viewModel, month, roomCostShareRent.RoomName, roomCostShareRent.FirstYearContractCostsShare, false);
                }
            }
            else
            {
                dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, viewModel, month, roomCostShareRent.RoomName, roomCostShareRent.PriceShare, true);
            }

            return dataRowGroup;
        }


        private TableRowGroup ContractCostsRoomsDisplay(TableRowGroup dataRowGroup, RentViewModel viewModel, RoomCostShareRent roomCostShareRent, int month)
        {
            dataRowGroup.Rows.Add(_Print.TableRowRoomHeader());

            dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.RentShare, viewModel.Rent.ColdRent.TransactionItem, month));

            dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.AdvanceShare, viewModel.Rent.Advance.TransactionItem, month));

            return dataRowGroup;
        }


        private TableRowGroup ContractCostsRoomsDisplayUntilEndOfYear(TableRowGroup dataRowGroup, RentViewModel viewModel, RoomCostShareRent roomCostShareRent, int month)
        {
            dataRowGroup.Rows.Add(_Print.TableRowRoomHeader());

            dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.FirstYearRentShare, viewModel.Rent.ColdRent.TransactionItem, month));

            dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.FirstYearAdvanceShare, viewModel.Rent.Advance.TransactionItem, month));

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

        #endregion


    }
}
// EOF