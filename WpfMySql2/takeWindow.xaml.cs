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
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.Data;
namespace WpfMySql2
{
    /// <summary>
    /// Interaction logic for takeWindow.xaml
    /// </summary>
    /// 

    public partial class takeWindow : Window
    {
        public long lastID;
        public string dataZeit = "";
       public string einMitarbeiterNach = "";
        public string meinKundeID = "";
        public takeWindow()
        {
            InitializeComponent();
            dataZeit = DateTime.Now.ToString();
            InitVonCbx();
            InitAnrCbx();
            InitGrpCbx();

            //mitarbeiterNach.IsEditable = true;
            //mitarbeiterNach.IsReadOnly = true;
            //mitarbeiterNach.Text = "Areit";            
            //mitarbeiterNach.ItemsSource = new List<String> { "A", "B", "C", "D", "E" };

        }
        private void InitGrpCbx()
        {
            ComboBoxItem cbx = new ComboBoxItem();
            cbx.IsSelected = true;
            cbx.Content = "\t alle Adressen";

            comboBoxKundenGruppe.Items.Add(cbx);
            foreach (KeyValuePair<string, string> item in MainWindow.kundenGruppe)
            {
                if ("alle Adressen" != item.Value)
                {
                    cbx = new ComboBoxItem();
                    cbx.Content = item.Value;
                    comboBoxKundenGruppe.Items.Add(cbx);
                }
            }
        }
        private void InitAnrCbx()
        {
            ComboBoxItem cbx = new ComboBoxItem();
            cbx.IsSelected = true;
            cbx.Content = "\t***";
            
            comboBoxAnrede.Items.Add(cbx);
            foreach (string item in MainWindow.AnredeList)
            {
                cbx = new ComboBoxItem();
                cbx.Content = item;
           
                comboBoxAnrede.Items.Add(cbx);
            }
        }
        private void InitVonCbx()
        {
            ComboBoxItem cbx = new ComboBoxItem();
            cbx.IsSelected = true;
            cbx.Content = "\t***";
          
            mitarbeiterNach.Items.Add(cbx);

            foreach (string item in MainWindow.mitarbeiter)
            {
                cbx = new ComboBoxItem();
                cbx.Content = item;
          
                mitarbeiterNach.Items.Add(cbx);
            }
        }

