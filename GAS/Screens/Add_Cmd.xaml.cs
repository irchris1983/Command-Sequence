using GAS.Classes;
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
using System.Windows.Shapes;

namespace GAS.Screens
{
    /// <summary>
    /// Interaction logic for Add_Cmd.xaml
    /// </summary>
    public partial class Add_Cmd : Window
    {
        public Add_Cmd()
        {
            InitializeComponent();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            Grid_List.Add(index.Text, command.Text, comment.Text);
            this.Close();
        }
    }
}
