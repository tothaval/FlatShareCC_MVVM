/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  DoubleInputView 
 * 
 *  component code behind with some custom
 *  dependency properties, the component
 *  offers a label and a textbox
 */
using System.Windows;
using System.Windows.Controls;


namespace SharedLivingCostCalculator.Components
{
    /// <summary>
    /// Interaktionslogik für DoubleInputView.xaml
    /// </summary>
    public partial class DoubleInputView : UserControl
    {

        // dependency properties
        #region dependency properties

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DoubleInputView), new PropertyMetadata(false));


        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(DoubleInputView), new PropertyMetadata(""));


        public double LabelMinWidth
        {
            get { return (double)GetValue(LabelMinWidthProperty); }
            set { SetValue(LabelMinWidthProperty, value); }
        }
        public static readonly DependencyProperty LabelMinWidthProperty =
            DependencyProperty.Register("LabelMinWidth", typeof(double), typeof(DoubleInputView), new PropertyMetadata(100.0));


        public double LabelWidth
        {
            get { return (double)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }
        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register("LabelWidth", typeof(double), typeof(DoubleInputView), new PropertyMetadata(100.0));


        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(DoubleInputView), new PropertyMetadata(0.0));


        public TextAlignment ValueAlignment
        {
            get { return (TextAlignment)GetValue(ValueAlignmentProperty); }
            set { SetValue(ValueAlignmentProperty, value); }
        }
        public static readonly DependencyProperty ValueAlignmentProperty =
            DependencyProperty.Register("ValueAlignment", typeof(TextAlignment), typeof(DoubleInputView), new PropertyMetadata(null));


        public double ValueMinWidth
        {
            get { return (double)GetValue(ValueMinWidthProperty); }
            set { SetValue(ValueMinWidthProperty, value); }
        }
        public static readonly DependencyProperty ValueMinWidthProperty =
            DependencyProperty.Register("ValueMinWidth", typeof(double), typeof(DoubleInputView), new PropertyMetadata(250.0));

        #endregion dependency properties


        // constructors
        #region constructors

        public DoubleInputView()
        {
            InitializeComponent();
            this.TB_Value.DataContext = this;
        }

        #endregion constructors


    }
}
// EOF