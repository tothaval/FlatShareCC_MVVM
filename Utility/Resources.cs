using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Utility
{
    [Serializable]
    [XmlRoot("Resources")]
    public class Resources
    {

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
            Application.Current.Resources["FS"] = FS;
            Application.Current.Resources["FF"] = new FontFamily(FontFamily);

            Application.Current.Resources["SCB_Background"] = new SolidColorBrush(C_Background);
            Application.Current.Resources["SCB_Text"] = new SolidColorBrush(C_Text);
            Application.Current.Resources["SCB_Text_Header"] = new SolidColorBrush(C_Text_Header);
        }
    }
}
