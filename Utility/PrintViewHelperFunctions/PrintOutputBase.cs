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

namespace SharedLivingCostCalculator.Utility.PrintViewHelperFunctions
{
    public class PrintOutputBase
    {

        // Properties & Fields
        #region Properties & Fields

        private FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;


        public int SelectedYear { get; set; }

        #endregion


        // Constructors
        #region Constructors

        public PrintOutputBase(FlatViewModel flatViewModel, int selectedYear)
        {
            _FlatViewModel = flatViewModel;
            SelectedYear = selectedYear;
        }

        #endregion


        // Methods
        #region Methods

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

            return $"{FlatViewModel.Address}, {_FlatViewModel.Area}m², {_FlatViewModel.RoomCount} {rooms}";
        }


        public Section BuildRoomAreaData()
        {
            Section s = new Section();

            s.Blocks.Add(new Paragraph(new Run($"{BuildAddressDetails()}")) { FontWeight = FontWeights.Normal, FontSize = 14.0 });
            s.Blocks.Add(new Paragraph(new Run($"Room Area Data")) { FontWeight = FontWeights.Bold, FontSize = 14.0 });

            s.Blocks.Add(RoomAreaTable());

            return s ;
        }


        public ObservableCollection<RentViewModel> FindRelevantRentViewModels()
        {
            ObservableCollection<RentViewModel> preSortList = new ObservableCollection<RentViewModel>();
            ObservableCollection<RentViewModel> RentList = new ObservableCollection<RentViewModel>();

            DateTime startDate = new DateTime(SelectedYear, 1, 1);
            DateTime endDate = new DateTime(SelectedYear, 12, 31);

            int counter = 0;

            // rent begins before selected year ends
            if (_FlatViewModel.InitialRent.StartDate < endDate)
            {
                counter++;
            }

            // rent begins after Billing period start but before Billing period end
            if (_FlatViewModel.InitialRent.StartDate > startDate || _FlatViewModel.InitialRent.StartDate < endDate)
            {
                counter++;
            }

            if (counter > 0)
            {
                preSortList.Add(_FlatViewModel.InitialRent); 
            }

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
                        preSortList.Add(rent);
                        continue;
                    }

                    // rent begins before selected year ends
                    if (rent.StartDate < endDate)
                    {
                        preSortList.Add(rent);

                        continue;
                    }

