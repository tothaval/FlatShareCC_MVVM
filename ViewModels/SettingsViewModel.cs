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
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
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


        private double _ButtonCornerRadius;
        public double ButtonCornerRadius
        {
            get { return _ButtonCornerRadius; }
            set
            {
                _ButtonCornerRadius = value;
                Application.Current.Resources["Button_CornerRadius"] = new CornerRadius(_ButtonCornerRadius);
                OnPropertyChanged(nameof(ButtonCornerRadius));
            }
        }


        private double _VisibilityFieldCornerRadius;
        public double VisibilityFieldCornerRadius
        {
            get { return _VisibilityFieldCornerRadius; }
            set
            {
                _VisibilityFieldCornerRadius = value;
                Application.Current.Resources["VisibilityField_CornerRadius"] = new CornerRadius(_VisibilityFieldCornerRadius);
                OnPropertyChanged(nameof(VisibilityFieldCornerRadius));
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
                Application.Current.Resources["HFS"] = FontSize * 1.25;
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

                OnPropertyChanged(nameof(ForegroundColor));
            }
        }


        private Brush _headerText;
        public Brush HeaderText
        {
            get { return _headerText; }
            set
            {
                _headerText = value;
                OnPropertyChanged(nameof(Foreground));
            }
        }


        private Color _headerTextColor;
        public Color HeaderTextColor
        {
            get { return _headerTextColor; }
            set
            {
                _headerTextColor = value;

                HeaderText = new SolidColorBrush(HeaderTextColor);
                Application.Current.Resources["SCB_Text_Header"] = HeaderText;

                OnPropertyChanged(nameof(HeaderTextColor));
            }
        }


        private Brush _selection;
        public Brush Selection
        {
            get { return _selection; }
            set
            {
                _selection = value;
                OnPropertyChanged(nameof(Selection));
            }
        }


        private Color _selectionColor;
        public Color SelectionColor
        {
            get { return _selectionColor; }
            set
            {
                _selectionColor = value;

                Selection = new SolidColorBrush(SelectionColor);
                Application.Current.Resources["SCB_Selection"] = Selection;

                OnPropertyChanged(nameof(SelectionColor));
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





        private CultureInfo _SelectedCulture;
        public CultureInfo SelectedCulture
        {
            get { return _SelectedCulture; }
            set
            {
                _SelectedCulture = value;

                Application.Current.Resources["Culture"] = XmlLanguage.GetLanguage(_SelectedCulture.IetfLanguageTag);

                OnPropertyChanged(nameof(SelectedCulture));
            }
        }




        private SupportedLanguages _SelectedLanguageItem;
        public SupportedLanguages SelectedLanguageItem
        {
            get { return _SelectedLanguageItem; }
            set
            {
                _SelectedLanguageItem = value;
                Language = value;
                OnPropertyChanged(nameof(SelectedLanguageItem));
            }
        }

        #endregion properties



        // collections
        #region collections

        private ObservableCollection<CultureInfo> _Currency;
        public ObservableCollection<CultureInfo> Currency
        {
            get { return _Currency; }
            set
            {
                _Currency = value;

                OnPropertyChanged(nameof(Currency));
            }
        }


        //private ObservableCollection<FontFamily> _FontFamilies;
        //public ObservableCollection<FontFamily> FontFamilies
        //{
        //    get { return _FontFamilies; }
        //    set
        //    {
        //        _FontFamilies = value;

        //        OnPropertyChanged(nameof(FontFamilies));
        //    }
        //}

        #endregion collections




        // constructors
        #region constructors

        public SettingsViewModel()
        {
            FontSize = (double)Application.Current.Resources["FS"];
            FontFamily = (FontFamily)Application.Current.Resources["FF"];

            BackgroundColor = ((SolidColorBrush)Application.Current.Resources["SCB_Background"]).Color;
            ForegroundColor = ((SolidColorBrush)Application.Current.Resources["SCB_Text"]).Color;
            HeaderTextColor = ((SolidColorBrush)Application.Current.Resources["SCB_Text_Header"]).Color;
            SelectionColor = ((SolidColorBrush)Application.Current.Resources["SCB_Selection"]).Color;

            ButtonCornerRadius = ((CornerRadius)Application.Current.Resources["Button_CornerRadius"]).TopLeft;

            VisibilityFieldCornerRadius = ((CornerRadius)Application.Current.Resources["VisibilityField_CornerRadius"]).TopLeft;

            if (Application.Current.Resources["Language"] != null)
            {
                SelectedLanguageItem = (SupportedLanguages)System.Enum.Parse(typeof(SupportedLanguages), Application.Current.Resources["Language"].ToString());
            }
            else
            {
                SelectedLanguageItem = (SupportedLanguages)System.Enum.Parse(typeof(SupportedLanguages), SupportedLanguages.English.ToString());
            }
                        
            
            Currency = new ObservableCollection<CultureInfo>(CultureInfo.GetCultures(CultureTypes.SpecificCultures).ToList());

        }

        #endregion constructors


    }
}
// EOF