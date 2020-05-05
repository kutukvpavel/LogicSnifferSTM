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
using LiveCharts;
using LiveCharts.Defaults; //Contains the already defined types
using LiveCharts.Wpf;

namespace DataVisualizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
                          

        public void LoadData()
        {
            ChartValues<ObservablePoint> Values;
            Values = new ChartValues<ObservablePoint>();
            Values.Add(new ObservablePoint(1, 1));
            Values.Add(new ObservablePoint(2, 2));
            Values.Add(new ObservablePoint(3, 4));
            MyChart.Values = Values;
        }
    }
}
