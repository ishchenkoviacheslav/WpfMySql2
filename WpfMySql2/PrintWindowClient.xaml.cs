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
using System.Data;
using MySql.Data.MySqlClient;

namespace WpfMySql2
{
    /// <summary>
    /// Interaction logic for PrintWindowClient.xaml
    /// </summary>
    public partial class PrintWindowClient : Window
    {
        string kontakt = " Winkler IT Service UG \n (haftungsbeschränkt) \n Hauptstrasse 42, 92339 Beilngries \n Telefon: 08461 7001920 \n E-Mail: info@winkleritservice.de \n http://www.winkleritservice.de \n St-Nr.:201/142/50342";

        string sAGB = "Information:\nWir sind stets bestrebt, Ihre Reparatur so schnell und unkompliziert wie möglich durchzuführen. Unser\nServiceversprechen an Sie: Wir geben Ihnen umgehend Bescheid sobald wir näheres zur Reparatur sagen können oder \nIhr Gerät fertig zur Abholung bereitsteht. Deshalb bitten wir Sie, von häufigen Rückfragen abzusehen. Sie helfen uns \ndamit, Ihr und das Anliegen anderer Kunden zügiger bearbeiten zu können. Wichtige Hinweise: Beachten Sie bitte auch,\ndass Reparaturen - wenn nicht ausdrücklich anders vereinbar - bar oder per EC-Karte bei Abholung bezahlt\nwerden müssen. Bitte beachten Sie: Wir sind stets bestrebt, Ihr Anliegen so schnell wie möglich zu bearbeiten.\nEs kann aber - je nach  Art und Umfang der Reparatur - mehrere Tage dauern, bis Sie Ihr Gerät repariert zurückerhalten. \nHaftung für Datenverlust wird ausdrücklich ausgeschlossen!";
        PageContent pc = new PageContent();
        FixedPage fp = new FixedPage();
        Canvas canvas = new Canvas();
        DataRow row = null;

