using FGV;
using System;

namespace Files
{
    class FileAccess
    {
        public FileAccess()
        {

        }

        public static void ReadFile(string filename, out string[] lines)
        {
            lines = System.IO.File.ReadAllLines(filename);
        }
            
        public static void WriteFile(string filename, int account, string newEntry)
        {
            string[] lines = System.IO.File.ReadAllLines(filename);         //read all lines of file
            lines[account] = newEntry.Replace(lines[account], newEntry);    //replace contents of line index 'account'
            System.IO.File.WriteAllLines(filename, lines);
        }
        public static void OpenDialog()
        {
            Microsoft.Win32.OpenFileDialog file = new Microsoft.Win32.OpenFileDialog();     //specific to WPF, wont work for forms
            file.DefaultExt = ".csv";                                                       // Set filter for file extension and default file extension 
            //file.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            
            Nullable<bool> result = file.ShowDialog();                                      // Display OpenFileDialog by calling ShowDialog method. Specific to WPF, wont work for forms
            Global.Filename = file.FileName;
            Global.FileFound = result.Value;
        }

    }
}
