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
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;

namespace NewtonDemo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Physics physics = new Physics();
        private int mode = 0;
        private int mode_graph = 0;

        public MainWindow()
        {
            InitializeComponent();
            versionLabel.Content = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Chart chart = this.FindName("WinFormsChart") as Chart;
            chart.Legends.Clear();
            chart.ChartAreas[0].AxisX.Title = "F, Ньютон";
            chart.ChartAreas[0].AxisY.Title = "a, м/с^2";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ComboBox.SelectedIndex == 0) //Ускорение
            {
                Label1.Content = "Введите массу телы(m, кг)";
                Label2.Content = "Введите суммарная сила телы(F, Н)";
                Label3.Content = "Ускорение телы равно(a, м/с^2)";
                First_TB.Text = String.Empty;
                Second_TB.Text = String.Empty;
                Result_TB.Text = String.Empty;
                mode = 0;
            }
            else if(ComboBox.SelectedIndex == 1) //Сила
            {
                Label1.Content = "Введите массу телы(m, кг)";
                Label2.Content = "Введите ускорение телы(a, м/с^2)";
                Label3.Content = "Суммарная сила телы равно(F, Н)";
                First_TB.Text = String.Empty;
                Second_TB.Text = String.Empty;
                Result_TB.Text = String.Empty;
                mode = 1;
            }
            else if(ComboBox.SelectedIndex == 2) //Масса
            {
                Label1.Content = "Введите суммарная сила телы(F, Н)";
                Label2.Content = "Введите ускорение телы(a, м/с^2)";
                Label3.Content = "Масса телы равно(m, кг)";
                First_TB.Text = String.Empty;
                Second_TB.Text = String.Empty;
                Result_TB.Text = String.Empty;
                mode = 2;
            }                     
        }

        private void First_TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(First_TB.Text.Contains("."))
            {
                First_TB.Text = First_TB.Text.Replace(".", ",");
            }
            try
            {
                Calculate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Second_TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Second_TB.Text.Contains("."))
            {
                First_TB.Text = First_TB.Text.Replace(".", ",");
            }
            try
            {
                Calculate();
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Calculate()
        {
            if (First_TB.Text != String.Empty && Second_TB.Text != String.Empty)
            {
                if (mode == 0)
                {
                    physics.Mass = Double.Parse(First_TB.Text);
                    physics.Force = Double.Parse(Second_TB.Text);
                    Result_TB.Text = physics.GetAcceleration().ToString("F3");
                }
                else if (mode == 1)
                {
                    physics.Mass = Double.Parse(First_TB.Text);
                    physics.Acceleration = Double.Parse(Second_TB.Text);
                    Result_TB.Text = physics.GetForce().ToString("F3");
                }
                else if (mode == 1)
                {
                    physics.Force = Double.Parse(First_TB.Text);
                    physics.Acceleration = Double.Parse(Second_TB.Text);
                    Result_TB.Text = physics.GetMass().ToString("F3");
                }
            }
            else
            {
                Result_TB.Text = String.Empty;
            }
        }

        private void Grpah_CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Graph_CB.SelectedIndex == 0) //Функция a(F)
            {
                StartLabel.Content = "Начало F";
                EndLabel.Content = "Конец F";
                IntervalLabel.Content = "Интервал dF";
                mode_graph = 0;
                Start_TB.Text = String.Empty;
                End_TB.Text = String.Empty;
                Interval_TB.Text = String.Empty;
            }
            else if(Graph_CB.SelectedIndex == 1) //Функция F(a)
            {
                StartLabel.Content = "Начало a";
                EndLabel.Content = "Конец a";
                IntervalLabel.Content = "Интервал da";
                mode_graph = 1;
                Start_TB.Text = String.Empty;
                End_TB.Text = String.Empty;
                Interval_TB.Text = String.Empty;
            }
        }

        private void Draw_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Start_TB.Text != String.Empty && End_TB.Text != String.Empty && Interval_TB.Text != String.Empty && MassGraph_TB.Text != String.Empty)
                {                                     
                    double y;
                    Int64 start = Int64.Parse(Start_TB.Text);
                    Int64 end = Int64.Parse(End_TB.Text);
                    Int64 dx = Int64.Parse(Interval_TB.Text);
                    Physics physics_graph = new Physics();
                    physics_graph.Mass = Double.Parse(MassGraph_TB.Text);

                    if (start >= end)
                        throw new Exception("Диапозон начало больше или равно к конец");

                    Chart chart = this.FindName("WinFormsChart") as Chart;
                    
                    chart.Series[0].Points.Clear();
                    chart.ChartAreas[0].AxisX.Minimum = start;
                    chart.ChartAreas[0].AxisX.Maximum = end;
                    chart.ChartAreas[0].AxisX.Interval = dx;

                    if (mode_graph == 0)
                    {                       
                        chart.ChartAreas[0].AxisY.Minimum = physics_graph.GetAcceleration(start, physics_graph.Mass);
                        chart.ChartAreas[0].AxisY.Maximum = physics_graph.GetAcceleration(end, physics_graph.Mass);
                        chart.ChartAreas[0].AxisX.Title = "F, Ньютон";
                        chart.ChartAreas[0].AxisY.Title = "a, м/с^2";
                    }
                    else if(mode_graph == 1)
                    {
                        chart.ChartAreas[0].AxisY.Minimum = physics_graph.GetForce(start, physics_graph.Mass);
                        chart.ChartAreas[0].AxisY.Maximum = physics_graph.GetForce(end, physics_graph.Mass);
                        chart.ChartAreas[0].AxisX.Title = "a, м/с^2";
                        chart.ChartAreas[0].AxisY.Title = "F, Ньютон";                       
                    } 
                    

                    for (Int64 x = start; x <= end; x += dx)
                    {
                        if(mode_graph == 0)
                        {
                            physics_graph.Force = x;
                            y = physics_graph.GetAcceleration();
                            chart.Series[0].Points.AddXY(x, y);
                        }
                        else if (mode_graph == 1)
                        {
                            physics_graph.Acceleration = x;
                            y = physics_graph.GetForce();
                            chart.Series[0].Points.AddXY(x, y);
                        }                       
                    }
                    WFHost.Child = chart;
                }                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
