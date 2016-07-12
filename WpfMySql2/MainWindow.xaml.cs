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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Collections.ObjectModel;
namespace WpfMySql2
{
    //
    //
    //печать
    //скомпилить не дебаг проект
    //узнать как его установить(что лежит в папке, что встроится в ехе шник)
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DataRowView rowDetail = null;
        public static DataRowView kundenID = null;
        public static List<string> mitarbeiter = null;
        public static Dictionary<string, string> kundenGruppe = new Dictionary<string, string> { { "0", "alle Adressen" }, { "1", "Privat" }, { "2", "Geweblich" }, { "3", "Offentlich" }, { "4", "Altbestand" }, { "999", "Lieferanten" } };
        public static List<string> AnredeList = new List<string>() { "Bürgermeister", "Doktor", "Familie", "Firma", "Frau", "Herr", "Pfarrer", "Professor" };
        public static List<string> statuses = new List<string>() { "Angenommen von", "In Bearbeitung", "Warten auf Ersatzteile", "Gerät ist beim Hersteller", "Warten auf Kundenrückmeldung", "Fertig zur Abholung", "Abgeholt/Abgeschlossen" };
        public static bool isRun = false;
        public static bool cancel = false;
        public MainWindow()
        {
            InitializeComponent();

            //при отказе, конект типа "прошёл" но на самом деле его нету
            // и это не проблемма потому что код все равно исполнен не будет изза переменной изРан

            while (chekConnection() == false)
            { }

            if (isRun == true)
            {
                LoadEmployee();
                CreateNewTable();
                readAllTableService();
                dataGridMain.MouseDoubleClick += DataGridMain_MouseDoubleClick;
                textBoxFilter.TextChanged += TextBoxFilter_TextChanged;
                dataGridMain.AutoGeneratingColumn += DataGridMain_AutoGeneratingColumn;
                initSuchFilter();
            }
        }
        private void initSuchFilter()
        {
            ComboBoxItem cbItm = new ComboBoxItem();
            cbItm.IsSelected = true;
            cbItm.Content = "\t***";
            comboBoxStatusFilter.Items.Add(cbItm);
            foreach (string item in MainWindow.statuses)
            {
                cbItm = new ComboBoxItem();
                cbItm.Content = item;
                comboBoxStatusFilter.Items.Add(cbItm);
            }
            comboBoxStatusFilter.SelectionChanged += ComboBoxStatusFilter_SelectionChanged;
        }

