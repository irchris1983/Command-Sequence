using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GAS.Classes
{
    public class DataObject                 //Define column headers
    {
        public string No { get; set; }
        public string Command { get; set; }
        public string Comment { get; set; }
    }
    
    class Grid_List
    {
        public static void Add(string num, string cmd, string com)
        {
            GridFGV.grid.Add(new DataObject() { No = num, Command = cmd, Comment = com });
            
        }
        public static void Remove(int index)
        {
            GridFGV.grid.RemoveAt(index);
            
        }
    }
    class GridFGV                           //Accessor for storing the datagrid info
    {
        public static ObservableCollection<DataObject> grid = new ObservableCollection<DataObject>();
    }
}
