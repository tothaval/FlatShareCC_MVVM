/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  SettingsViewModel  : BaseViewModel
 * 
 *  viewmodel for SettingsView
 *  
 *  shows a separate window which enables the
 *  user to edit some application properties.
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Windows;
using System.Windows.Media;

namespace SharedLivingCostCalculator.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {

        // properties & fields
        #region properties

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

        #endregion properties


        // constructors
        #region constructors

        public SettingsViewModel()
        {
            FontSize = (double)Application.Current.Resources["FS"];
            FontFamily = (FontFamily)Application.Current.Resources["FF"];

            BackgroundColor = ((SolidColorBrush)Application.Current.Resources["SCB_Background"]).Color;
            ForegroundColor = ((SolidColorBrush)Application.Current.Resources["SCB_Text"]).Color;

            SelectedItem = (SupportedLanguages)System.Enum.Parse(typeof(SupportedLanguages), Application.Current.Resources["Language"].ToString());
        }

        #endregion constructors


    }
}
// EOF