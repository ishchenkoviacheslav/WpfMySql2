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
    /// Interaction logic for PrintWindow.xaml
    /// </summary>
    public partial class PrintWindow : Window
    {
        string kontakt = " Winkler IT Service UG \n (haftungsbeschränkt) \n Hauptstrasse 42, 92339 Beilngries \n Telefon: 08461 7001920 \n E-Mail: info@winkleritservice.de \n http://www.winkleritservice.de \n St-Nr.:201/142/50342";


        PageContent pc = new PageContent();
        FixedPage fp = new FixedPage();
        Canvas canvas = new Canvas();

        DataRow row = null;
        public PrintWindow()
        {
            InitializeComponent();
        }
        public PrintWindow(takeWindow infoDetail)
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
            tb.Text = "Reparaturauftrag  " + infoDetail.lastID.ToString();
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
            empInfo.Text = infoDetail.einMitarbeiterNach;
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
                clienInfo = row["MATCHCODE"].ToString() + "\nNAME:\t\t"+ row["ANREDE"] +" "+ row["NAME1"].ToString() + " " + row["NAME2"].ToString() + "\nSTRASSE:\t" + row["STRASSE"].ToString() + "  PLZ " + row["PLZ"].ToString() + "\nTelefon1:\t\t" + row["TELE1"].ToString() + "\nTelefon2:\t\t" + row["TELE2"].ToString() +  "\nORT:\t\t" + row["ORT"].ToString() + "\nEMAIL:\t\t" + row["EMAIL"].ToString();            
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

            //TextBlock labelPartsMaterialWorkTime = new TextBlock();
            //labelPartsMaterialWorkTime.FontWeight = FontWeights.Bold;
            //labelPartsMaterialWorkTime.Margin = new Thickness() { Top = 780, Left = 40 };
            //labelPartsMaterialWorkTime.FontSize = 18;
            //labelPartsMaterialWorkTime.FontStyle = FontStyles.Italic;
            //labelPartsMaterialWorkTime.Text = "Ersatzteile - Material - Zeit:";
            //canvas.Children.Add(labelPartsMaterialWorkTime);

            //TextBlock spareParts = new TextBlock();
            //spareParts.Margin = new Thickness() { Top = 810, Left = 40 };
            //spareParts.FontSize = 16;
            //spareParts.Text = infoDetail.textBoxSpareParts.Text;
            //canvas.Children.Add(spareParts);

            //TextBlock material = new TextBlock();
            //material.Margin = new Thickness() { Top = 830, Left = 40 };
            //material.FontSize = 16;
            //material.Text = infoDetail.textBoxMaterial.Text;
            //canvas.Children.Add(material);

            //TextBlock workTime = new TextBlock();
            //workTime.Margin = new Thickness() { Top = 850, Left = 40 };
            //workTime.FontSize = 16;
            //if (infoDetail.textBoxWorkTime.Text != "" && infoDetail.textBoxWorkTime.Text != null)
            //    workTime.Text = infoDetail.textBoxWorkTime.Text + " St.";
            //else
            //    workTime.Text = infoDetail.textBoxWorkTime.Text;
            //canvas.Children.Add(workTime);
            /////////////////////////////////////////////////////////////////////

            //TextBlock labelReport = new TextBlock();
            //labelReport.FontWeight = FontWeights.Bold;
            //labelReport.Margin = new Thickness() { Top = 880, Left = 40 };
            //labelReport.FontSize = 18;
            //labelReport.FontStyle = FontStyles.Italic;
            //labelReport.Text = "Bericht:";
            //canvas.Children.Add(labelReport);

            //TextBlock report = new TextBlock();
            //report.Margin = new Thickness() { Top = 910, Left = 40 };
            //report.FontSize = 16;
            //report.Text = infoDetail.textBoxReport.Text;
            //canvas.Children.Add(report);
            /////////////////////////////////////////////////////////////////////
        }
        private void BottomPrint()
        {
            TextBlock GeratLine = new TextBlock();
            GeratLine.Margin = new Thickness() { Top = 980, Left = 80 };
            GeratLine.FontSize = 16;
            GeratLine.Text = "______________________________________";
            canvas.Children.Add(GeratLine);

            TextBlock Gerat = new TextBlock();
            Gerat.Margin = new Thickness() { Top = 995, Left = 130 };
            Gerat.FontSize = 16;
            Gerat.Text = "Gerät übernommen";
            canvas.Children.Add(Gerat);

            /////////////////////////////////////////////////////////
            TextBlock AGBLine = new TextBlock();
            AGBLine.Margin = new Thickness() { Top = 980, Left = 470 };
            AGBLine.FontSize = 16;
            AGBLine.Text = "______________________________________";
            canvas.Children.Add(AGBLine);

            TextBlock AGB = new TextBlock();
            AGB.Margin = new Thickness() { Top = 995, Left = 500 };
            AGB.FontSize = 16;
            AGB.Text = "AGB zur Kenntnis genommen";
            canvas.Children.Add(AGB);
        }
    }
}
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
//using System.Data;
//namespace WpfMySql2
//{
//    /// <summary>
//    /// Interaction logic for PrintWindow.xaml
//    /// </summary>
//    public partial class PrintWindow : Window
//    {
//        string kontakt = " Winkler IT Service UG \n (haftungsbeschränkt) \n Hauptstrasse 42, 92339 Beilngries \n Telefon: 08461 7001920 \n E-Mail: info@winkleritservice.de \n http://www.winkleritservice.de \n St-Nr.:201/142/50342";

