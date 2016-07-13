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
    /// Interaction logic for Artikel.xaml
    /// </summary>
    public partial class Artikel : Window
    {
        public Artikel()
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
                InfoDetail.artikelRow = (DataRowView)mainGrid.SelectedItem;    //принимает весь ряд но можно юзать только ИД клиента из бд.
            }
            this.Close();
        }

        private void DataGridClients_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid)
            {
                DataGrid mainGrid = (DataGrid)sender;
                InfoDetail.artikelRow = ((DataRowView)mainGrid.SelectedItem);
            }
        }

        private void readAllTableGrid()
        {
            DataTable table = new DataTable();
            DataColumn column = new DataColumn("Suchbegriff", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("Arikelnummer", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("Kurzname", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("VK-Preis 5N", typeof(string));
            table.Columns.Add(column);

            using (MySqlConnection cn = new MySqlConnection())
            {
                cn.ConnectionString = App.GetConnection();
                try
                {
                    cn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("Select * From artikel", cn))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            // table.Load(dr);
                            while (dr.Read())
                            {
                                DataRow row = table.NewRow();
                                row["Suchbegriff"] = dr["MATCHCODE"].ToString();
                                row["Arikelnummer"] = dr["ARTNUM"].ToString();
                                row["Kurzname"] = dr["KURZNAME"].ToString();
                                row["VK-Preis 5N"] = dr["VK5"].ToString();
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
            DataColumn column = new DataColumn("Suchbegriff", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("Arikelnummer", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("Kurzname", typeof(string));
            table.Columns.Add(column);
            column = new DataColumn("VK-Preis 5N", typeof(string));
            table.Columns.Add(column);

            using (MySqlConnection cn = new MySqlConnection())
            {
                cn.ConnectionString = App.GetConnection();
                try
                {
                    cn.Open();
                    string CommandStringEnterText = String.Format("Select * From artikel WHERE MATCHCODE  LIKE '%{0}%'", this.enterdText.Text);
                    using (MySqlCommand cmd = new MySqlCommand(CommandStringEnterText, cn))
                    {
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            // table.Load(dr);
                            while (dr.Read())
                            {
                                DataRow row = table.NewRow();
                                row["Suchbegriff"] = dr["MATCHCODE"].ToString();
                                row["Arikelnummer"] = dr["ARTNUM"].ToString();
                                row["Kurzname"] = dr["KURZNAME"].ToString();
                                row["VK-Preis 5N"] = dr["VK5"].ToString();
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
