using GAS.Classes.DAQ_Classes;
using NationalInstruments.DAQmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using FGV;

namespace GAS.Screens
{
    /// <summary>
    /// Interaction logic for DAQ.xaml
    /// </summary>
    public partial class DAQ : Window
    {
        
        List<string> DI = new List<string>();
        List<string> DO = new List<string>();
        List<string> AI = new List<string>();
        List<string> AO = new List<string>();
        List<string> operations = new List<string> { "Read DI", "Write DO", "Read AI", "Write AO", "Counter" };
        List<string> selected_channels = new List<string> { };
        
        public DAQ()
        {
            InitializeComponent();
            operation.ItemsSource = operations;
            operation.SelectedIndex = 0;
            
            foreach (string s in DaqSystem.Local.Tasks)
            {
                try
                {
                    using (NationalInstruments.DAQmx.Task t = DaqSystem.Local.LoadTask(s))
                    {
                        t.Control(TaskAction.Verify);

                        if (t.DIChannels.Count > 0 && t.Timing.SampleTimingType == SampleTimingType.OnDemand)
                        {
                            DI.Add(s);
                        }
                        else if (t.DOChannels.Count > 0 && t.Timing.SampleTimingType == SampleTimingType.OnDemand)
                        {
                            DO.Add(s);
                        }
                        else if (t.AIChannels.Count > 0)
                        {
                            AI.Add(s);
                        }
                        else if (t.AOChannels.Count > 0)
                        {
                            AO.Add(s);
                        }
                    }
                }
                catch (DaqException)
                {
                }
            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void executeTask_Click(object sender, RoutedEventArgs e)
        {
            //string taskName = comboBox.SelectedItem.ToString();
            //using (NationalInstruments.DAQmx.Task analogReadTask = DaqSystem.Local.LoadTask(taskName))
            //{
            //    // Get switch values
            //    double[] AI = new double[4];

            //    AnalogSingleChannelReader AI_Channel = new AnalogSingleChannelReader(analogReadTask.Stream);
            //    AI[0] = AI_Channel.ReadSingleSample();
            //    textBox.Text = AI[0].ToString();
            //}
            _const.Text = Global.AI_Single.ToString();
            checkBox1.IsChecked = Global.DI[3];
            Global.DO = digitalOut.IsChecked.Value;
        }
    
    
        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox.IsChecked == true)
            {
                Tasklist.DO = true;
            }
            else if (checkBox.IsChecked == false)
            {
                Tasklist.DO = false;
            }
            else
            {
            }
        }
        
        private void operation_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
            if (operation.SelectedItem.ToString() == "Read DI")
            {
                selected_channels = DI;
            }
            else if (operation.SelectedItem.ToString() == "Write DO")
            {
                selected_channels = DO;
            }
            else if (operation.SelectedItem.ToString() == "Read AI")
            {
                selected_channels = AI;
            }
            else if (operation.SelectedItem.ToString() == "Write AO")
            {
                selected_channels = AO;
            }
            else if (operation.SelectedItem.ToString() == "Counter")
            {
                //selected_channels = { };
            }
            else
            {
            }
            if (selected_channels.Count > 0)
            {
                comboBox.Visibility = Visibility.Visible;
            }
            else
            {
                comboBox.Visibility = Visibility.Hidden;
            }
            comboBox.ItemsSource = selected_channels;
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }
        
        
    }
}