//        string AGB = @"Information:
//                        Wir sind stets bestrebt, Ihre Reparatur so schnell und unkompliziert wie möglich durchzuführen.
//                        Unser Serviceversprechen an Sie:
//                        Wir geben Ihnen umgehend Bescheid sobald wir näheres zur Reparatur sagen können oder Ihr Gerät fertig zur Abholung bereitsteht.
//                        Deshalb bitten wir Sie, von häufigen Rückfragen abzusehen. Sie helfen uns damit, Ihr und das Anliegen anderer Kunden zügiger bearbeiten zu können.
//                        Wichtige Hinweise:
//                        Beachten Sie bitte auch, dass Reparaturen - wenn nicht ausdrücklich anders vereinbar - bar oder per EC-Karte bei Abholung bezahlt werden müssen.
//                        Bitte beachten Sie:
//                        Wir sind stets bestrebt, Ihr Anliegen so schnell wie möglich zu bearbeiten. Es kann aber - je nach Art und Umfang der Reparatur - mehrere Tage dauern, bis Sie Ihr Gerät repariert zurückerhalten.
//                        Haftung für Datenverlust wird ausdrücklich ausgeschlossen!";

//        PageContent pc = new PageContent();
//        FixedPage fp = new FixedPage();
//        Canvas canvas = new Canvas();


//        public PrintWindow()
//        {
//            InitializeComponent();
//        }
//        public PrintWindow(InfoDetail infoDetail)
//        {
//            InitializeComponent();
//            TopPrint();
//            BodyPrint(infoDetail);
//            //add the text box to the FixedPage
//            fp.Children.Add(canvas);

//            //add the FixedPage to the PageContent 
//            pc.Child = fp;
//            //add the PageContent to the FixedDocument
//            fd.Pages.Add(pc);
//        }
//        void TopPrint()
//        {
//            Image simpleImage = new Image();
//            simpleImage.Width = 500;
//            simpleImage.Margin = new Thickness() { Top = 40, Left = 40 };

//            // Create source.
//            BitmapImage bi = new BitmapImage();
//            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
//            bi.BeginInit();
//            bi.UriSource = new Uri("WinklerITService.jpg", UriKind.Relative);
//            bi.EndInit();
//            // Set the image source.
//            simpleImage.Source = bi;
//            canvas.Children.Add(simpleImage);

//            TextBlock tb = new TextBlock();
//            //add some text to a TextBox object
//            tb.Margin = new Thickness() { Top = 40, Left = 550 };
//            tb.FontSize = 14;
//            tb.Text = kontakt;
//            canvas.Children.Add(tb);

//            TextBlock line = new TextBlock();
//            line.Margin = new Thickness() { Top = 170, Left = 40 };
//            line.FontWeight = FontWeights.Bold;
//            line.Text = "__________________________________________________________________________________________________________________________________________________";
//            canvas.Children.Add(line);
//        }
//        void BodyPrint(InfoDetail infoDetail)
//        {
//            /////////////////////////////////////////////////////////////////////////////////
//            TextBlock tb = new TextBlock();
//            //add some text to a TextBox object
//            tb.Margin = new Thickness() { Top = 200, Left = 40 };
//            tb.FontSize = 25;
//            tb.Text = "Reparaturauftrag  " + infoDetail.textBlockId.Text;
//            canvas.Children.Add(tb);
//            ////////////////////////////////////////////////////////////////////////////////
//            TextBlock client = new TextBlock();
//            client.Margin = new Thickness() { Top = 250, Left = 40 };
//            client.FontSize = 16;
//            string clienInfo = "";
//            foreach (DataRow row in infoDetail.tableCurrentClient.Rows)
//            {
//                clienInfo = row["MATCHCODE"].ToString() + "\n" + row["NAME1"].ToString() + row["NAME2"].ToString() + "\n" + row["STRASSE"].ToString() + row["PLZ"].ToString() + "\n" + row["TELE1"].ToString() + "\n" + row["TELE2"].ToString() + "\n" + row["FAX"].ToString() + "\n" + row["ORT"].ToString() + "\n" + row["EMAIL"].ToString() + "\n" + row["INTERNET"].ToString();
//            }
//            client.Text = clienInfo;
//            canvas.Children.Add(client);
//            ////////////////////////////////////////////////////////////////////////////////

