using System.Windows;
using Files;
using System.Collections.Generic;
using System;

using System.Text;

namespace ATM.Screens
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public bool signed_in = false;
        
        public Login()
        {
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            string[] lines;
            FileAccess.ReadFile(@"C:\Users\10126901\Desktop\Credentials.csv", out lines);
            var credentials = new List<Tuple<string, string>>();

            int j = 0;
            foreach (string line in lines)
            {
                string[] entry;
                if (j == 0)
                {
                }
                else
                {
                    entry = line.Split(',');                        //convert comma seperated string to 1-d array
                    string userName = entry[0];
                    string password = entry[1];
                    credentials.Add(new Tuple<string, string>(userName, password));
                }
                j++;
            }

            for (int i = 0; i < credentials.Count; i++)
            {
                if (credentials[i].Item1 == userName.Text)
                {
                    if (credentials[i].Item2 == password.Text)
                    {
                        userName.Text = "Access granted";
                        signed_in = true;
                        this.Close();                                     // exit for loop if entry found
                    }
                    else
                    {
                        userName.Text = "Incorrect password";
                    }
                }
                else
                {
                    userName.Text = "Invalid user";
                }
            }
        }
    }
}
