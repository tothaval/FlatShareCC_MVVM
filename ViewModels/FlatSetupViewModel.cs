/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *   
 *  FlatSetupViewModel  : BaseViewModel
 * 
 *  viewmodel for FlatSetupView
 *  
 *  displays a seperate window for creating
 *  or editing of FlatViewModel instances
 */
using System.Collections.ObjectModel;
using System.Windows.Input;
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Views;


namespace SharedLivingCostCalculator.ViewModels
{
    internal class FlatSetupViewModel : BaseViewModel
    {

        private ObservableCollection<FlatViewModel> _flatCollection;


        private FlatViewModel _flatsetup;
        public FlatViewModel FlatSetup => _flatsetup;


        private FlatSetupView _flatSetupView;


        public bool IsNewFlatWindow { get; set; } = false;


        private string _flatSetupTitleText;


        public string FlatSetupTitleText
        {
            get { return _flatSetupTitleText; }
            set
            {
                _flatSetupTitleText = value;
                OnPropertyChanged(nameof(FlatSetupTitleText));
            }
        }


        public ICommand FlatSetupCommand { get; }


        public ICommand LeaveViewCommand { get; }


        /// <summary>
        /// Called on New Flat Button clicked
        /// </summary>
        /// <param name="flatCollection"></param>
        /// <param name="flatSetupView"></param>
        public FlatSetupViewModel(ObservableCollection<FlatViewModel> flatCollection, FlatSetupView flatSetupView)
        {
            IsNewFlatWindow = true;

            _flatCollection = flatCollection;
            _flatSetupView = flatSetupView;

            _flatsetup = new FlatViewModel(new Flat());

            FlatSetupTitleText = "Shared Living Cost Calculator - Flat Setup";

            FlatSetupCommand = new FlatSetupCommand(_flatCollection, this, flatSetupView);
            LeaveViewCommand = new RelayCommand(Close, (s) => true);

            if (_flatsetup != null)
            {
                _flatsetup.RoomCreation += _flatsetup_RoomCreation;
            }
        }


        /// <summary>
        /// Called on Edit Button is clicked, Proceed Button is hidden,
        /// by default, all input fields except Details and RoomNames are readonly
        /// an option in Settings may be used to allow for all fields to be edited.
        /// </summary>
        /// <param name="flatCollection"></param>
        /// <param name="flatSetupView"></param>
        /// <param name="flatViewModel"></param>
        public FlatSetupViewModel(ObservableCollection<FlatViewModel> flatCollection, FlatSetupView flatSetupView,
                FlatViewModel flatViewModel)
        {
            IsNewFlatWindow = false;

            _flatCollection = flatCollection;
            _flatSetupView = flatSetupView;

            _flatsetup = flatViewModel;

            FlatSetupTitleText = "Shared Living Cost Calculator - Flat Setup";

            FlatSetupCommand = new FlatSetupCommand(_flatCollection, this, flatSetupView);
            LeaveViewCommand = new RelayCommand(Close, (s) => true);

            if (_flatsetup != null)
            {
                _flatsetup.RoomCreation += _flatsetup_RoomCreation;
            }
        }


        private void Close(object obj)
        {
            _flatSetupView.Close();
        }


        private void _flatsetup_RoomCreation()
        {
            OnPropertyChanged(nameof(IsNewFlatWindow));
            OnPropertyChanged(nameof(FlatSetup));
        }


    }
}
// EOF