        public PrintWindowClient()
        {
            InitializeComponent();
        }
        public PrintWindowClient(takeWindow infoDetail)
        {
            InitializeComponent();
            TopPrint();
            BodyPrint(infoDetail);
            BottomPrint();

            //add the text box to the FixedPage
            fp.Children.Add(canvas);

            //add the FixedPage to the PageContent 
            pc.Child = fp;
            //add the PageContent to the FixedDocument
            fd.Pages.Add(pc);
        }
        void readSomeTable(takeWindow infoDetail)
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
                    string CommandStringEnterText = String.Format("Select * From adressen WHERE REC_ID  LIKE '%{0}%'", infoDetail.meinKundeID);
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
        void TopPrint()
        {
            Image simpleImage = new Image();
            simpleImage.Width = 500;
            simpleImage.Margin = new Thickness() { Top = 40, Left = 40 };

            // Create source.
            BitmapImage bi = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            bi.BeginInit();
            bi.UriSource = new Uri("WinklerITService.jpg", UriKind.Relative);
            bi.EndInit();
            // Set the image source.
            simpleImage.Source = bi;
            canvas.Children.Add(simpleImage);

            TextBlock tb = new TextBlock();
            //add some text to a TextBox object
            tb.Margin = new Thickness() { Top = 40, Left = 550 };
            tb.FontSize = 14;
            tb.Text = kontakt;
            canvas.Children.Add(tb);

            TextBlock line = new TextBlock();
            line.Margin = new Thickness() { Top = 170, Left = 40 };
            line.FontWeight = FontWeights.Bold;
            line.Text = "__________________________________________________________________________________________________________________________________________________";
            canvas.Children.Add(line);
        }
        void BodyPrint(takeWindow infoDetail)
        {
            /////////////////////////////////////////////////////////////////////////////////
            TextBlock tb = new TextBlock();
            //add some text to a TextBox object
            tb.Margin = new Thickness() { Top = 200, Left = 40 };
            tb.FontSize = 25;
            tb.Text = "Reparaturauftrag  " + infoDetail.lastID;
            canvas.Children.Add(tb);

            ////////////////////////////////////////////////////////////////////////////////
            TextBlock labelDataTime = new TextBlock();
            labelDataTime.FontWeight = FontWeights.Bold;
            labelDataTime.Margin = new Thickness() { Top = 250, Left = 40 };
            labelDataTime.FontSize = 18;
            labelDataTime.FontStyle = FontStyles.Italic;
            labelDataTime.Text = "Datum - Zeit - Mitarbeiter:";
            canvas.Children.Add(labelDataTime);

            TextBlock date = new TextBlock();
            date.Margin = new Thickness() { Top = 280, Left = 40 };
            date.FontSize = 16;
            date.Text = infoDetail.dataZeit;
            canvas.Children.Add(date);

            TextBlock empInfo = new TextBlock();
            empInfo.Margin = new Thickness() { Top = 280, Left = 200 };
            empInfo.FontSize = 16;
            empInfo.Text = infoDetail.mitarbeiterNach.Text;
            canvas.Children.Add(empInfo);
            ////////////////////////////////////////////////////////////////////////////////

            TextBlock labelKunde = new TextBlock();
            labelKunde.FontWeight = FontWeights.Bold;
            labelKunde.Margin = new Thickness() { Top = 310, Left = 40 };
            labelKunde.FontSize = 18;
            labelKunde.FontStyle = FontStyles.Italic;
            labelKunde.Text = "Kunde:";
            canvas.Children.Add(labelKunde);

            TextBlock client = new TextBlock();
            client.Margin = new Thickness() { Top = 340, Left = 40 };
            client.FontSize = 16;
            string clienInfo = "";
            readSomeTable(infoDetail);
            //взять ИД клиента достать его из бд и распечатать
            clienInfo = row["MATCHCODE"].ToString() + "\nNAME:\t\t" + row["ANREDE"] + " " + row["NAME1"].ToString() + " " + row["NAME2"].ToString() + "\nSTRASSE:\t" + row["STRASSE"].ToString() + "  PLZ " + row["PLZ"].ToString() + "\nTelefon1:\t\t" + row["TELE1"].ToString() + "\nTelefon2:\t\t" + row["TELE2"].ToString() + "\nORT:\t\t" + row["ORT"].ToString() + "\nEMAIL:\t\t" + row["EMAIL"].ToString();
            client.Text = clienInfo;
            canvas.Children.Add(client);
            ////////////////////////////////////////////////////////////////////////////////

            TextBlock labelObj = new TextBlock();
            labelObj.FontWeight = FontWeights.Bold;
            labelObj.Margin = new Thickness() { Top = 540, Left = 40 };
            labelObj.FontSize = 18;
            labelObj.FontStyle = FontStyles.Italic;
            labelObj.Text = "Gerät - Zubehör - Seriennummer:";
            canvas.Children.Add(labelObj);

            TextBlock obj = new TextBlock();
            obj.Margin = new Thickness() { Top = 570, Left = 40 };
            obj.FontSize = 16;
            obj.Text = infoDetail.geratTxt.Text;
            canvas.Children.Add(obj);

            TextBlock acces = new TextBlock();
            acces.Margin = new Thickness() { Top = 590, Left = 40 };
            acces.FontSize = 16;
            acces.Text = infoDetail.zubehorTxt.Text;
            canvas.Children.Add(acces);

            TextBlock serNum = new TextBlock();
            serNum.Margin = new Thickness() { Top = 610, Left = 40 };
            serNum.FontSize = 16;
            serNum.Text = infoDetail.serialNummerTxt.Text;
            canvas.Children.Add(serNum);
            ////////////////////////////////////////////////////////////////////////////////

            TextBlock labelReasone = new TextBlock();
            labelReasone.FontWeight = FontWeights.Bold;
            labelReasone.Margin = new Thickness() { Top = 640, Left = 40 };
            labelReasone.FontSize = 18;
            labelReasone.FontStyle = FontStyles.Italic;
            labelReasone.Text = "Schadenbeschreibung:";
            canvas.Children.Add(labelReasone);

            TextBlock reason = new TextBlock();
            reason.Margin = new Thickness() { Top = 670, Left = 40 };
            reason.FontSize = 16;
            reason.Text = infoDetail.FehlerbeschreibungTxt.Text;
            canvas.Children.Add(reason);
            //////////////////////////////////////////////////////////////////////////

            TextBlock labelPriceOther = new TextBlock();
            labelPriceOther.FontWeight = FontWeights.Bold;
            labelPriceOther.Margin = new Thickness() { Top = 700, Left = 40 };
            labelPriceOther.FontSize = 18;
            labelPriceOther.FontStyle = FontStyles.Italic;
            labelPriceOther.Text = "Vorläufige Preise und andere Informationen:";
            canvas.Children.Add(labelPriceOther);

            TextBlock prevPrice = new TextBlock();
            prevPrice.Margin = new Thickness() { Top = 730, Left = 40 };
            prevPrice.FontSize = 16;
            prevPrice.Text = infoDetail.maxPrice.Text;
            canvas.Children.Add(prevPrice);

            TextBlock other = new TextBlock();
            other.Margin = new Thickness() { Top = 750, Left = 40 };
            other.FontSize = 16;
            other.Text = infoDetail.zustandTxt.Text;
            canvas.Children.Add(other);
            ////////////////////////////////////////////////////////////////////////           
           
        }
        private void BottomPrint()
        {

            TextBlock labelAGB = new TextBlock();
            labelAGB.FontWeight = FontWeights.Bold;
            labelAGB.Margin = new Thickness() { Top = 780, Left = 40 };
            labelAGB.FontSize = 18;
            labelAGB.FontStyle = FontStyles.Italic;
            labelAGB.Text = "Unsere AGB:";
            canvas.Children.Add(labelAGB);

            TextBlock AGB = new TextBlock();
            AGB.Margin = new Thickness() { Top = 810, Left = 40 };
            AGB.FontSize = 14;
            AGB.Text = sAGB;
            canvas.Children.Add(AGB);
   
        }
    }
}
