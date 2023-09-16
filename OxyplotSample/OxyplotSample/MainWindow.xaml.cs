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
using OxyPlot;
using OxyPlot.Series;
using System.Windows.Threading;

namespace OxyplotSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel m_vm;
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public class MainViewModel
    {
        public PlotModel MyModel { get; private set; }

        private double[] plotRawdata1 = new double[1024];
        private double[] plotRawdata2 = new double[1024];
        private double[] plotRawdata3 = new double[1024];
        private double[] plotRawdata4 = new double[1024];

        private LineSeries line1 = new LineSeries();
        private LineSeries line2 = new LineSeries();
        private LineSeries line3 = new LineSeries();
        private LineSeries line4 = new LineSeries();


        public MainViewModel()
        {
            var filename = @"C:\mySample.txt";

            this.GetPlotdataFromFile(filename);

            double[] d = new double[4];

            d[0] = plotRawdata1.Max();
            d[1] = plotRawdata2.Max();
            d[2] = plotRawdata3.Max();
            d[3] = plotRawdata4.Max() * 0.04;

            d[0] = plotRawdata1.Min();
            d[1] = plotRawdata2.Min();
            d[2] = plotRawdata3.Min();
            d[3] = plotRawdata4.Min();

            double minVal = d.Min();
            int peakIndex = int.MinValue;

            double m = double.MinValue;
            for( int i = 0; i< plotRawdata3.Length; i++)
            {
                if(plotRawdata3[i] > m)
                {
                    m = plotRawdata3[i];
                    peakIndex = i;
                }
            }

            int maJorInterval = (int)((d.Max() - minVal) / 10);

            this.createGraphdata(maxVal * 1.1, minVal, maJorInterval, peakIndex);
        }

        private bool GetPlotdataFromFile(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\mySample.txt");

            int index = 0;
            foreach( var n in lines)
            {
                string[] strs= n.Split(',');
                plotRawdata1[index] = double.Parse(strs[2]);
                plotRawdata2[index] = double.Parse(strs[3]);
                plotRawdata3[index] = double.Parse(strs[4]);
                plotRawdata4[index] = double.Parse(strs[5]);

                index++;
            }

            return true;
        }

        private void createGraphdata(double maxVal, double minVal, int maJorInterval, int peakIndex)
        {
            this.MyModel = new PlotModel();
            MyModel.Updated += MyModel_Updated;
            
            MyModel.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                MajorStep = 100,
                MinorStep = 10,
                MinorGridlineStyle = LineStyle.Dot,
                MinorGridlineThickness = 1,
                MajorGridlineThickness = 1,
                MajorGridlineStyle = LineStyle.Dot,
                Minimum = 0,
                Maximum = 1024,
                AbsoluteMinimum = 0,
                AbsoluteMaximum = 1024
            }); ;


            MyModel.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                MajorStep = maJorInterval,
                MinorGridlineStyle = LineStyle.Dot,
                MinorGridlineThickness = 1,
                MajorGridlineThickness = 1,
                MajorGridlineStyle = LineStyle.Dot,
                AbsoluteMinimum = minVal,
                AbsoluteMaximum = maxVal,
                Minimum = minVal,
                Maximum = maxVal,
            }); ;


            line1.Color = OxyColor.FromRgb(0, 0, 255);
            line1.StrokeThickness = 2;

            line2.Color = OxyColor.FromRgb(0, 255, 255);
            line2.StrokeThickness = 2;

            line3.Color = OxyColor.FromRgb(0, 128, 128);
            line3.StrokeThickness = 2;

            line4.Color = OxyColor.FromRgb(128, 0, 128);
            line4.StrokeThickness = 2;

            int i = 0;
            foreach( var n in plotRawdata1)
            {
                DataPoint dp = new DataPoint(i, n);
                line1.Points.Add(dp);
                i++;
            }
            MyModel.Series.Add(line1);

            i = 0;
            foreach (var n in plotRawdata2)
            {
                DataPoint dp = new DataPoint(i, n);
                line2.Points.Add(dp);
                i++;
            }
            MyModel.Series.Add(line2);

            i = 0;
            foreach (var n in plotRawdata3)
            {
                DataPoint dp = new DataPoint(i, n);
                line3.Points.Add(dp);
                i++;
            }
            MyModel.Series.Add(line3);

            i = 0;
            foreach (var n in plotRawdata4)
            {
                DataPoint dp = new DataPoint(i, n * 0.05);
                line4.Points.Add(dp);
                i++;
            }
            MyModel.Series.Add(line4);

            var labelX = new OxyPlot.Annotations.TextAnnotation
            {
                Text = line3.Points[peakIndex].Y.ToString(),
                FontSize = 12,
                TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Left,
                TextPosition = new DataPoint(line3.Points[peakIndex].X, line3.Points[peakIndex].Y),
                StrokeThickness = 0,
            };


            MyModel.Annotations.Add(labelX);

            MyModel.InvalidatePlot(true);


        }

    	// MyModel_Updated
        private void MyModel_Updated(object sender, EventArgs e)
        {
            Console.WriteLine("MyModel_Updated is called.");

            MyModel.Updated -= MyModel_Updated;
        }
    }

}
