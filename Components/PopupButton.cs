using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace SharedLivingCostCalculator.Components
{
    class PopupButton : ButtonBase
    {
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(PopupButton), new PropertyMetadata(null));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(PopupButton), new PropertyMetadata(string.Empty));


        public string PopupText
        {
            get { return (string)GetValue(PopupTextProperty); }
            set { SetValue(PopupTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PopupText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupTextProperty =
            DependencyProperty.Register("PopupText", typeof(string), typeof(PopupButton), new PropertyMetadata(string.Empty));

        public PopupButton()
        {
            try
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupButton), new FrameworkPropertyMetadata(typeof(PopupButton)));
            }
            catch (Exception)
            {
            }

            Click += PopupButton_Click;
        }


        private void PopupButton_Click(object sender, RoutedEventArgs e)
        {
            Popup popup = new Popup();
            TextBlock popupText = new TextBlock();
            popupText.Text = PopupText;
            popupText.Background = (SolidColorBrush)Application.Current.FindResource("SCB_Text_Header");
            popupText.Foreground = (SolidColorBrush)Application.Current.FindResource("SCB_ButtonBackground");
            popupText.FontSize = 12.0;
            popupText.FontFamily = (FontFamily)Application.Current.FindResource("FF");
            popupText.Padding = new Thickness(5, 2, 5, 2);
            popup.Child = popupText;

            popup.IsOpen = true;
            popup.StaysOpen = false;
            popup.Placement = PlacementMode.Mouse;

            e.Handled = true;
        }


    }
}
