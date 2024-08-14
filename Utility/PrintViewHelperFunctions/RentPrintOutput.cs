﻿/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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

namespace SharedLivingCostCalculator.Utility.PrintViewHelperFunctions
{
    public class RentPrintOutput
    {

        // Properties & Fields
        #region Properties & Fields

        private int _SelectedYear { get; }


        private FlatViewModel _FlatViewModel { get; }


        private PrintViewModel _PrintViewModel { get; }

        #endregion


        // Constructors
        #region Constructors
        
        public RentPrintOutput(PrintViewModel printViewModel, FlatViewModel flatViewModel, int selectedYear)
        {
            _PrintViewModel = printViewModel;
            _FlatViewModel = flatViewModel;
            _SelectedYear = selectedYear;
        } 

        #endregion


        // Methods
        #region Methods

        public Section BuildRentDetails(string SelectedDetailOption)
        {
            Section rentOutput = new Section();

            ObservableCollection<RentViewModel> RentList = new PrintOutputBase(_FlatViewModel, _SelectedYear).FindRelevantRentViewModels();

            for (int i = 0; i < RentList.Count; i++)
            {



                if (RentList[i].StartDate.Year < _SelectedYear)
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


                PrintOutputBase print = new PrintOutputBase(_FlatViewModel, _SelectedYear);

                Table headerTable = print.OutputTableForFlat();


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


        private Section BuildRoomDetails(string SelectedDetailOption)
        {
            PrintOutputBase print = new PrintOutputBase(_FlatViewModel, _SelectedYear);

            Section roomsOutput = new Section();

            Paragraph p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };

            roomsOutput.Blocks.Add(p);

            p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };
            p.Inlines.Add(new Run($"Rent Plan Rooms {_SelectedYear}: ")
            { FontWeight = FontWeights.Bold, FontSize = 16.0 });
            p.Inlines.Add(new Run($"{print.BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
            //p.Style = headerParagraph;
            roomsOutput.Blocks.Add(p);

            ObservableCollection<RentViewModel> RentList = print.FindRelevantRentViewModels();

            for (int i = 0; i < RentList.Count; i++)
            {
                if (RentList[i].StartDate.Year < _SelectedYear)
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


        private Block RentPlanTable(RentViewModel viewModel, int month = -1)
        {
            PrintOutputBase print = new PrintOutputBase(_FlatViewModel, _SelectedYear);

            Table rentPlanFlatTable = print.OutputTableForFlat();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            dataRowGroup.Rows.Add(print.SeparatorTextTableRow("contract costs"));
            dataRowGroup.Rows.Add(print.TableRowRentHeader());

            dataRowGroup.Rows.Add(print.OutputTableRow(viewModel, -1 * viewModel.ColdRent, viewModel.Rent.ColdRent.TransactionItem, month));
            dataRowGroup.Rows.Add(print.OutputTableRow(viewModel, -1 * viewModel.FixedCostsAdvance, viewModel.Rent.FixedCostsAdvance.TransactionItem, month));
            dataRowGroup.Rows.Add(print.OutputTableRow(viewModel, -1 * viewModel.HeatingCostsAdvance, viewModel.Rent.HeatingCostsAdvance.TransactionItem, month));

            dataRowGroup.Rows.Add(print.OutputTableRow(viewModel, -1 * viewModel.CostsTotal, "sum", month, true));

            dataRowGroup.Rows.Add(print.SeparatorLineTableRow(true));
            dataRowGroup.Rows.Add(print.SeparatorTextTableRow("all costs"));
            dataRowGroup.Rows.Add(print.TableRowRentHeader());

            dataRowGroup.Rows.Add(print.OutputTableRow(viewModel, -1 * viewModel.ColdRent, viewModel.Rent.ColdRent.TransactionItem, month));
            dataRowGroup.Rows.Add(print.OutputTableRow(viewModel, -1 * viewModel.FixedCostsAdvance, viewModel.Rent.FixedCostsAdvance.TransactionItem, month));
            dataRowGroup.Rows.Add(print.OutputTableRow(viewModel, -1 * viewModel.HeatingCostsAdvance, viewModel.Rent.HeatingCostsAdvance.TransactionItem, month));

            dataRowGroup.Rows.Add(print.OutputTableRow(viewModel, -1 * viewModel.OtherFTISum, "other", month));
            dataRowGroup.Rows.Add(print.OutputTableRow(viewModel, viewModel.CreditSum, "credit", month));

            dataRowGroup.Rows.Add(print.OutputTableRow(viewModel, -1 * viewModel.CostsAndCredits, "sum", month, true));

            rentPlanFlatTable.RowGroups.Add(dataRowGroup);

            return rentPlanFlatTable;
        }


        private Block RentPlanTableRooms(RentViewModel viewModel, int month = -1)
        {
            PrintOutputBase print = new PrintOutputBase(_FlatViewModel, _SelectedYear);

            Table rentPlanRoomsTable = print.OutputTableRooms();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            foreach (RoomCostShareRent item in viewModel.RoomCostShares)
            {
                dataRowGroup.Rows.Add(print.SeparatorTextTableRow($"{item.RoomName} {item.RoomArea}m²"));

                dataRowGroup.Rows.Add(print.SeparatorLineTableRow(true));
                dataRowGroup.Rows.Add(print.SeparatorTextTableRow("contract costs", true));

                dataRowGroup.Rows.Add(print.TableRowRoomHeader());

                dataRowGroup.Rows.Add(print.OutputTableRowRooms(viewModel, item.RoomName, item.RentShare, viewModel.Rent.ColdRent.TransactionItem, month));

                dataRowGroup.Rows.Add(print.OutputTableRowRooms(viewModel, item.RoomName, item.FixedCostsAdvanceShare, viewModel.Rent.FixedCostsAdvance.TransactionItem, month));

                dataRowGroup.Rows.Add(print.OutputTableRowRooms(viewModel, item.RoomName, item.HeatingCostsAdvanceShare, viewModel.Rent.HeatingCostsAdvance.TransactionItem, month));


                dataRowGroup.Rows.Add(print.OutputTableRowRooms(viewModel, item.RoomName, item.PriceShare, "sum", month, true));


                dataRowGroup.Rows.Add(print.SeparatorTextTableRow("all costs", true));

                dataRowGroup.Rows.Add(print.TableRowRoomHeader());

                dataRowGroup.Rows.Add(print.OutputTableRowRooms(viewModel, item.RoomName, item.RentShare, viewModel.Rent.ColdRent.TransactionItem, month));

                dataRowGroup.Rows.Add(print.OutputTableRowRooms(viewModel, item.RoomName, item.FixedCostsAdvanceShare, viewModel.Rent.FixedCostsAdvance.TransactionItem, month));

                dataRowGroup.Rows.Add(print.OutputTableRowRooms(viewModel, item.RoomName, item.HeatingCostsAdvanceShare, viewModel.Rent.HeatingCostsAdvance.TransactionItem, month));


                dataRowGroup.Rows.Add(print.OutputTableRowRooms(viewModel, item.RoomName, item.OtherCostsShare, "other", month));

                dataRowGroup.Rows.Add(print.OutputTableRowRooms(viewModel, item.RoomName, item.CreditShare, "credit", month));
                dataRowGroup.Rows.Add(print.OutputTableRowRooms(viewModel, item.RoomName, item.CostAndCreditShare, "sum", month, true));
            }

            rentPlanRoomsTable.RowGroups.Add(dataRowGroup);

            return rentPlanRoomsTable;
        } 

        #endregion


    }
}
// EOF