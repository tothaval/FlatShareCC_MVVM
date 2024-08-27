using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace SharedLivingCostCalculator.Views
{
    /// <summary>
    /// Interaktionslogik für RentManagementView.xaml
    /// </summary>
    public partial class RentManagementView : UserControl
    {

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        
        
        public RentManagementView()
        {
            InitializeComponent();
        }


        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(rentupdates.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }


        private void rentupdates_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;

            int index = 0;

            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    if (headerClicked != null)
                    {
                        GridViewHeaderRowPresenter presenter = headerClicked.Parent as GridViewHeaderRowPresenter;
                        if (presenter != null)
                        {
                            index = presenter.Columns.IndexOf(headerClicked.Column);
                        }
                    }


                    switch (index)
                    {

                        case 0:
                            sortBy = "StartDate";
                            break;

                        case 1:
                            sortBy = "ColdRent";
                            break;

                        case 2:
                            sortBy = "Advance";
                            break;

                        case 3:
                            sortBy = "OtherFTISum";
                            break;

                        case 4:
                            sortBy = "CreditSum";
                            break;


                        default:
                            break;
                    }

                    Sort(sortBy, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }


    }
}
// EOF