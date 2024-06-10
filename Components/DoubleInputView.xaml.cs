using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SharedLivingCostCalculator.Components
{
    /// <summary>
    /// Interaktionslogik für DoubleInputView.xaml
    /// </summary>
    public partial class DoubleInputView : UserControl
    {
        public DoubleInputView()
        {
            InitializeComponent();
            this.TB_Value.DataContext = this;
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(DoubleInputView), new PropertyMetadata(""));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(DoubleInputView), new PropertyMetadata(0.0));


        public double LabelMinWidth
        {
            get { return (double)GetValue(LabelMinWidthProperty); }
            set { SetValue(LabelMinWidthProperty, value); }
        }

        public static readonly DependencyProperty LabelMinWidthProperty =
            DependencyProperty.Register("LabelMinWidth", typeof(double), typeof(DoubleInputView), new PropertyMetadata(100.0));


        public double ValueMinWidth
        {
            get { return (double)GetValue(ValueMinWidthProperty); }
            set { SetValue(ValueMinWidthProperty, value); }
        }

        public static readonly DependencyProperty ValueMinWidthProperty =
            DependencyProperty.Register("ValueMinWidth", typeof(double), typeof(DoubleInputView), new PropertyMetadata(250.0));



        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DoubleInputView), new PropertyMetadata(false));




        public TextAlignment ValueAlignment
        {
            get { return (TextAlignment)GetValue(ValueAlignmentProperty); }
            set { SetValue(ValueAlignmentProperty, value); }
        }

        public static readonly DependencyProperty ValueAlignmentProperty =
            DependencyProperty.Register("ValueAlignment", typeof(TextAlignment), typeof(DoubleInputView), new PropertyMetadata(null));


    }
}
