using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SharedLivingCostCalculator.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private Color _backgroundColor;

        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;

                Background = new SolidColorBrush(BackgroundColor);
                Application.Current.Resources["R_Background"] = Background;

                OnPropertyChanged(nameof(BackgroundColor));
            }
        }

        private Color _foregroundColor;

        public Color ForegroundColor
        {
            get { return _foregroundColor; }
            set
            {
                _foregroundColor = value;

                Foreground = new SolidColorBrush(ForegroundColor);
                Application.Current.Resources["R_Foreground"] = Foreground;
                Application.Current.Resources["R_Header"] = Foreground;

                OnPropertyChanged(nameof(ForegroundColor));
            }
        }

        private FontFamily _fontFamiliy;

        public FontFamily FontFamily
        {
            get { return _fontFamiliy; }
            set
            {
                _fontFamiliy = value;

                OnPropertyChanged(nameof(FontFamily));
            }
        }

        // once the main functionality is up and running
        // implement some options to change displayed currency
                
        //public ObservableCollection<string> Currency = new ObservableCollection<string>() 
        //{ "Dollar", "Euro", "Pound" };

        //private Dictionary<string, string> _countries = new Dictionary<string, string>()
        //{
        //    { "$", "en-EN" },
        //    { "Germany", "de-DE"  },
        //    { "United States", "en-US" }
        //};               


        //private string _selectedCountry;

        //public string SelectedCountry
        //{
        //    get { return _selectedCountry; }
        //    set
        //    {
        //        _selectedCountry = value;
        //        OnPropertyChanged(nameof(SelectedCountry));
        //    }
        //}



        private double _fontSize;

        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;

                Application.Current.Resources["R_FontSize"] = FontSize;
                OnPropertyChanged(nameof(FontSize));
            }
        }


        private Brush _background;

        public Brush Background
        {
            get { return _background; }
            set
            {
                _background = value;
                OnPropertyChanged(nameof(Background));
            }
        }



        private Brush _foreground;

        public Brush Foreground
        {
            get { return _foreground; }
            set
            {
                _foreground = value;
                OnPropertyChanged(nameof(Foreground));                
            }
        }

        public ICommand LeaveCommand { get; }

        private readonly SettingsView _settingsView;


        public SettingsViewModel()
        {
            LeaveCommand = new RelayCommand(CloseWindow, CanExecute);

            FontSize = (double)Application.Current.Resources["R_FontSize"];
            FontFamily = (FontFamily)Application.Current.Resources["R_FontFamiliy"];

            BackgroundColor = ((SolidColorBrush)Application.Current.Resources["R_Background"]).Color;
            ForegroundColor = ((SolidColorBrush)Application.Current.Resources["R_Foreground"]).Color;
        }
        
        private void CloseWindow(object obj)
        {
            SettingsView? settingsView = (SettingsView)obj;

            settingsView?.Close();
        }

        private bool CanExecute(object obj)
        {
            return true;
        }
    }
}