//            TextBlock date = new TextBlock();
//            date.Margin = new Thickness() { Top = 250, Left = 470 };
//            date.FontSize = 16;
//            date.Text = infoDetail.textBlockDate.Text;
//            canvas.Children.Add(date);

//            TextBlock empInfo = new TextBlock();
//            empInfo.Margin = new Thickness() { Top = 270, Left = 470 };
//            empInfo.FontSize = 16;
//            empInfo.Text = infoDetail.textBlockEmployee.Text;
//            canvas.Children.Add(empInfo);
//            ////////////////////////////////////////////////////////////////////////////////

//            TextBlock obj = new TextBlock();
//            obj.Margin = new Thickness() { Top = 470, Left = 40 };
//            obj.FontSize = 16;
//            obj.Text = infoDetail.textBlockArticle.Text;
//            canvas.Children.Add(obj);

//            TextBlock acces = new TextBlock();
//            acces.Margin = new Thickness() { Top = 490, Left = 40 };
//            acces.FontSize = 16;
//            acces.Text = infoDetail.textBlockOp_equip.Text;
//            canvas.Children.Add(acces);

//            TextBlock serNum = new TextBlock();
//            serNum.Margin = new Thickness() { Top = 510, Left = 40 };
//            serNum.FontSize = 16;
//            serNum.Text = infoDetail.textBoxkSerialNum.Text;
//            canvas.Children.Add(serNum);
//            ////////////////////////////////////////////////////////////////////////////////

//            TextBlock reason = new TextBlock();
//            reason.Margin = new Thickness() { Top = 470, Left = 470 };
//            reason.FontSize = 16;
//            reason.Text = infoDetail.textBoxReason.Text;
//            canvas.Children.Add(reason);
//            //////////////////////////////////////////////////////////////////////////

//            TextBlock prevPrice = new TextBlock();
//            prevPrice.Margin = new Thickness() { Top = 550, Left = 40 };
//            prevPrice.FontSize = 16;
//            prevPrice.Text = infoDetail.textBoxPrevPrice.Text;
//            canvas.Children.Add(prevPrice);

//            TextBlock other = new TextBlock();
//            other.Margin = new Thickness() { Top = 570, Left = 40 };
//            other.FontSize = 16;
//            other.Text = infoDetail.textBoxOther.Text;
//            canvas.Children.Add(other);
//            ////////////////////////////////////////////////////////////////////////

//            TextBlock spareParts = new TextBlock();
//            spareParts.Margin = new Thickness() { Top = 570, Left = 470 };
//            spareParts.FontSize = 16;
//            spareParts.Text = infoDetail.textBoxSpareParts.Text;
//            canvas.Children.Add(spareParts);

//            TextBlock material = new TextBlock();
//            material.Margin = new Thickness() { Top = 590, Left = 470 };
//            material.FontSize = 16;
//            material.Text = infoDetail.textBoxMaterial.Text;
//            canvas.Children.Add(material);

//            TextBlock workTime = new TextBlock();
//            workTime.Margin = new Thickness() { Top = 610, Left = 470 };
//            workTime.FontSize = 16;
//            if (infoDetail.textBoxWorkTime.Text != "" && infoDetail.textBoxWorkTime.Text != null)
//                workTime.Text = infoDetail.textBoxWorkTime.Text + " St.";
//            else
//                workTime.Text = infoDetail.textBoxWorkTime.Text;
//            canvas.Children.Add(workTime);
//            /////////////////////////////////////////////////////////////////////

//            TextBlock report = new TextBlock();
//            report.Margin = new Thickness() { Top = 650, Left = 40 };
//            report.FontSize = 16;
//            report.Text = infoDetail.textBoxReport.Text;
//            canvas.Children.Add(report);
//            /////////////////////////////////////////////////////////////////////

//            TextBlock GeratLine = new TextBlock();
//            GeratLine.Margin = new Thickness() { Top = 970, Left = 80 };
//            GeratLine.FontSize = 16;
//            GeratLine.Text = "______________________________________";
//            canvas.Children.Add(GeratLine);

//            TextBlock Gerat = new TextBlock();
//            Gerat.Margin = new Thickness() { Top = 985, Left = 130 };
//            Gerat.FontSize = 16;
//            Gerat.Text = "Gerät übernommen";
//            canvas.Children.Add(Gerat);

//            /////////////////////////////////////////////////////////
//            TextBlock AGBLine = new TextBlock();
//            AGBLine.Margin = new Thickness() { Top = 970, Left = 470 };
//            AGBLine.FontSize = 16;
//            AGBLine.Text = "______________________________________";
//            canvas.Children.Add(AGBLine);

//            TextBlock AGB = new TextBlock();
//            AGB.Margin = new Thickness() { Top = 985, Left = 500 };
//            AGB.FontSize = 16;
//            AGB.Text = "AGB zur Kenntnis genommen";
//            canvas.Children.Add(AGB);
//        }
//    }
//}

