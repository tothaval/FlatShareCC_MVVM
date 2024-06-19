/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Resources 
 * 
 *  serializable helper class to
 *  store and retrieve application settings
 *  data to or from hard drive storage
 */
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;
using SharedLivingCostCalculator.Enums;


namespace SharedLivingCostCalculator.Utility
{

    [Serializable]
    [XmlRoot("Resources")]
    public class Resources
    {

        public string Language { get; set; } = SupportedLanguages.English.ToString();


        [XmlIgnore]
        public SolidColorBrush SCB_Background { get; set; } = new SolidColorBrush(Colors.White);


        public Color C_Background { get; set; }


        [XmlIgnore]
        public SolidColorBrush SCB_Text { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#444"));


        public Color C_Text { get; set; }


        [XmlIgnore]
        public SolidColorBrush SCB_Text_Header { get; set; } = new SolidColorBrush(Colors.Black);


        public Color C_Text_Header { get; set; }


        [XmlIgnore]
        public FontFamily FF { get; set; } = new FontFamily("Verdana");


        public string FontFamily { get; set; }


        public double FS { get; set; } = 11.0;


        public Resources GetResources()
        {
            // sprachdatei.xml laden und werte zuweisen?
            // liste aus gefundenen sprachdateien einlesen und in combobox itemsource reinladen
            // statt des enums? enums lassen sich glaub nicht erweitern.
            Language = Application.Current.Resources["Language"].ToString();


            SCB_Background = (SolidColorBrush)Application.Current.Resources["SCB_Background"];

            C_Background = SCB_Background.Color;


            SCB_Text = (SolidColorBrush)Application.Current.Resources["SCB_Text"];

            C_Text = SCB_Text.Color;


            SCB_Text_Header = (SolidColorBrush)Application.Current.Resources["SCB_Text_Header"];

            C_Text_Header = SCB_Text_Header.Color;


            FF = (FontFamily)Application.Current.Resources["FF"];

            FontFamily = FF.Source;


            FS = (double)Application.Current.Resources["FS"];


            return this;
        }


        public void SetResources()
        {
            Application.Current.Resources["Language"] = Language;

            Application.Current.Resources["FS"] = FS;
            Application.Current.Resources["FF"] = new FontFamily(FontFamily);

            Application.Current.Resources["SCB_Background"] = new SolidColorBrush(C_Background);
            Application.Current.Resources["SCB_Text"] = new SolidColorBrush(C_Text);
            Application.Current.Resources["SCB_Text_Header"] = new SolidColorBrush(C_Text_Header);
        
            new LanguageResources(Language);

            // country => specify ConverterCulture   
            // ??? how to apply global to app?

        }


    }
}
// EOF