﻿using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Series;

namespace Simpson_Rule
{
    
    public partial class MainWindow : Window
    {
        private Func<double,double> SelectedFunction;
        private string FunctionName;
        
        public MainWindow()
        {
            InitializeComponent();
        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            FunctionName = pressed.Content.ToString();
            switch (pressed.Name)
            {
                case "SinX":
                    SelectedFunction = Math.Sin;
                    break;

                case "OneByX":
                    SelectedFunction = x => 1 / x;
                    break;

                case "eToPowerX":
                    SelectedFunction = Math.Exp;
                    break;

                case "XSinX":
                    SelectedFunction = x => x * Math.Sin(x);
                    break;
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double result = double.MinValue;
            double result1 = double.MinValue;
            if (SelectedFunction != null && BoundMin.Text != "" && BoundMax.Text != "" && IntergrationIntervals.Text != "")
            {
                double min = double.Parse(BoundMin.Text);
                double max = double.Parse(BoundMax.Text);
                int intervals = int.Parse(IntergrationIntervals.Text);
                if (max < min)
                {
                    var temp = min;
                    min = max;
                    max = temp;
                }
                // Draw plot
                PlotModel myModel = new PlotModel();
                myModel.Title = FunctionName;
                myModel.Series.Add(new FunctionSeries(SelectedFunction, min, max, 0.1, FunctionName.Split('=').Last()));
                MyPlot.Model = myModel;
                result1 = MathNet.Numerics.Integrate.OnClosedInterval(SelectedFunction, min, max);
                // Calcualte 
                try
                {
                    result = SimpsonIntegration.SimpsonRule(SelectedFunction, min, max, intervals);
                    
                    
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            if(result != double.MinValue)
            {
                Result.Visibility = Visibility.Visible;
                ResultTitle.Visibility = Visibility.Visible;
                Error.Visibility = Visibility.Visible;

                Result.Text = result.ToString();
                Error.Text = Math.Abs(result-result1).ToString();
            }
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}