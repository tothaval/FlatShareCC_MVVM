/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Resources 
 * 
 *  serializable helper class to
 *  store and retrieve application settings
 *  data to or from hard drive storage
 */
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml.Serialization;
using SharedLivingCostCalculator.Enums;


namespace SharedLivingCostCalculator.Utility
{

    [Serializable]
    [XmlRoot("Resources")]
    public class Resources
    {

        // properties & fields
        #region properties

        public Color C_Background { get; set; }


        public Color C_Selection{ get; set; }


        public Color C_Text { get; set; }


        public Color C_Text_Header { get; set; }


        public CornerRadius ButtonCornerRadius { get; set; } 

        public CornerRadius VisibilityFieldCornerRadius { get; set; }



        [XmlIgnore]
        public FontFamily FF { get; set; } = new FontFamily("Verdana");


        public string FontFamily { get; set; }


        /// <summary>
        /// FontSize
        /// </summary>
        public double FS { get; set; } = 11.0;


        /// <summary>
        /// HeaderFontsize
        /// </summary>
        public double HFS { get; set; } = 11.0;


        public string Language { get; set; } = "English";


        [XmlIgnore]
        public CultureInfo CurrentCulture { get; set; } = CultureInfo.CurrentCulture;



        [XmlIgnore]
        public SolidColorBrush SCB_Background { get; set; } = new SolidColorBrush(Colors.White);


        [XmlIgnore]
        public SolidColorBrush SCB_Selection{ get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AEF"));


        [XmlIgnore]
        public SolidColorBrush SCB_Text { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#444"));


        [XmlIgnore]
        public SolidColorBrush SCB_Text_Header { get; set; } = new SolidColorBrush(Colors.Black);

        #endregion properties


        // methods
        #region methods

        public Resources GetResources()
        {
            // sprachdatei.xml laden und werte zuweisen?
            // liste aus gefundenen sprachdateien einlesen und in combobox itemsource reinladen
            // statt des enums? enums lassen sich glaub nicht erweitern.
            Language = Application.Current.Resources["Language"].ToString();


            SCB_Background = (SolidColorBrush)Application.Current.Resources["SCB_Background"];

            C_Background = SCB_Background.Color;


            SCB_Selection = (SolidColorBrush)Application.Current.Resources["SCB_Selection"];

            C_Selection = SCB_Selection.Color;


            SCB_Text = (SolidColorBrush)Application.Current.Resources["SCB_Text"];

            C_Text = SCB_Text.Color;


            SCB_Text_Header = (SolidColorBrush)Application.Current.Resources["SCB_Text_Header"];

            C_Text_Header = SCB_Text_Header.Color;



            FF = (FontFamily)Application.Current.Resources["FF"];

            FontFamily = FF.Source;


            FS = (double)Application.Current.Resources["FS"];


            HFS = (double)Application.Current.Resources["FS"] * 1.5;


            ButtonCornerRadius = (CornerRadius)Application.Current.Resources["Button_CornerRadius"];
            VisibilityFieldCornerRadius = (CornerRadius)Application.Current.Resources["VisibilityField_CornerRadius"];


            return this;
        }


        public void FindLanguages()
        {

        }


        public void SetResources()
        {
            Application.Current.Resources["Language"] = Language;

            Application.Current.Resources["FS"] = FS;
            Application.Current.Resources["FF"] = new FontFamily(FontFamily);

            Application.Current.Resources["HFS"] = FS * 1.25;

            Application.Current.Resources["Button_CornerRadius"] = ButtonCornerRadius;

            Application.Current.Resources["VisibilityField_CornerRadius"] = VisibilityFieldCornerRadius;


            Application.Current.Resources["SCB_Background"] = new SolidColorBrush(C_Background);
            Application.Current.Resources["SCB_Text"] = new SolidColorBrush(C_Text);
            Application.Current.Resources["SCB_Text_Header"] = new SolidColorBrush(C_Text_Header);
            Application.Current.Resources["SCB_Selection"] = new SolidColorBrush(C_Selection);

            Application.Current.Resources["Culture"] = XmlLanguage.GetLanguage(CurrentCulture.IetfLanguageTag);

            new LanguageResources(Language);

        }

        #endregion methods


    }
}
// EOF