        private void ComboBoxStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cbItm = ((sender as ComboBox).SelectedItem as ComboBoxItem);
            readPartTableSuchFilter(cbItm.Content.ToString());
        }

        private bool chekConnection()
        {
            using (MySqlConnection cn = new MySqlConnection())
            {
                cn.ConnectionString = App.GetConnection();
                try
                {
                    cn.Open();
                }
                catch (MySqlException ex)
                {
                    Setting_DB dbSet = new Setting_DB();
                    dbSet.ShowDialog();
                    //MessageBox.Show("Bitte führen Sie die Anwendung erneut");
                    if (cancel)
                    {
                        Close();
                        return true;
                    }
                    else
                        return false;
                }

                isRun = true;

                return true;
            }
        }
        public void LoadEmployee()
        {
            if (mitarbeiter == null)
            {
                mitarbeiter = new List<string>();
                DataTable table = new DataTable();
                DataColumn column = new DataColumn("NAME", typeof(string));
                table.Columns.Add(column);
                column = new DataColumn("VNAME", typeof(string));
                table.Columns.Add(column);

                using (MySqlConnection cn = new MySqlConnection())
                {
                    cn.ConnectionString = App.GetConnection();
                    try
                    {
                        cn.Open();
                        using (MySqlCommand cmd = new MySqlCommand("Select * From mitarbeiter", cn))
                        {
                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    DataRow row = table.NewRow();
                                    row["NAME"] = dr["NAME"].ToString();
                                    row["VNAME"] = dr["VNAME"].ToString();
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
                foreach (DataRow row in table.Rows)
                {
                    mitarbeiter.Add(row["NAME"].ToString() + " " + row["VNAME"].ToString());
                }
            }
        }
        private void readPartTableSuchFilter(string status)
        {
            if (status == "\t***")
            {
                readAllTableService();
            }
            else
            {
                DataTable table = new DataTable();
                DataColumn column = new DataColumn("id", typeof(string));
                column.Caption = "Nr.";
                table.Columns.Add(column);

                column = new DataColumn("dateTime", typeof(string));
                column.Caption = "Datum";
                table.Columns.Add(column);

                column = new DataColumn("status", typeof(string));
                column.Caption = "Status";
                table.Columns.Add(column);

                column = new DataColumn("clientID", typeof(string));
                column.Caption = "Kundennr.";
                table.Columns.Add(column);

                column = new DataColumn("kundNamVorMatch", typeof(string));
                column.Caption = "Kunden matchcode";
                table.Columns.Add(column);

                column = new DataColumn("gerat", typeof(string));
                table.Columns.Add(column);
                column.Caption = "Gerät";

                column = new DataColumn("serialNummer", typeof(string));
                table.Columns.Add(column);
                column.Caption = "Seriennummer";

                //column = new DataColumn("zubehor", typeof(string));
                //table.Columns.Add(column);

                //column = new DataColumn("fehlerBeschreibung", typeof(string));
                //table.Columns.Add(column);

                //column = new DataColumn("maxPrice", typeof(string));
                //table.Columns.Add(column);

                column = new DataColumn("mitarbeiterNach", typeof(string));
                column.Caption = "angenomen von";
                table.Columns.Add(column);

                //column = new DataColumn("mitarbeiterAus", typeof(string));
                //table.Columns.Add(column);

                //column = new DataColumn("passKunden", typeof(string));
                //table.Columns.Add(column);

                //column = new DataColumn("graphKey", typeof(string));
                //table.Columns.Add(column);

                column = new DataColumn("bemerkung", typeof(string));
                column.Caption = "Bemerkung";
                table.Columns.Add(column);

                //column = new DataColumn("zustadn", typeof(string));
                //table.Columns.Add(column);

                //column = new DataColumn("bereicht", typeof(string));
                //table.Columns.Add(column);

                //column = new DataColumn("internVermerk", typeof(string));
                //table.Columns.Add(column);

                using (MySqlConnection cn = new MySqlConnection())
                {
                    cn.ConnectionString = App.GetConnection();
                    try
                    {
                        cn.Open();
                        string CommandStringEnterText = String.Format("Select * From service WHERE status LIKE '%{0}%'", status);
                        using (MySqlCommand cmd = new MySqlCommand(CommandStringEnterText, cn))
                        {
                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                // table.Load(dr);
                                while (dr.Read())
                                {
                                    DataRow row = table.NewRow();
                                    row["id"] = dr["id"].ToString();
                                    row["dateTime"] = dr["dateTime"].ToString();
                                    row["status"] = dr["status"].ToString();
                                    row["clientID"] = dr["clientID"].ToString();
                                    row["kundNamVorMatch"] = dr["kundNamVorMatch"].ToString();
                                    row["gerat"] = dr["gerat"].ToString();
                                    row["serialNummer"] = dr["serialNummer"].ToString();
                                    //row["zubehor"] = dr["zubehor"].ToString();
                                    //row["fehlerBeschreibung"] = dr["fehlerBeschreibung"].ToString();
                                    // row["maxPrice"] = dr["maxPrice"].ToString();
                                    row["mitarbeiterNach"] = dr["mitarbeiterNach"].ToString();
                                    //row["mitarbeiterAus"] = dr["mitarbeiterAus"].ToString();
                                    //row["passKunden"] = dr["passKunden"].ToString();
                                    //row["graphKey"] = dr["graphKey"].ToString();
                                    row["bemerkung"] = dr["bemerkung"].ToString();
                                    //row["zustadn"] = dr["zustadn"].ToString();
                                    //row["bereicht"] = dr["bereicht"].ToString();
                                    //row["internVermerk"] = dr["internVermerk"].ToString();
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
                auftragQualitat(table);
                this.dataGridMain.ItemsSource = table.AsDataView();
            }
        }
        private void TextBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            readPartTable();
            //initSuchFilter();//разобратся
        }
        private void readPartTable()
        {
            DataTable table = new DataTable();
            DataColumn column = new DataColumn("id", typeof(string));
            column.Caption = "Nr.";
            table.Columns.Add(column);

            column = new DataColumn("dateTime", typeof(string));
            column.Caption = "Datum";
            table.Columns.Add(column);

            column = new DataColumn("status", typeof(string));
            column.Caption = "Status";
            table.Columns.Add(column);

            column = new DataColumn("clientID", typeof(string));
            column.Caption = "Kundennr.";
            table.Columns.Add(column);

            column = new DataColumn("kundNamVorMatch", typeof(string));
            column.Caption = "Kunden matchcode";
            table.Columns.Add(column);

            column = new DataColumn("gerat", typeof(string));
            table.Columns.Add(column);
            column.Caption = "Gerät";

            column = new DataColumn("serialNummer", typeof(string));
            table.Columns.Add(column);
            column.Caption = "Seriennummer";

            //column = new DataColumn("zubehor", typeof(string));
            //table.Columns.Add(column);

            //column = new DataColumn("fehlerBeschreibung", typeof(string));
            //table.Columns.Add(column);

            //column = new DataColumn("maxPrice", typeof(string));
            //table.Columns.Add(column);

            column = new DataColumn("mitarbeiterNach", typeof(string));
            column.Caption = "angenomen von";
            table.Columns.Add(column);

            //column = new DataColumn("mitarbeiterAus", typeof(string));
            //table.Columns.Add(column);

            //column = new DataColumn("passKunden", typeof(string));
            //table.Columns.Add(column);

            //column = new DataColumn("graphKey", typeof(string));
            //table.Columns.Add(column);

            column = new DataColumn("bemerkung", typeof(string));
            column.Caption = "Bemerkung";
            table.Columns.Add(column);

            //column = new DataColumn("zustadn", typeof(string));
            //table.Columns.Add(column);

            //column = new DataColumn("bereicht", typeof(string));
            //table.Columns.Add(column);

            //column = new DataColumn("internVermerk", typeof(string));
            //table.Columns.Add(column);
            using (MySqlConnection cn = new MySqlConnection())
            {
                cn.ConnectionString = App.GetConnection();
                try
                {
                    cn.Open();
                    string CommandStringEnterText = String.Format("Select * From service WHERE kundNamVorMatch LIKE '%{0}%'", this.textBoxFilter.Text);
                    using (MySqlCommand cmd = new MySqlCommand(CommandStringEnterText, cn))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            // table.Load(dr);
                            while (dr.Read())
                            {
                                DataRow row = table.NewRow();
                                row["id"] = dr["id"].ToString();
                                row["dateTime"] = dr["dateTime"].ToString();
                                row["status"] = dr["status"].ToString();
                                row["clientID"] = dr["clientID"].ToString();
                                row["kundNamVorMatch"] = dr["kundNamVorMatch"].ToString();

                                row["gerat"] = dr["gerat"].ToString();
                                row["serialNummer"] = dr["serialNummer"].ToString();
                                //row["zubehor"] = dr["zubehor"].ToString();
                                //row["fehlerBeschreibung"] = dr["fehlerBeschreibung"].ToString();
                                // row["maxPrice"] = dr["maxPrice"].ToString();
                                row["mitarbeiterNach"] = dr["mitarbeiterNach"].ToString();
                                //row["mitarbeiterAus"] = dr["mitarbeiterAus"].ToString();
                                //row["passKunden"] = dr["passKunden"].ToString();
                                //row["graphKey"] = dr["graphKey"].ToString();
                                row["bemerkung"] = dr["bemerkung"].ToString();
                                //row["zustadn"] = dr["zustadn"].ToString();
                                //row["bereicht"] = dr["bereicht"].ToString();
                                //row["internVermerk"] = dr["internVermerk"].ToString();
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
            auftragQualitat(table);
            this.dataGridMain.ItemsSource = table.AsDataView();
        }
        private void readAllOneRow(string id)
        {
            DataTable table = new DataTable();
            DataColumn column = new DataColumn("id", typeof(string));
            column.Caption = "Nr.";
            table.Columns.Add(column);

            column = new DataColumn("dateTime", typeof(string));
            column.Caption = "Datum";
            table.Columns.Add(column);

            column = new DataColumn("status", typeof(string));
            column.Caption = "Status";
            table.Columns.Add(column);

            column = new DataColumn("clientID", typeof(string));
            column.Caption = "Kundennr.";
            table.Columns.Add(column);

            column = new DataColumn("kundNamVorMatch", typeof(string));
            column.Caption = "Kunden matchcode";
            table.Columns.Add(column);

            column = new DataColumn("gerat", typeof(string));
            table.Columns.Add(column);
            column.Caption = "Gerät";

            column = new DataColumn("serialNummer", typeof(string));
            table.Columns.Add(column);
            column.Caption = "Seriennummer";

            column = new DataColumn("zubehor", typeof(string));
            table.Columns.Add(column);

            column = new DataColumn("fehlerBeschreibung", typeof(string));
            table.Columns.Add(column);

            column = new DataColumn("maxPrice", typeof(string));
            table.Columns.Add(column);

            column = new DataColumn("mitarbeiterNach", typeof(string));
            column.Caption = "angenomen von";
            table.Columns.Add(column);

            column = new DataColumn("mitarbeiterAus", typeof(string));
            table.Columns.Add(column);

            column = new DataColumn("passKunden", typeof(string));
            table.Columns.Add(column);

            column = new DataColumn("graphKey", typeof(string));
            table.Columns.Add(column);

            column = new DataColumn("bemerkung", typeof(string));
            column.Caption = "Bemerkung";
            table.Columns.Add(column);

            column = new DataColumn("zustadn", typeof(string));
            table.Columns.Add(column);

            column = new DataColumn("bereicht", typeof(string));
            table.Columns.Add(column);

            column = new DataColumn("internVermerk", typeof(string));
            table.Columns.Add(column);
            using (MySqlConnection cn = new MySqlConnection())
            {
                cn.ConnectionString = App.GetConnection();
                try
                {
                    cn.Open();
                    string CommandStringEnterText = String.Format("Select * From service WHERE id LIKE '%{0}%'", id);
                    using (MySqlCommand cmd = new MySqlCommand(CommandStringEnterText, cn))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            // table.Load(dr);
                            while (dr.Read())
                            {
                                DataRow row = table.NewRow();
                                row["id"] = dr["id"].ToString();
                                row["dateTime"] = dr["dateTime"].ToString();
                                row["status"] = dr["status"].ToString();
                                row["clientID"] = dr["clientID"].ToString();
                                row["kundNamVorMatch"] = dr["kundNamVorMatch"].ToString();

                                row["gerat"] = dr["gerat"].ToString();
                                row["serialNummer"] = dr["serialNummer"].ToString();
                                row["zubehor"] = dr["zubehor"].ToString();
                                row["fehlerBeschreibung"] = dr["fehlerBeschreibung"].ToString();
                                row["maxPrice"] = dr["maxPrice"].ToString();
                                row["mitarbeiterNach"] = dr["mitarbeiterNach"].ToString();
                                row["mitarbeiterAus"] = dr["mitarbeiterAus"].ToString();
                                row["passKunden"] = dr["passKunden"].ToString();
                                row["graphKey"] = dr["graphKey"].ToString();
                                row["bemerkung"] = dr["bemerkung"].ToString();
                                row["zustadn"] = dr["zustadn"].ToString();
                                row["bereicht"] = dr["bereicht"].ToString();
                                row["internVermerk"] = dr["internVermerk"].ToString();
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
           DataRow row2 = (table.Rows[0]);
            rowDetail = table.DefaultView[table.Rows.IndexOf(row2)]; // преобразование DataRow в DataRowView http://stackoverflow.com/questions/15632036/how-can-convert-datarow-to-datarowview-in-c-sharp
        }
        private void DataGridMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid)
            {
                DataGrid mainGrid = (DataGrid)sender;
                rowDetail = (DataRowView)mainGrid.SelectedItem;
                if (rowDetail != null)//защита от дабл клик на пустой строке датагрида. Если пустой строки нету, то дабл клик по полосе прокрутки ведет к тому же.
                {
                    readAllOneRow(rowDetail["id"].ToString());//внутри будет переинициализирован rowDetail
                    InfoDetail detail = new InfoDetail();
                    detail.ShowDialog();
                    readAllTableService();
                    rowDetail = null;
                }
            }


        }

        private void CreateNewTable()
        {

            List<String> allTableNames = new List<string>();

            using (MySqlConnection cn = new MySqlConnection())
            {
                cn.ConnectionString = App.GetConnection();
                try
                {
                    cn.Open();
                    //получаем все таблицы из БД
                    DataTable dataTable = cn.GetSchema("Tables");
                    foreach (DataRow row in dataTable.Rows)
                    {
                        allTableNames.Add((string)row[2]);
                    }
                }
                catch (MySqlException ex)
                {
                    //MessageBox.Show(ex.Message);                    
                    return;
                }
            }
            bool exist = false;
            //chek if exist table
            foreach (String name in allTableNames)
            {
                if (name == "service")
                    exist = true;
            }

            //create new table if not exist
            if (exist == false)
            {
                using (MySqlConnection cn = new MySqlConnection())
                {
                    cn.ConnectionString = App.GetConnection();
                    try
                    {
                        cn.Open();
                        using (MySqlCommand cmd = new MySqlCommand("create table service (id INT AUTO_INCREMENT PRIMARY KEY, dateTime VARCHAR(50),status VARCHAR(50), clientID VARCHAR(10), gerat VARCHAR(200), serialNummer VARCHAR(50), zubehor VARCHAR(250), fehlerBeschreibung VARCHAR(250), maxPrice VARCHAR(10), mitarbeiterNach VARCHAR(50), mitarbeiterAus VARCHAR(50), passKunden VARCHAR(30), graphKey VARCHAR(30), bemerkung VARCHAR(250), zustadn VARCHAR(200),  bereicht VARCHAR(500), internVermerk VARCHAR(500), kundNamVorMatch VARCHAR(150))", cn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (MySqlException ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                    MessageBox.Show("Tabble \"service\" was created");
                }
            }
        }

        private void readAllTableService()
        {
            DataTable table = new DataTable();
            DataColumn column = new DataColumn("id", typeof(string));
            column.Caption = "Nr.";
            table.Columns.Add(column);

            column = new DataColumn("dateTime", typeof(string));
            column.Caption = "Datum";
            table.Columns.Add(column);

            column = new DataColumn("status", typeof(string));
            column.Caption = "Status";
            table.Columns.Add(column);

            column = new DataColumn("clientID", typeof(string));
            column.Caption = "Kundennr.";
            table.Columns.Add(column);

            column = new DataColumn("kundNamVorMatch", typeof(string));
            column.Caption = "Kunden matchcode";
            table.Columns.Add(column);

            column = new DataColumn("gerat", typeof(string));
            table.Columns.Add(column);
            column.Caption = "Gerät";

            column = new DataColumn("serialNummer", typeof(string));
            table.Columns.Add(column);
            column.Caption = "Seriennummer";

            //column = new DataColumn("zubehor", typeof(string));
            //table.Columns.Add(column);

            //column = new DataColumn("fehlerBeschreibung", typeof(string));
            //table.Columns.Add(column);

            //column = new DataColumn("maxPrice", typeof(string));
            //table.Columns.Add(column);

            column = new DataColumn("mitarbeiterNach", typeof(string));
            column.Caption = "angenomen von";
            table.Columns.Add(column);

            //column = new DataColumn("mitarbeiterAus", typeof(string));
            //table.Columns.Add(column);

            //column = new DataColumn("passKunden", typeof(string));
            //table.Columns.Add(column);

            //column = new DataColumn("graphKey", typeof(string));
            //table.Columns.Add(column);

            column = new DataColumn("bemerkung", typeof(string));
            column.Caption = "Bemerkung";
            table.Columns.Add(column);

            //column = new DataColumn("zustadn", typeof(string));
            //table.Columns.Add(column);

            //column = new DataColumn("bereicht", typeof(string));
            //table.Columns.Add(column);

            //column = new DataColumn("internVermerk", typeof(string));
            //table.Columns.Add(column);
            using (MySqlConnection cn = new MySqlConnection())
            {
                cn.ConnectionString = App.GetConnection();
                try
                {
                    cn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("Select * From service", cn))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            // table.Load(dr);
                            while (dr.Read())
                            {
                                DataRow row = table.NewRow();
                                row["id"] = dr["id"].ToString();
                                row["dateTime"] = dr["dateTime"].ToString();
                                row["status"] = dr["status"].ToString();
                                row["clientID"] = dr["clientID"].ToString();
                                row["kundNamVorMatch"] = dr["kundNamVorMatch"].ToString();

                                row["gerat"] = dr["gerat"].ToString();
                                row["serialNummer"] = dr["serialNummer"].ToString();
                                //row["zubehor"] = dr["zubehor"].ToString();
                                //row["fehlerBeschreibung"] = dr["fehlerBeschreibung"].ToString();
                                // row["maxPrice"] = dr["maxPrice"].ToString();
                                row["mitarbeiterNach"] = dr["mitarbeiterNach"].ToString();
                                //row["mitarbeiterAus"] = dr["mitarbeiterAus"].ToString();
                                //row["passKunden"] = dr["passKunden"].ToString();
                                //row["graphKey"] = dr["graphKey"].ToString();
                                row["bemerkung"] = dr["bemerkung"].ToString();
                                //row["zustadn"] = dr["zustadn"].ToString();
                                //row["bereicht"] = dr["bereicht"].ToString();
                                //row["internVermerk"] = dr["internVermerk"].ToString();
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
            //подсчёт кол. договоров
            auftragQualitat(table);
            this.dataGridMain.ItemsSource = table.AsDataView();
        }

        private void DataGridMain_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "id":
                    e.Column.Header = "Nr.";
                    break;

                case "dateTime":
                    e.Column.Header = "Datum";
                    break;
                case "status":
                    e.Column.Header = "Status";
                    break;
                case "clientID":
                    e.Column.Header = "Kundennr.";
                    break;
                case "gerat":
                    e.Column.Header = "Gerät";
                    break;
                case "serialNummer":
                    e.Column.Header = "Seriennummer";
                    break;
                case "mitarbeiterNach":
                    e.Column.Header = "angenomen von";
                    break;
                case "bemerkung":
                    e.Column.Header = "Bemerkung";
                    break;
                case "kundNamVorMatch":
                    e.Column.Header = "Kunden matchcode";
                    break;
                default:
                    break;
            }
        }

        private void auftragQualitat(DataTable table)
        {
            statBarAuftrag.Items.Clear();
            int allRows = 0;
            int grunstatus = 0;
            int rotStatus = 0;
            foreach (DataRow row in table.Rows)
            {
                allRows++;
                if (row["status"].ToString() == "Angenommen von" || row["status"].ToString() == "In Bearbeitung" || row["status"].ToString() == "Warten auf Ersatzteile" || row["status"].ToString() == "Gerät ist beim Hersteller")
                {
                    rotStatus++;
                }
                else
                {
                    grunstatus++;
                }
            }
            Label reparaturAuftrage = new Label();
            reparaturAuftrage.Foreground = Brushes.Black;
            reparaturAuftrage.Content = string.Format("Reparaturaufträge: {0} ", allRows);
            statBarAuftrag.Items.Add(reparaturAuftrage);

            Label Offen = new Label();
            Offen.Foreground = Brushes.Red;
            Offen.Content = string.Format(" Offen: {0}", rotStatus);
            statBarAuftrag.Items.Add(Offen);

            Label Fertig = new Label();
            Fertig.Foreground = Brushes.Green;
            Fertig.Content = string.Format(" Fertig: {0}", grunstatus);
            statBarAuftrag.Items.Add(Fertig);
        }
        //neu auftrag
        private void NeuerAuftrag(object sender, RoutedEventArgs e)
        {
            LoadEmployee();
            takeWindow takeWnd = new takeWindow();
            takeWnd.ShowDialog();
            readAllTableService();
        }

        private void Option_Click(object sender, RoutedEventArgs e)
        {
            Setting_DB DbSet = new Setting_DB();
            DbSet.ShowDialog();
            readAllTableService();
        }
    }
}
