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
       private static List<MusterClass> musterList = new List<MusterClass>();
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
            InitTextBoxMuster();
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
     
        //не давать вводить в текстбокс до тех пор пока текстбокс предыдущего уровня не будет зполнен
        private void InitTextBoxMuster()
        {
            //для старых мустеров(тут были только textBox'ы)
            //1
            //textMuster1.TextChanged += (s,e)=> {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster1_Copy.IsEnabled = true;
            //    else
            //        textMuster1_Copy.IsEnabled = false;
            //};
            //textMuster1_Copy.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster1_Copy2.IsEnabled = true;
            //    else
            //        textMuster1_Copy2.IsEnabled = false;
            //};
            ////2
            //textMuster2.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster2_Сopy.IsEnabled = true;
            //    else
            //        textMuster2_Сopy.IsEnabled = false;
            //};
            //textMuster2_Сopy.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster2_Сopy2.IsEnabled = true;
            //    else
            //        textMuster2_Сopy2.IsEnabled = false;
            //};
            ////3
            //textMuster3.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster3_Сopy.IsEnabled = true;
            //    else
            //        textMuster3_Сopy.IsEnabled = false;
            //};
            //textMuster3_Сopy.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster3_Сopy2.IsEnabled = true;
            //    else
            //        textMuster3_Сopy2.IsEnabled = false;
            //};
            ////4
            //textMuster4.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster4_Сopy.IsEnabled = true;
            //    else
            //        textMuster4_Сopy.IsEnabled = false;
            //};
            //textMuster4_Сopy.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster4_Сopy2.IsEnabled = true;
            //    else
            //        textMuster4_Сopy2.IsEnabled = false;
            //};
            ////5
            //textMuster5.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster5_Сopy.IsEnabled = true;
            //    else
            //        textMuster5_Сopy.IsEnabled = false;
            //};
            //textMuster5_Сopy.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster5_Сopy2.IsEnabled = true;
            //    else
            //        textMuster5_Сopy2.IsEnabled = false;
            //};
            ////6
            //textMuster6.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster6_Сopy.IsEnabled = true;
            //    else
            //        textMuster6_Сopy.IsEnabled = false;
            //};
            //textMuster6_Сopy.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster6_Сopy2.IsEnabled = true;
            //    else
            //        textMuster6_Сopy2.IsEnabled = false;
            //};
            ////7
            //textMuster7.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster7_Сopy.IsEnabled = true;
            //    else
            //        textMuster7_Сopy.IsEnabled = false;
            //};
            //textMuster7_Сopy.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster7_Сopy2.IsEnabled = true;
            //    else
            //        textMuster7_Сopy2.IsEnabled = false;
            //};
            ////8
            //textMuster8.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster8_Сopy.IsEnabled = true;
            //    else
            //        textMuster8_Сopy.IsEnabled = false;
            //};
            //textMuster8_Сopy.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster8_Сopy2.IsEnabled = true;
            //    else
            //        textMuster8_Сopy2.IsEnabled = false;
            //};
            ////9
            //textMuster9.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster9_Сopy.IsEnabled = true;
            //    else
            //        textMuster9_Сopy.IsEnabled = false;
            //};
            //textMuster9_Сopy.TextChanged += (s, e) => {
            //    TextBox tBx = (TextBox)s;
            //    if (tBx.Text != "")
            //        textMuster9_Сopy2.IsEnabled = true;
            //    else
            //        textMuster9_Сopy2.IsEnabled = false;
            //};
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

        string readSomeKunde(string rec_id,MySqlConnection cn)
        {
            string match = "";
                    string CommandStringEnterText = String.Format("Select * From adressen WHERE REC_ID  LIKE '%{0}%'", rec_id);
                    using (MySqlCommand cmd = new MySqlCommand(CommandStringEnterText, cn))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                
                                match = dr["MATCHCODE"].ToString() + " ";
                                match += dr["NAME1"].ToString() + " ";
                                match += dr["NAME2"].ToString();
                            }
                        }
                    }
            return match;
        }

        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            string musterString = "";
            foreach (MusterClass item in musterList)
            {
                musterString += item.NameNumer;
            }
            if (((ComboBoxItem)mitarbeiterNach.SelectedValue).Content.ToString() != "\t***" && geratTxt.Text != "" && serialNummerTxt.Text != "" && BemerkungTxt.Text != "" && (passKundenTxt.Text != "" || musterString != "") && zubehorTxt.Text != "" && (nameTxt.Text != "" || VornameTxt.Text != "") && (TelefonTxt.Text != "" || MobilTxt.Text != "" || EMailTxt.Text != ""))
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
                       
                       
                        string fehlerBeschreibungStr = MySqlHelper.EscapeString(FehlerbeschreibungTxt.Text); // чтоб можна было писать любые символы.напр. '  
                        string geratStr = MySqlHelper.EscapeString(geratTxt.Text);
                        string zubehorStr = MySqlHelper.EscapeString(zubehorTxt.Text);
                        string bemerkundStr = MySqlHelper.EscapeString(BemerkungTxt.Text);
                        string zustandStr = MySqlHelper.EscapeString(zustandTxt.Text);
                        string internerFermStr = MySqlHelper.EscapeString(InternerVermerkTxt.Text);
                        string kundMatchcode = readSomeKunde(meinKundeID,cn);
                        
                        string comm = string.Format("Insert Into service (dateTime, status, clientID, gerat, serialNummer, zubehor, fehlerBeschreibung, maxPrice, mitarbeiterNach, passKunden, graphKey, bemerkung, zustadn, internVermerk, kundNamVorMatch) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')", dataZeit , "Angenommen von", meinKundeID, geratStr, serialNummerTxt.Text, zubehorStr, fehlerBeschreibungStr, maxPrice.Text, einMitarbeiterNach, passKundenTxt.Text, musterString, bemerkundStr, zustandStr, internerFermStr, kundMatchcode);
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
                string alleFelder = "Bitte füllen Sie folgende Felder aus:";

                if (((ComboBoxItem)mitarbeiterNach.SelectedValue).Content.ToString() == "\t***")
                {
                    alleFelder += "\n Von";
                }
                if (geratTxt.Text == "")
                {
                    alleFelder += "\n Gerät";
                }
                if (serialNummerTxt.Text == "")
                {
                    alleFelder += "\n Seriennr";
                }
                if (BemerkungTxt.Text == "")
                {
                    alleFelder += "\n Bemerkung";
                }

                if (passKundenTxt.Text == "" && musterString == "")
                {
                    alleFelder += "\n Log/Pass";
                }
                if (zubehorTxt.Text == "")
                {
                    alleFelder += "\n Zubehör";
                }
                if (nameTxt.Text == "" && VornameTxt.Text == "")
                {
                    alleFelder += "\n Name und/oder Vorname";
                }
                if (TelefonTxt.Text == "" && MobilTxt.Text == "" && EMailTxt.Text == "")
                {
                    alleFelder += "\n Telefon oder Mobil oder E-Mail";
                }
                MessageBox.Show(alleFelder, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void musBut1_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            switch ((string)but.ToolTip)
            {
                case "1":
                    but.Background = enumColor.First;
                    but.ToolTip = "2";
                    break;
                case "2":
                    but.Background = enumColor.Second;
                    but.ToolTip = "3";
                    break;
                case "3":
                    but.Background = enumColor.Third;
                    but.ToolTip = "4";
                    break;
                case "4":
                    but.Background = enumColor.Fourth;
                    but.ToolTip = "5";
                    break;
                case "5":
                    but.Background = enumColor.Fivth;
                    break;
                default:
                    break;
            }

            musterList.Add(new MusterClass() { NameNumer = but.Content.ToString() }); //, time = DateTime.Now 
        }

        private void musBut2_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            switch ((string)but.ToolTip)
            {
                case "1":
                    but.Background = enumColor.First;
                    but.ToolTip = "2";
                    break;
                case "2":
                    but.Background = enumColor.Second;
                    but.ToolTip = "3";
                    break;
                case "3":
                    but.Background = enumColor.Third;
                    but.ToolTip = "4";
                    break;
                case "4":
                    but.Background = enumColor.Fourth;
                    but.ToolTip = "5";
                    break;
                case "5":
                    but.Background = enumColor.Fivth;
                    break;
                default:
                    break;
            }
            musterList.Add(new MusterClass() { NameNumer = but.Content.ToString() });//, time = DateTime.Now
        }

        private void musBut3_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            switch ((string)but.ToolTip)
            {
                case "1":
                    but.Background = enumColor.First;
                    but.ToolTip = "2";
                    break;
                case "2":
                    but.Background = enumColor.Second;
                    but.ToolTip = "3";
                    break;
                case "3":
                    but.Background = enumColor.Third;
                    but.ToolTip = "4";
                    break;
                case "4":
                    but.Background = enumColor.Fourth;
                    but.ToolTip = "5";
                    break;
                case "5":
                    but.Background = enumColor.Fivth;
                    break;
                default:
                    break;
            }

            musterList.Add(new MusterClass() { NameNumer = but.Content.ToString() });//, time = DateTime.Now
        }

        private void musBut4_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            switch ((string)but.ToolTip)
            {
                case "1":
                    but.Background = enumColor.First;
                    but.ToolTip = "2";
                    break;
                case "2":
                    but.Background = enumColor.Second;
                    but.ToolTip = "3";
                    break;
                case "3":
                    but.Background = enumColor.Third;
                    but.ToolTip = "4";
                    break;
                case "4":
                    but.Background = enumColor.Fourth;
                    but.ToolTip = "5";
                    break;
                case "5":
                    but.Background = enumColor.Fivth;
                    break;
                default:
                    break;
            }
            musterList.Add(new MusterClass() { NameNumer = but.Content.ToString() });//, time = DateTime.Now 
        }

        private void musBut5_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            switch ((string)but.ToolTip)
            {
                case "1":
                    but.Background = enumColor.First;
                    but.ToolTip = "2";
                    break;
                case "2":
                    but.Background = enumColor.Second;
                    but.ToolTip = "3";
                    break;
                case "3":
                    but.Background = enumColor.Third;
                    but.ToolTip = "4";
                    break;
                case "4":
                    but.Background = enumColor.Fourth;
                    but.ToolTip = "5";
                    break;
                case "5":
                    but.Background = enumColor.Fivth;
                    break;
                default:
                    break;
            }
            musterList.Add(new MusterClass() { NameNumer = but.Content.ToString() });//, time = DateTime.Now 
        }

        private void musBut6_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            switch ((string)but.ToolTip)
            {
                case "1":
                    but.Background = enumColor.First;
                    but.ToolTip = "2";
                    break;
                case "2":
                    but.Background = enumColor.Second;
                    but.ToolTip = "3";
                    break;
                case "3":
                    but.Background = enumColor.Third;
                    but.ToolTip = "4";
                    break;
                case "4":
                    but.Background = enumColor.Fourth;
                    but.ToolTip = "5";
                    break;
                case "5":
                    but.Background = enumColor.Fivth;
                    break;
                default:
                    break;
            }
            musterList.Add(new MusterClass() { NameNumer = but.Content.ToString() });//, time = DateTime.Now
        }

        private void musBut7_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            switch ((string)but.ToolTip)
            {
                case "1":
                    but.Background = enumColor.First;
                    but.ToolTip = "2";
                    break;
                case "2":
                    but.Background = enumColor.Second;
                    but.ToolTip = "3";
                    break;
                case "3":
                    but.Background = enumColor.Third;
                    but.ToolTip = "4";
                    break;
                case "4":
                    but.Background = enumColor.Fourth;
                    but.ToolTip = "5";
                    break;
                case "5":
                    but.Background = enumColor.Fivth;
                    break;
                default:
                    break;
            }
            musterList.Add(new MusterClass() { NameNumer = but.Content.ToString() });//, time = DateTime.Now
        }

        private void musBut8_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            switch ((string)but.ToolTip)
            {
                case "1":
                    but.Background = enumColor.First;
                    but.ToolTip = "2";
                    break;
                case "2":
                    but.Background = enumColor.Second;
                    but.ToolTip = "3";
                    break;
                case "3":
                    but.Background = enumColor.Third;
                    but.ToolTip = "4";
                    break;
                case "4":
                    but.Background = enumColor.Fourth;
                    but.ToolTip = "5";
                    break;
                case "5":
                    but.Background = enumColor.Fivth;
                    break;
                default:
                    break;
            }
            musterList.Add(new MusterClass() { NameNumer = but.Content.ToString() });//, time = DateTime.Now
        }

        private void musBut9_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            switch ((string)but.ToolTip)
            {
                case "1":
                    but.Background = enumColor.First;
                    but.ToolTip = "2";
                    break;
                case "2":
                    but.Background = enumColor.Second;
                    but.ToolTip = "3";
                    break;
                case "3":
                    but.Background = enumColor.Third;
                    but.ToolTip = "4";
                    break;
                case "4":
                    but.Background = enumColor.Fourth;
                    but.ToolTip = "5";
                    break;
                case "5":
                    but.Background = enumColor.Fivth;
                    break;
                default:
                    break;
            }
            musterList.Add(new MusterClass() { NameNumer = but.Content.ToString() });//, time = DateTime.Now
        }

        private void delMustButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Muss machen! \nJetzt functioniert es nicht!");
        }
    }
}
