using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SharedLivingCostCalculator.Utility
{
    internal class UISettings : ConfigurationSection
    {
        [ConfigurationProperty("background", DefaultValue = typeof(SolidColorBrush))]
        public SolidColorBrush Background
        {
            get { return (SolidColorBrush)this["background"]; }
            set { this["background"] = value; }
        }

        [ConfigurationProperty("foreground", DefaultValue = typeof(SolidColorBrush))]
        public SolidColorBrush Foreground
        {
            get { return (SolidColorBrush)this["foreground"]; }
            set { this["foreground"] = value; }
        }

        [ConfigurationProperty("fontfamily", DefaultValue = "Verdana")]
        public string FontFamily
        {
            get { return (string)this["fontfamily"]; }
            set { this["fontfamily"] = value; }
        }

        [ConfigurationProperty("fontsize", DefaultValue = 14)]
        [IntegerValidator(MaxValue = 100, MinValue = 5)]
        public int FontSize
        {
            get { return (int)this["fontsize"]; }
            set { this["fontsize"] = value; }
        }
    }
}