                    // rent begins after Billing period start but before Billing period end
                    if (rent.StartDate > startDate || rent.StartDate < endDate)
                    {
                        preSortList.Add(rent);
                    }
                }
            }

            // sort List by StartDate, ascending
            RentList = new ObservableCollection<RentViewModel>(preSortList.OrderBy(i => i.StartDate));

            return RentList;
        }


        public Table OutputTableForFlat()
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


        public Table OutputTableForFlatBilling()
        {

            Table dataOutputTable = new Table();

            TableColumn StartDateColumn = new TableColumn() { Width = new GridLength(100) };
            TableColumn EndDateColumn = new TableColumn() { Width = new GridLength(100) };
            TableColumn ItemColumn = new TableColumn() { Width = new GridLength(250) };
            TableColumn CostColumn = new TableColumn() { Width = new GridLength(100) };

            dataOutputTable.Columns.Add(StartDateColumn);
            dataOutputTable.Columns.Add(EndDateColumn);
            dataOutputTable.Columns.Add(ItemColumn);
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


        public TableRow OutputTableRow(RentViewModel viewModel, double payment, string item, int month = -1, bool FontWeightBold = false)
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
            AreaPercentageCell.Blocks.Add(new Paragraph(new Run($"Area %")) { FontWeight = FontWeights.Bold, FontSize = 14.0, Margin=new Thickness(0,0,10,0) });

            dataRow.Cells.Add(RoomNameCell);
            dataRow.Cells.Add(RoomAreaCell);
            dataRow.Cells.Add(SharedAreaShareCell);
            dataRow.Cells.Add(TotalAreaShareCell);
            dataRow.Cells.Add(AreaPercentageCell);

            return dataRow;
        }



        public TableRow OutputTableRowRooms(RentViewModel viewModel, string roomname, double payment, string item, int month = -1, bool FontWeightBold = false)
        {
            TableRow dataRow = new TableRow();

            TableCell RoomName = new TableCell();

            TableCell DueTime = new TableCell();
            DueTime.TextAlignment = TextAlignment.Right;

            TableCell Item = new TableCell();
            TableCell Payment = new TableCell();
            Payment.TextAlignment = TextAlignment.Right;

            RoomName.Blocks.Add(new Paragraph(new Run()));

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


        public BillingViewModel? SearchForBillingViewModel()
        {
            foreach (BillingViewModel billingViewModel in FlatViewModel.AnnualBillings)
            {
                if (billingViewModel.Year == SelectedYear)
                {
                    return billingViewModel;
                }
            }

            return null;
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


        public TableRow SeparatorTextTableRow(string text, bool sub = false)
        {
            TableRow tableRow = new TableRow();

            TableCell cell = new TableCell();
            TableCell emptycell = new TableCell();

            cell.Blocks.Add(new Paragraph(new Run(text)) { Background = new SolidColorBrush(Colors.LightGray), FontSize = 14.0 });

            if (sub)
            {
                cell.ColumnSpan = 3;

                tableRow.Cells.Add(emptycell);
                tableRow.Cells.Add(cell);
            }
            else
            {
                cell.ColumnSpan = 4;

                tableRow.Cells.Add(cell);
            }


            return tableRow;
        }


        public TableRow RoomSeparatorTableRow(IRoomCostShare roomCostShare, bool showTenant )
        {
            if (showTenant)
            {
                return SeparatorTextTableRow($"{roomCostShare.RoomName} {roomCostShare.RoomArea}m² {roomCostShare.Tenant}");
            }
            
            return SeparatorTextTableRow($"{roomCostShare.RoomName} {roomCostShare.RoomArea}m²");            
        }


        public TableRow TableRowBillingHeader()
        {
            TableRow headerRow = new TableRow();

            TableCell headerCell = new TableCell();
            TableCell headerCell_DueTime = new TableCell();
            TableCell headerCell_Item = new TableCell();
            TableCell headerCell_Costs = new TableCell();

            headerCell.Blocks.Add(new Paragraph(new Run()));
            headerCell_DueTime.Blocks.Add(new Paragraph(new Run("Time")) { FontWeight = FontWeights.Bold, FontSize = 12 });
            headerCell_Item.Blocks.Add(new Paragraph(new Run("Item")) { FontWeight = FontWeights.Bold, FontSize = 12 });
            headerCell_Costs.Blocks.Add(new Paragraph(new Run("Costs")) { FontWeight = FontWeights.Bold, FontSize = 12 });

            headerRow.Cells.Add(headerCell);
            headerRow.Cells.Add(headerCell_DueTime);
            headerRow.Cells.Add(headerCell_Item);
            headerRow.Cells.Add(headerCell_Costs);

            return headerRow;
        }


        public TableRow TableRowRentHeader()
        {
            TableRow headerRow = new TableRow();

            TableCell headerCell_DueTime = new TableCell();
            TableCell headerCell_Item = new TableCell();
            TableCell headerCell_Costs = new TableCell();

            headerCell_DueTime.Blocks.Add(new Paragraph(new Run("Time")) { FontWeight = FontWeights.Bold, FontSize = 12 });
            headerCell_Item.Blocks.Add(new Paragraph(new Run("Item")) { FontWeight = FontWeights.Bold, FontSize = 12 });
            headerCell_Costs.Blocks.Add(new Paragraph(new Run("Costs")) { FontWeight = FontWeights.Bold, FontSize = 12 });

            headerRow.Cells.Add(headerCell_DueTime);
            headerRow.Cells.Add(headerCell_Item);
            headerRow.Cells.Add(headerCell_Costs);

            return headerRow;
        }


        public TableRow TableRowRoomHeader()
        {
            TableRow headerRow = new TableRow();

            TableCell RoomName = new TableCell();
            TableCell DueTime = new TableCell();
            TableCell Item = new TableCell();
            TableCell Costs = new TableCell();

            RoomName.Blocks.Add(new Paragraph(new Run()));
            DueTime.Blocks.Add(new Paragraph(new Run("Time")) { FontSize = 12, FontWeight = FontWeights.Bold });
            Item.Blocks.Add(new Paragraph(new Run("Item")) { FontSize = 12, FontWeight = FontWeights.Bold });
            Costs.Blocks.Add(new Paragraph(new Run("Costs")) { FontSize = 12, FontWeight = FontWeights.Bold });

            headerRow.Cells.Add(RoomName);
            headerRow.Cells.Add(DueTime);
            headerRow.Cells.Add(Item);
            headerRow.Cells.Add(Costs);

            return headerRow;
        }

        #endregion

    }
}
// EOF