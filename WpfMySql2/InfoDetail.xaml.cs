using System;
using System.Data;
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
using MySql.Data.MySqlClient;
namespace WpfMySql2
{
    /// <summary>
    /// Interaction logic for InfoDetail.xaml
    /// </summary>
    public partial class InfoDetail : Window
    {
        private DataTable tableArtGeid = new DataTable();
        public static DataRowView artikelRow = null;
        public DataTable tableCurrentClient;
        DataRow row = null;
        string id = MainWindow.rowDetail["id"].ToString();//пробл. при дабл клик на пустой строке
        public InfoDetail()
        {
            InitializeComponent();
            this.Title = "Reparatur Status Nr.: " + id;
            this.Closing += InfoDetail_Closing;
            initTable();
            InitData();
        }

        private void StatusCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comb = sender as ComboBox;
            ComboBoxItem combItem = comb.SelectedItem as ComboBoxItem;
            if(combItem.Content.ToString() == "Abgeholt/Abgeschlossen")
            {
                MessageBox.Show("statusCbx changed!");
            }
        }

        private void initTable()
        {
            DataColumn column = new DataColumn("REC_ID", typeof(string));
            tableArtGeid.Columns.Add(column);
            column = new DataColumn("Suchbegriff", typeof(string));
            tableArtGeid.Columns.Add(column);
            column = new DataColumn("Arikelnummer", typeof(string));
            tableArtGeid.Columns.Add(column);
            column = new DataColumn("Kurzname", typeof(string));
            tableArtGeid.Columns.Add(column);
            column = new DataColumn("VK-Preis 5N", typeof(string));
            tableArtGeid.Columns.Add(column);
        }
        private void InfoDetail_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("speichern Sie die Änderungen?", "Schließen", MessageBoxButton.YesNo))
            {
                btnOK_Click(null, null);
            }
        }

        void readSomeTable()
        {
            DataTable table = new DataTable();
            DataColumn column = new DataColumn("REC_ID", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("MATCHCODE", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("NAME1", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("NAME2", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("STRASSE", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("TELE1", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("TELE2", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("KUNDENGRUPPE", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("PLZ", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("ORT", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("EMAIL", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("ANREDE", typeof(string));
            table.Columns.Add(column);

            using (MySqlConnection cn = new MySqlConnection())
            {
                cn.ConnectionString = App.GetConnection();
                try
                {
                    cn.Open();
                    string CommandStringEnterText = String.Format("Select * From adressen WHERE REC_ID  LIKE '%{0}%'", MainWindow.rowDetail["clientID"]);
                    using (MySqlCommand cmd = new MySqlCommand(CommandStringEnterText, cn))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            // table.Load(dr);
                            while (dr.Read())
                            {
                                row = table.NewRow();
                                row["REC_ID"] = dr["REC_ID"].ToString();
                                row["MATCHCODE"] = dr["MATCHCODE"].ToString();
                                row["NAME1"] = dr["NAME1"].ToString();
                                row["NAME2"] = dr["NAME2"].ToString();
                                row["STRASSE"] = dr["STRASSE"].ToString();
                                row["TELE1"] = dr["TELE1"].ToString();
                                row["TELE2"] = dr["TELE2"].ToString();
                                row["KUNDENGRUPPE"] = dr["KUNDENGRUPPE"].ToString();
                                row["PLZ"] = dr["PLZ"].ToString();
                                row["ORT"] = dr["ORT"].ToString();
                                row["EMAIL"] = dr["EMAIL"].ToString();
                                row["ANREDE"] = dr["ANREDE"].ToString();
                                table.Rows.Add(row);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void InitData()
        {
            if (MainWindow.rowDetail != null)
            {

                readSomeTable();
                InitDataGridArtikel();
                kundenNrtxt.Text = MainWindow.rowDetail["clientID"].ToString();
                anredeTxt.Text = row["ANREDE"].ToString();
                nameTxt.Text = row["NAME1"].ToString();
                VornameTxt.Text = row["NAME2"].ToString();
                StrasseTxt.Text = row["STRASSE"].ToString();
                PLZTxt.Text = row["PLZ"].ToString();
                OrtTxt.Text = row["ORT"].ToString();
                TelefonTxt.Text = row["TELE1"].ToString();
                MobilTxt.Text = row["TELE2"].ToString();
                EMailTxt.Text = row["EMAIL"].ToString();
                geratTxt.Text = MainWindow.rowDetail["gerat"].ToString();
                serialNummerTxt.Text = MainWindow.rowDetail["serialNummer"].ToString();
                BemerkungTxt.Text = MainWindow.rowDetail["bemerkung"].ToString();
                passKundenTxt.Text = MainWindow.rowDetail["passKunden"].ToString();
                zubehorTxt.Text = MainWindow.rowDetail["zubehor"].ToString();
                maxPrice.Text = MainWindow.rowDetail["maxPrice"].ToString();
                zustandTxt.Text = MainWindow.rowDetail["zustadn"].ToString();
                /////////////////////////////////////
                ComboBoxItem cbItm = new ComboBoxItem();
                cbItm.IsSelected = true;
                cbItm.Content = MainWindow.rowDetail["status"];
                statusCbx.Items.Add(cbItm);
                foreach (string item in MainWindow.statuses)
                {
                    if (item != MainWindow.rowDetail["status"].ToString())
                    {
                        cbItm = new ComboBoxItem();
                        cbItm.Content = item;
                        //cbItm.Background = new SolidColorBrush(Color.FromArgb(100,215,216,178));
                        //cbItm.Background = Brushes.LightSteelBlue;
                        statusCbx.Items.Add(cbItm);
                    }
                }
                statusCbx.SelectionChanged += StatusCbx_SelectionChanged;//подпись на событие именно здесь очень важна!потому что при инициализации окна тоже оно срабатывает если этот статус уже был установлен до этого
                ////////////////////////////////////////
                ComboBoxItem cbItm2 = new ComboBoxItem();
                cbItm2.IsSelected = true;
                if (MainWindow.rowDetail["mitarbeiterAus"].ToString() == "")
                {
                    cbItm2.Content = "\t***";
                }
                else
                {
                    cbItm2.Content = MainWindow.rowDetail["mitarbeiterAus"].ToString();
                }
                mitarbeiterAus.Items.Add(cbItm2);
                foreach (string item in MainWindow.mitarbeiter)
                {
                    if (item != MainWindow.rowDetail["mitarbeiterAus"].ToString())
                    {
                        cbItm2 = new ComboBoxItem();
                        cbItm2.Content = item;
                        //cbItm2.Background = Brushes.LightSteelBlue;
                        mitarbeiterAus.Items.Add(cbItm2);
                    }
                }
                ///////////////////////////////////
                FehlerbeschreibungTxt.Text = MainWindow.rowDetail["fehlerBeschreibung"].ToString();
                InternerVermerkTxt.Text = MainWindow.rowDetail["internVermerk"].ToString();
                reparBericht.Text = MainWindow.rowDetail["bereicht"].ToString();
                listBoxStatus.Items.Add(("Data/Zeit:\t\t" + MainWindow.rowDetail["dateTime"]));
                listBoxStatus.Items.Add(("Reparatur Nr.:\t\t" + MainWindow.rowDetail["id"].ToString()));
                listBoxStatus.Items.Add(("Von:\t\t\t" + MainWindow.rowDetail["mitarbeiterNach"].ToString()));
                readNeuList();
                string graphKey = MainWindow.rowDetail["graphKey"].ToString();
                textMuster1.Text = graphKey.Substring(0, 1);
                textMuster2.Text = graphKey.Substring(1, 1);
                textMuster3.Text = graphKey.Substring(2, 1);
                textMuster4.Text = graphKey.Substring(3, 1);
                textMuster5.Text = graphKey.Substring(4, 1);
                textMuster6.Text = graphKey.Substring(5, 1);
                textMuster7.Text = graphKey.Substring(6, 1);
                textMuster8.Text = graphKey.Substring(7, 1);
                textMuster9.Text = graphKey.Substring(8, 1);
                MainWindow.rowDetail = null;
            }
        }

        public void InitDataGridArtikel()
        {
            string idArtikel = "";
            using (MySqlConnection cn = new MySqlConnection())
            {
                cn.ConnectionString = App.GetConnection();
                try
                {
                    cn.Open();
                    string CommandStringEnterText = String.Format("Select * From service WHERE id  LIKE '%{0}%'", MainWindow.rowDetail["id"]);
                    using (MySqlCommand cmd = new MySqlCommand(CommandStringEnterText, cn))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            // table.Load(dr);
                            while (dr.Read())
                            {
                                idArtikel = dr["artikelRecId"].ToString();
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (idArtikel != "")
            {
                char[] ar = new char[] { '*' };
                string[] allIdArtikel = idArtikel.Split(ar);
                //DataTable tablei = new DataTable();
                //DataColumn column = new DataColumn("REC_ID", typeof(string));
                //tablei.Columns.Add(column);
                //column = new DataColumn("Suchbegriff", typeof(string));
                //tablei.Columns.Add(column);
                //column = new DataColumn("Arikelnummer", typeof(string));
                //tablei.Columns.Add(column);
                //column = new DataColumn("Kurzname", typeof(string));
                //tablei.Columns.Add(column);
                //column = new DataColumn("VK-Preis 5N", typeof(string));
                //tablei.Columns.Add(column);

                using (MySqlConnection cn = new MySqlConnection())
                {
                    cn.ConnectionString = App.GetConnection();
                    try
                    {
                        cn.Open();
                        foreach (string art in allIdArtikel)
                        {
                            if (art != "")
                            {
                                string CommandStringEnterText = String.Format("Select * From artikel WHERE REC_ID  LIKE '{0}'", art);
                                using (MySqlCommand cmd = new MySqlCommand(CommandStringEnterText, cn))
                                {
                                    using (MySqlDataReader dr = cmd.ExecuteReader())
                                    {
                                        while (dr.Read())
                                        {
                                            DataRow row = tableArtGeid.NewRow();
                                            row["REC_ID"] = dr["REC_ID"].ToString();
                                            row["Suchbegriff"] = dr["MATCHCODE"].ToString();
                                            row["Arikelnummer"] = dr["ARTNUM"].ToString();
                                            row["Kurzname"] = dr["KURZNAME"].ToString();
                                            row["VK-Preis 5N"] = dr["VK5"].ToString();
                                            tableArtGeid.Rows.Add(row);
                                        }
                                    }
                                }
                            }
                        }

                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                this.DataGridArtikel.ItemsSource = tableArtGeid.AsDataView();
            }
        }

        void readNeuList()
        {
            if (MainWindow.rowDetail["listBoxAdd"].ToString() != "")
            {
                string allNeuData = MainWindow.rowDetail["listBoxAdd"].ToString();
                string[] aRallNeuData = allNeuData.Split(new char[] { '*' });
                foreach (string str in aRallNeuData)
                {
                    if (str != "")//кажется ф-ция split добавляет "" вот такую строку...
                    {
                        listBoxStatus.Items.Add(str);
                    }
                }
            }
        }
        private void ButtonForEsc_Click(object sender, RoutedEventArgs e)
        {
            Close();//добавить вопрос о сохранении
        }

        //private void buttonPrint_Click(object sender, RoutedEventArgs e)
        //{
        //    if (this.textBlockDate.Text != "Date")//защита от дабл клик на пустом дата грид (тоесть нету сток) 
        //    {
        //        using (MySqlConnection cn = new MySqlConnection())
        //        {
        //            cn.ConnectionString = App.GetConnection();
        //            try
        //            {
        //                cn.Open();
        //                ComboBoxItem cmbx = (ComboBoxItem)comboBoxStatus.SelectedValue;
        //                string status = (String)cmbx.Content;
        //                ComboBoxItem cmbx2 = (ComboBoxItem)comboBoxEmployeeOut.SelectedValue;
        //                string nameEmp = (String)cmbx2.Content;
        //                string comm = string.Format("Update service Set  status = '{0}', other = '{1}', report = '{2}', material = '{3}', spare_parts = '{4}', work_time = '{5}', employeeOut = '{6}' Where id = '{7}'", status, this.textBoxOther.Text, this.textBoxReport.Text, textBoxMaterial.Text, textBoxSpareParts.Text, textBoxWorkTime.Text, nameEmp, this.textBlockId.Text);
        //                using (MySqlCommand cmd = new MySqlCommand(comm, cn))// незабыть разобрать в имени базы данных
        //                {
        //                    cmd.ExecuteNonQuery();
        //                }
        //            }
        //            catch (MySqlException ex)
        //            {
        //                MessageBox.Show(ex.Message);
        //            }
        //        }
        //    }
        //    //PrintWindow prntWindow = new PrintWindow(this);
        //    //prntWindow.ShowDialog();
        //}

        //private void buttonPrintKunde_Click(object sender, RoutedEventArgs e)
        //{
        //    if (this.textBlockDate.Text != "Date")//защита от дабл клик на пустом дата грид (тоесть нету сток) 
        //    {
        //        using (MySqlConnection cn = new MySqlConnection())
        //        {
        //            cn.ConnectionString = App.GetConnection();
        //            try
        //            {
        //                cn.Open();
        //                ComboBoxItem cmbx = (ComboBoxItem)comboBoxStatus.SelectedValue;
        //                string status = (String)cmbx.Content;
        //                ComboBoxItem cmbx2 = (ComboBoxItem)comboBoxEmployeeOut.SelectedValue;
        //                string nameEmp = (String)cmbx2.Content;
        //                string comm = string.Format("Update service Set  status = '{0}', other = '{1}', report = '{2}', material = '{3}', spare_parts = '{4}', work_time = '{5}', employeeOut = '{6}' Where id = '{7}'", status, this.textBoxOther.Text, this.textBoxReport.Text, textBoxMaterial.Text, textBoxSpareParts.Text, textBoxWorkTime.Text, nameEmp, this.textBlockId.Text);
        //                using (MySqlCommand cmd = new MySqlCommand(comm, cn))// незабыть разобрать в имени базы данных
        //                {
        //                    cmd.ExecuteNonQuery();
        //                }
        //            }
        //            catch (MySqlException ex)
        //            {
        //                MessageBox.Show(ex.Message);
        //            }
        //        }
        //    }
        //    //PrintWindowClient prntWindowClient = new PrintWindowClient(this);
        //    //prntWindowClient.ShowDialog();
        //}

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection cn = new MySqlConnection())
            {
                cn.ConnectionString = App.GetConnection();
                try
                {
                    cn.Open();
                    ComboBoxItem cmbx = (ComboBoxItem)statusCbx.SelectedValue;
                    string status = (String)cmbx.Content;
                    ComboBoxItem cmbx2 = (ComboBoxItem)mitarbeiterAus.SelectedValue;
                    string mitarbAus = (String)cmbx2.Content;
                    string internerVermStr = MySqlHelper.EscapeString(InternerVermerkTxt.Text);
                    string reparBetichtStr = MySqlHelper.EscapeString(reparBericht.Text);
                    string allIdArtikel = "";
                    foreach (DataRow row in tableArtGeid.Rows)
                    {
                        allIdArtikel += row["REC_ID"].ToString() + "*";
                    }
                    string comm = string.Format("Update service Set  status = '{0}', mitarbeiterAus = '{1}', internVermerk = '{2}', bereicht = '{3}', artikelRecId = '{4}' Where id = '{5}'", status, mitarbAus, internerVermStr, reparBetichtStr, allIdArtikel, this.id);
                    using (MySqlCommand cmd = new MySqlCommand(comm, cn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            this.Closing -= InfoDetail_Closing;//долго сидел ночью думал почему так. Убрирать нужно для того чтобы при нажатии на ОК не спрашивало нужно ли сохранять, потому что если на ОК нажал то сохранять по любому нужно

            if (sender != null)// эта функция вызывается в момент когда уже происходит завершение существования окна. поэтому повторный вызов закрытия дает исклю.сит.
                this.Close();//тоесть нужно вызывать клозе ТОЛЬКо если наша функция вызвалась непосредственно, а не как часть процесса закрытия окна.это происходит потому что эту функцию удобно использовать как функцию для сохранения всех данных окна
        }

        private void btnAddListBox_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxData.Text != "" && textBoxHeader.Text != "")
            {
                string oldAddList = "";

                using (MySqlConnection cn = new MySqlConnection())
                {
                    cn.ConnectionString = App.GetConnection();
                    try
                    {
                        cn.Open();
                        string CommandStringEnterText = String.Format("Select listBoxAdd From service WHERE id LIKE '%{0}%'", id);
                        using (MySqlCommand cmd = new MySqlCommand(CommandStringEnterText, cn))
                        {
                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    oldAddList = dr["listBoxAdd"].ToString();
                                }
                            }
                        }
                        string addList = (textBoxHeader.Text + "\t\t\t" + textBoxData.Text);
                        string comm = string.Format("Update service Set  listBoxAdd = '{0}' Where id = '{1}'", (oldAddList + addList + "*"), this.id);
                        using (MySqlCommand cmd = new MySqlCommand(comm, cn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        listBoxStatus.Items.Add(addList);
                        textBoxHeader.Text = "";
                        textBoxData.Text = "";
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btnAddArtikel_Click(object sender, RoutedEventArgs e)
        {
            Artikel art = new Artikel();
            art.ShowDialog();
            if (InfoDetail.artikelRow != null)
            {
                DataRow row = tableArtGeid.NewRow();
                row["REC_ID"] = artikelRow["REC_ID"];
                row["Suchbegriff"] = artikelRow["Suchbegriff"];
                row["Arikelnummer"] = artikelRow["Arikelnummer"];
                row["Kurzname"] = artikelRow["Kurzname"];
                row["VK-Preis 5N"] = artikelRow["VK-Preis 5N"];
                tableArtGeid.Rows.Add(row);
                DataGridArtikel.ItemsSource = tableArtGeid.AsDataView();

                InfoDetail.artikelRow = null;
            }
        }

        private void btnDelArtikel_Click(object sender, RoutedEventArgs e)
        {
            DataRow rowToDel = ((DataRowView)DataGridArtikel.SelectedItem).Row;
            if(rowToDel != null)
            {
                string id = rowToDel["REC_ID"].ToString();
                
                foreach (DataRow rowDel in tableArtGeid.Rows)
                {
                    if(rowDel["REC_ID"].ToString() == id)
                    {
                        tableArtGeid.Rows.Remove(rowDel);
                        break;
                    }
                }
                DataGridArtikel.ItemsSource = tableArtGeid.AsDataView();
            }
        }
    }
}
