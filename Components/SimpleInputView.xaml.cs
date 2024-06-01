﻿using SharedLivingCostCalculator.ViewModels;
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

namespace SharedLivingCostCalculator.Components.ComponentViews
{
    /// <summary>
    /// Interaktionslogik für SimpleInputView.xaml
    /// </summary>
    public partial class SimpleInputView : UserControl
    {
        public SimpleInputView()
        {
            InitializeComponent();

            this.TB_Value.DataContext = this;
        }


        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value);}
        }

        // Using a DependencyProperty as the backing store for InputDesignator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(SimpleInputView), new PropertyMetadata(""));

        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputDesignator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(SimpleInputView), new PropertyMetadata(null));

    }
}
