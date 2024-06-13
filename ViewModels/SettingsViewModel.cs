using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
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
        private SupportedLanguages _Language;
        public SupportedLanguages Language
        {
            get { return _Language; }
            set
            {
                _Language = value;

                Application.Current.Resources["Language"] = Language.ToString();
                new LanguageResources(Language.ToString());

                OnPropertyChanged(nameof(Language));
            }
        }


        private SupportedLanguages _SelectedItem;

        public SupportedLanguages SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                Language = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }



        private Color _backgroundColor;
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;

                Background = new SolidColorBrush(BackgroundColor);
                Application.Current.Resources["SCB_Background"] = Background;

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
                Application.Current.Resources["SCB_Text"] = Foreground;
                Application.Current.Resources["SCB_Text_Header"] = Foreground;

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

                Application.Current.Resources["FF"] = FontFamily;
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

                Application.Current.Resources["FS"] = FontSize;
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

            FontSize = (double)Application.Current.Resources["FS"];
            FontFamily = (FontFamily)Application.Current.Resources["FF"];

            BackgroundColor = ((SolidColorBrush)Application.Current.Resources["SCB_Background"]).Color;
            ForegroundColor = ((SolidColorBrush)Application.Current.Resources["SCB_Text"]).Color;

            SelectedItem = (SupportedLanguages)System.Enum.Parse(typeof(SupportedLanguages), Application.Current.Resources["Language"].ToString());
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
