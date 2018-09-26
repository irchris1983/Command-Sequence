using Drivers;
using Files;
using GAS.Classes;
using GAS.Screens;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FGV;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Collections.Generic;
using System.Windows.Documents;

namespace GAS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Driver2 d1 = new Driver2();
        private System.Timers.Timer m_timer;
        private SynchronizationContext m_sync;
        public MainWindow()
        {
            InitializeComponent();
            dataGrid.CanUserAddRows = false;

            //Needs to have NI drivers installed to work but could be swapped for different driver
            //Thread DAQ_AI = new Thread(new ThreadStart(d1.DAQAI));
            //DAQ_AI.Start();
            //Thread DAQ_DI = new Thread(new ThreadStart(d1.DAQDI));
            //DAQ_DI.Start();
            //Thread DAQ_DO = new Thread(new ThreadStart(d1.DAQDO));
            //DAQ_DO.Start();

            m_sync = SynchronizationContext.Current;
            m_timer = new System.Timers.Timer();
            m_timer.Interval = 500;
            m_timer.AutoReset = true;
            m_timer.Elapsed += new System.Timers.ElapsedEventHandler(AI_FB);
           // m_timer.Elapsed += new System.Timers.ElapsedEventHandler(DO_FB);
            m_timer.Start();

            //Login login = new Login();
            //login.InitializeComponent();
            //login.ShowDialog();
            //if (login.signed_in != true)
            //{
            //    this.Close();
            //}
        }
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            d1.exit = true;
            this.Close();
        }

        private void daq_Click(object sender, RoutedEventArgs e)
        {
            DAQ daq = new DAQ();
            daq.InitializeComponent();
            daq.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            daq.ShowDialog();
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            FileAccess.OpenDialog();

            if (Global.FileFound == true)                                                   // Get the selected file name and display in a TextBox 
            {
                if (dataGrid.Items.Count != 0)                                              //check if datagrid currently has any data
                {
                    this.dataGrid.ItemsSource = null;                                       //delete contents of datagrid
                    GridFGV.grid.Clear();                                                   //remove contents of GridFGV accessor
                }
                string[] lines;
                FileAccess.ReadFile(Global.Filename, out lines);                             // Open document 
                Global.Filesize = lines.Count();
                List cmds = new List();
                int i = 0;
                foreach (string line in lines)
                {
                    string[] entry;
                    entry = line.Split(',');                                                //convert comma seperated string to 1-d array
                    if (entry.Count() > 4)                                                  //check that the file has correct number of columns
                    {
                        textBox.Text = "Invalid file: number of columns not compatible";
                        break;
                    }
                    Grid_List.Add(entry[0], entry[1], entry[2]);
                    
                    this.dataGrid.ItemsSource = GridFGV.grid;
                    i++;
                }
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            string line;
            string[] entry = new string[3];
            string[] fileUpdate = new string[Global.Filesize];

            for (int i = 0; i < Global.Filesize; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    entry[j] = GetCellValue(dataGrid, i, j);
                }
                line = string.Join(",", entry);
                fileUpdate[i] = line;
            }
            System.IO.File.WriteAllLines(Global.Filename, fileUpdate);         //write grid contents to file
        }

        public string GetCellValue(DataGrid datagrid, int row, int column)
        {
            var cellInfo = new DataGridCellInfo(datagrid.Items[row], dataGrid.Columns[column]);

            DataGridCell cell = null;
            var cellContent = cellInfo.Column.GetCellContent(cellInfo.Item);
            if (cellContent != null)
                cell = (DataGridCell)cellContent.Parent;

            if (cell == null) return string.Empty;

            // if DataGridTextColumn / DataGridComboBoxColumn is used 
            // or AutoGeneratedColumns is True
            if (cell.Content is TextBlock)
                return ((TextBlock)cell.Content).Text;
            else if (cell.Content is ComboBox)
                return ((ComboBox)cell.Content).Text;

            // if DataGridTemplateColumn is used 
            // assuming cells are either TextBox, TextBlock or ComboBox. Other Types could be handled the same way.
            else
            {
                var txtPresenter = FindVisualChild<TextBox>((ContentPresenter)cell.Content);
                if (txtPresenter != null) return txtPresenter.Text;
                var txbPresenter = FindVisualChild<TextBlock>((ContentPresenter)cell.Content);
                if (txbPresenter != null) return txbPresenter.Text;
                var cmbPresenter = FindVisualChild<ComboBox>((ContentPresenter)cell.Content);
                if (cmbPresenter != null) return cmbPresenter.Text;
            }
            return string.Empty;
        }// found online
        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }// found online

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            //ContextMenu cm = this.FindResource("RemoveButton") as ContextMenu;
            //cm.PlacementTarget = sender as Button;
            //cm.IsOpen = true;

            Grid_List.Remove(dataGrid.SelectedIndex);
            // this.dataGrid.ItemsSource = null;
            this.dataGrid.ItemsSource = GridFGV.grid;
            Global.Filesize = Global.Filesize - 1;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            Add_Cmd addCmd = new Add_Cmd();                     //create new instance of Add_Cmd popup
            addCmd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addCmd.InitializeComponent();
            addCmd.ShowDialog();

            this.dataGrid.ItemsSource = null;
            this.dataGrid.ItemsSource = GridFGV.grid;
            Global.Filesize = Global.Filesize + 1;
        }
        private void AI_FB(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                m_sync.Post((o) =>
                {
                    textBox.Text = Global.AI_Single.ToString();
                }, null);
            });
        }   //this method is called when the timer, defined at the start, is triggered
        //private void do_fb(object sender, system.timers.elapsedeventargs e)
        //{
        //    task.factory.startnew(() =>
        //    {
        //        m_sync.post((o) =>
        //        {
        //            //checkbox.ischecked = global.do;
        //            checkbox.ischecked = global.di[1];
        //        }, null);
        //    });
        //}
    }
}