        private void clientBaseBtn_Click(object sender, RoutedEventArgs e)
        {
            clientsBase clientBaseWnd = new clientsBase();
            clientBaseWnd.ShowDialog();
            if (MainWindow.kundenID != null)
            {
                kundenNrtxt.Text = MainWindow.kundenID["REC_ID"].ToString();
                nameTxt.Text = MainWindow.kundenID["NAME1"].ToString();
                VornameTxt.Text = MainWindow.kundenID["NAME2"].ToString();
                StrasseTxt.Text = MainWindow.kundenID["STRASSE"].ToString();
                PLZTxt.Text = MainWindow.kundenID["PLZ"].ToString();
                OrtTxt.Text = MainWindow.kundenID["ORT"].ToString();
                TelefonTxt.Text = MainWindow.kundenID["TELE1"].ToString();
                MobilTxt.Text = MainWindow.kundenID["TELE2"].ToString();
                EMailTxt.Text = MainWindow.kundenID["EMAIL"].ToString();
                //////////////////////////////////////////////////////////////////////
                comboBoxKundenGruppe.Items.Clear();
                string kundenGrup = MainWindow.kundenID["KUNDENGRUPPE"].ToString();
                ComboBoxItem cbx = new ComboBoxItem();
                cbx.IsSelected = true;
                foreach (KeyValuePair<string, string> item in MainWindow.kundenGruppe)
                {
                    if (item.Key == kundenGrup)
                        cbx.Content = item.Value;
                }
                comboBoxKundenGruppe.Items.Add(cbx);
                //////////////////////////////////////////////////////////////
                string anrede = MainWindow.kundenID["ANREDE"].ToString();
                comboBoxAnrede.Items.Clear();
                ComboBoxItem cbx2 = new ComboBoxItem();
                cbx2.IsSelected = true;
                cbx2.Content = anrede;
                // cbx.Background = Brushes.LightSteelBlue;

                comboBoxAnrede.Items.Add(cbx2);


                MainWindow.kundenID = null;
            }
        }

        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (((ComboBoxItem)mitarbeiterNach.SelectedValue).Content.ToString() != "\t***" && geratTxt.Text != "" && serialNummerTxt.Text != "" && BemerkungTxt.Text != "" && (passKundenTxt.Text != "" || (textMuster1.Text!="" || textMuster2.Text != ""|| textMuster3.Text != ""|| textMuster4.Text != ""|| textMuster5.Text != "" || textMuster6.Text != ""|| textMuster7.Text != ""|| textMuster8.Text != ""|| textMuster9.Text != "")) && zubehorTxt.Text != "" && nameTxt.Text != "" && VornameTxt.Text != "" && (TelefonTxt.Text != "" || MobilTxt.Text != "" || EMailTxt.Text != ""))
            {
                using (MySqlConnection cn = new MySqlConnection())
                {
                    cn.ConnectionString = App.GetConnection();
                    try
                    {
                        cn.Open();
                        if (kundenNrtxt.Text.ToString() == "")
                        {
                            string commNeuKunde = string.Format("Insert Into adressen (matchcode, name1,name2, strasse,tele1,tele2,plz, ort,email, anrede,kundengruppe) value ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", (nameTxt.Text + " " + VornameTxt.Text), nameTxt.Text, VornameTxt.Text, StrasseTxt.Text, TelefonTxt.Text, MobilTxt.Text, PLZTxt.Text, OrtTxt.Text, EMailTxt.Text, ((ComboBoxItem)comboBoxAnrede.SelectedItem).Content.ToString(), ((ComboBoxItem)comboBoxKundenGruppe.SelectedItem).Content.ToString());
                            using (MySqlCommand cmd = new MySqlCommand(commNeuKunde, cn))
                            {
                                cmd.ExecuteNonQuery();
                                meinKundeID = cmd.LastInsertedId.ToString();
                            }
                        }
                        else
                        {
                            meinKundeID = kundenNrtxt.Text;
                        }

                        ComboBoxItem cmbx = (ComboBoxItem)mitarbeiterNach.SelectedValue;
                        einMitarbeiterNach = (String)cmbx.Content;
                        if (textMuster1.Text == "")
                            textMuster1.Text = "*";
                        if (textMuster2.Text == "")
                            textMuster2.Text = "*";
                        if (textMuster3.Text == "")
                            textMuster3.Text = "*";
                        if (textMuster4.Text == "")
                            textMuster4.Text = "*";
                        if (textMuster5.Text == "")
                            textMuster5.Text = "*";
                        if (textMuster6.Text == "")
                            textMuster6.Text = "*";
                        if (textMuster7.Text == "")
                            textMuster7.Text = "*";
                        if (textMuster8.Text == "")
                            textMuster8.Text = "*";
                        if (textMuster9.Text == "")
                            textMuster9.Text = "*";
                        string graphKey = textMuster1.Text + textMuster2.Text + textMuster3.Text + textMuster4.Text + textMuster5.Text + textMuster6.Text + textMuster7.Text + textMuster8.Text + textMuster9.Text;
                        string comm = string.Format("Insert Into service (dateTime, status, clientID, gerat, serialNummer, zubehor, fehlerBeschreibung, maxPrice, mitarbeiterNach, passKunden, graphKey, bemerkung, zustadn, internVermerk) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')", dataZeit , "Angenommen von", meinKundeID.ToString(), geratTxt.Text, serialNummerTxt.Text, zubehorTxt.Text, FehlerbeschreibungTxt.Text, maxPrice.Text, einMitarbeiterNach, passKundenTxt.Text, graphKey, BemerkungTxt.Text, zustandTxt.Text, InternerVermerkTxt.Text);
                        using (MySqlCommand cmd = new MySqlCommand(comm, cn))
                        {
                            cmd.ExecuteNonQuery();
                          lastID = cmd.LastInsertedId;
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                PrintWindow prntWindow = new PrintWindow(this);
                prntWindow.Show();
                PrintWindowClient prntWindowClient = new PrintWindowClient(this);
                prntWindowClient.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("die Felder ausfüllen \n Erforderliche Felder (Gerat, Sernum, Bemerkung, Pass, Zubehor, Von, \n Name, Vorname und Telefon oder Mobil oder Email)", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
