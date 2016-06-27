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
using MySql.Data.MySqlClient;
using System.Data;
namespace WpfMySql2
{
    /// <summary>
    /// Interaction logic for clientsBase.xaml
    /// </summary>
    public partial class clientsBase : Window
    {
        public clientsBase()
        {
            InitializeComponent();
            this.enterdText.TextChanged += EnterdText_TextChanged;
            readAllTableGrid();
            dataGridClients.MouseLeftButtonUp += DataGridClients_MouseLeftButtonUp;
            dataGridClients.MouseDoubleClick += DataGridClients_MouseDoubleClick;
        }

        private void DataGridClients_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid)
            {
                DataGrid mainGrid = (DataGrid)sender;
                MainWindow.kundenID = (DataRowView)mainGrid.SelectedItem;    //принимает весь ряд но можно юзать только ИД клиента из бд.
            }
            this.Close();
        }

        private void DataGridClients_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid)
            {
                DataGrid mainGrid = (DataGrid)sender;
                MainWindow.kundenID = ((DataRowView)mainGrid.SelectedItem);                                      
            }
        }

        private void readAllTableGrid()
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
                    using (MySqlCommand cmd = new MySqlCommand("Select * From adressen", cn))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            // table.Load(dr);
                            while (dr.Read())
                            {
                                DataRow row = table.NewRow();
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

            this.dataGridClients.ItemsSource = table.AsDataView();

        }

        private void EnterdText_TextChanged(object sender, TextChangedEventArgs e)
        {
            readSomeTable();
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
                    string CommandStringEnterText = String.Format("Select * From adressen WHERE MATCHCODE  LIKE '%{0}%'", this.enterdText.Text);
                    using (MySqlCommand cmd = new MySqlCommand(CommandStringEnterText, cn))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            // table.Load(dr);
                            while (dr.Read())
                            {
                                DataRow row = table.NewRow();
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
            this.dataGridClients.ItemsSource = table.AsDataView();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            enterdText.Text = "";
        }

        private void ButtonForEsc_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
