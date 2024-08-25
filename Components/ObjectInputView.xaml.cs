/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ObjectInputView 
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
    /// Interaktionslogik für ObjectInputView.xaml
    /// </summary>
    public partial class ObjectInputView : UserControl
    {

        // dependency properties
        #region dependency properties

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(ObjectInputView), new PropertyMetadata(false));


        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(ObjectInputView), new PropertyMetadata(""));


        public double LabelMinWidth
        {
            get { return (double)GetValue(LabelMinWidthProperty); }
            set { SetValue(LabelMinWidthProperty, value); }
        }
        public static readonly DependencyProperty LabelMinWidthProperty =
            DependencyProperty.Register("LabelMinWidth", typeof(double), typeof(ObjectInputView), new PropertyMetadata(100.0));


        public double LabelWidth
        {
            get { return (double)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }
        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register("LabelWidth", typeof(double), typeof(ObjectInputView), new PropertyMetadata(100.0));


        public string PopupHint
        {
            get { return (string)GetValue(PopupHintProperty); }
            set { SetValue(PopupHintProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PopupHint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopupHintProperty =
            DependencyProperty.Register("PopupHint", typeof(string), typeof(ObjectInputView), new PropertyMetadata(string.Empty));


        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(ObjectInputView), new PropertyMetadata(null));


        public TextAlignment ValueAlignment
        {
            get { return (TextAlignment)GetValue(ValueAlignmentProperty); }
            set { SetValue(ValueAlignmentProperty, value); }
        }
        public static readonly DependencyProperty ValueAlignmentProperty =
            DependencyProperty.Register("ValueAlignment", typeof(TextAlignment), typeof(ObjectInputView), new PropertyMetadata(null));


        public double ValueMinWidth
        {
            get { return (double)GetValue(ValueMinWidthProperty); }
            set { SetValue(ValueMinWidthProperty, value); }
        }
        public static readonly DependencyProperty ValueMinWidthProperty =
            DependencyProperty.Register("ValueMinWidth", typeof(double), typeof(ObjectInputView), new PropertyMetadata(250.0));

        #endregion dependency properties


        // constructors
        #region constructors

        public ObjectInputView()
        {
            InitializeComponent();

            this.TB_Value.DataContext = this;
        }

        #endregion constructors


    }
}
// EOF