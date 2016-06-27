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
using System.Configuration;
namespace WpfMySql2
{
    /// <summary>
    /// Interaction logic for Setting_DB.xaml
    /// </summary>
    public partial class Setting_DB : Window
    {

        public Setting_DB()
        {
            InitializeComponent();
            
            textBoxUserName.MouseEnter += TextBoxUserName_MouseEnter;
            textBoxIP.MouseEnter += TextBoxIP_MouseEnter;
            textBoxPass.MouseEnter += TextBoxPass_MouseEnter;
            textBoxNameDB.MouseEnter += TextBoxNameDB_MouseEnter;

            textBoxIP.MouseLeave += TextBoxIP_MouseLeave;
            textBoxNameDB.MouseLeave += TextBoxNameDB_MouseLeave;
            textBoxPass.MouseLeave += TextBoxPass_MouseLeave;
            textBoxUserName.MouseLeave += TextBoxUserName_MouseLeave;

            textBoxIP.GotKeyboardFocus += TextBoxIP_GotKeyboardFocus;
            textBoxNameDB.GotKeyboardFocus += TextBoxNameDB_GotKeyboardFocus;
            textBoxPass.GotKeyboardFocus += TextBoxPass_GotKeyboardFocus;
            textBoxUserName.GotKeyboardFocus += TextBoxUserName_GotKeyboardFocus;

            textBoxIP.LostKeyboardFocus += TextBoxIP_LostKeyboardFocus;
            textBoxNameDB.LostKeyboardFocus += TextBoxNameDB_LostKeyboardFocus1;
            textBoxPass.LostKeyboardFocus += TextBoxPass_LostKeyboardFocus;
            textBoxUserName.LostKeyboardFocus += TextBoxUserName_LostKeyboardFocus;

            textBoxIP.Text = "beispielsweise 127.0.0.1";
            textBoxNameDB.Text = "beispielsweise myDataBase";
            textBoxPass.Text = "beispielsweise pass123";
            textBoxUserName.Text = "beispielsweise root";
        }

        private void TextBoxUserName_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
                ((TextBox)sender).Text = "beispielsweise root";
        }

        private void TextBoxUserName_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (((TextBox)sender).Text == "beispielsweise root")
                ((TextBox)sender).Text = "";
        }

        private void TextBoxPass_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
                ((TextBox)sender).Text = "beispielsweise pass123";
        }

        private void TextBoxPass_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (((TextBox)sender).Text == "beispielsweise pass123")
                ((TextBox)sender).Text = "";
        }

        private void TextBoxNameDB_LostKeyboardFocus1(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
                ((TextBox)sender).Text = "beispielsweise myDataBase";
        }

        private void TextBoxNameDB_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (((TextBox)sender).Text == "beispielsweise myDataBase")
                ((TextBox)sender).Text = "";
        }

        private void TextBoxIP_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
                ((TextBox)sender).Text = "beispielsweise 127.0.0.1";
        }

        private void TextBoxIP_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (((TextBox)sender).Text == "beispielsweise 127.0.0.1")
                ((TextBox)sender).Text = "";
        }

        private void TextBoxUserName_MouseLeave(object sender, MouseEventArgs e)
        {
            if (((TextBox)sender).Text == "")
                ((TextBox)sender).Text = "beispielsweise root";
        }

        private void TextBoxPass_MouseLeave(object sender, MouseEventArgs e)
        {
            if (((TextBox)sender).Text == "")
                ((TextBox)sender).Text = "beispielsweise pass123";
        }

        private void TextBoxNameDB_MouseLeave(object sender, MouseEventArgs e)
        {
            if (((TextBox)sender).Text == "")
                ((TextBox)sender).Text = "beispielsweise myDataBase";
        }

        private void TextBoxIP_MouseLeave(object sender, MouseEventArgs e)
        {
            if (((TextBox)sender).Text == "")
                ((TextBox)sender).Text = "beispielsweise 127.0.0.1";
        }

        private void TextBoxNameDB_MouseEnter(object sender, MouseEventArgs e)
        {
            if (((TextBox)sender).Text == "beispielsweise myDataBase")
                ((TextBox)sender).Text = "";
        }

        private void TextBoxPass_MouseEnter(object sender, MouseEventArgs e)
        {
            if (((TextBox)sender).Text == "beispielsweise pass123")
                ((TextBox)sender).Text = "";
        }

        private void TextBoxIP_MouseEnter(object sender, MouseEventArgs e)
        {
            if (((TextBox)sender).Text == "beispielsweise 127.0.0.1")
                ((TextBox)sender).Text = "";
        }

        private void TextBoxUserName_MouseEnter(object sender, MouseEventArgs e)
        {
            if (((TextBox)sender).Text == "beispielsweise root")
                ((TextBox)sender).Text = "";
        }             

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            string EncodedString = App.PassToXML(this.textBoxPass.Text);
            string IP = textBoxIP.Text;
            string userName = textBoxUserName.Text;
            string DBName = textBoxNameDB.Text;
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                if (settings["pass"] == null)
                {
                    settings.Add("pass", EncodedString);
                }
                else
                {
                    settings["pass"].Value = EncodedString;
                }

                if (settings["servIP"] == null)
                {
                    settings.Add("servIP", IP);
                }
                else
                {
                    settings["servIP"].Value = IP;
                }

                if (settings["userName"] == null)
                {
                    settings.Add("userName", userName);
                }
                else
                {
                    settings["userName"].Value = userName;
                }

                if (settings["DBName"] == null)
                {
                    settings.Add("DBName", DBName);
                }
                else
                {
                    settings["DBName"].Value = DBName;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
            Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.cancel = true;
            Close();           
        }
    }
}
