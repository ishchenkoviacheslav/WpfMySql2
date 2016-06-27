using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
namespace WpfMySql2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //возвращает раскодированую строку полученную из XML
        public static string PassToCode(string EncodedPass)
        {
            //***************From XML to Code**************************
            char[] arCha2 = EncodedPass.ToCharArray();
            int[] arInt2 = new int[arCha2.Length];
            for (int i = 0; i < arCha2.Length; i++)
            {
                arInt2[i] = ((int)arCha2[i] - 5);
            }
            for (int i = 0; i < arInt2.Length; i++)
            {
                arCha2[i] = (char)arInt2[i];
            }
            string res2 = new string(arCha2);
            res2 = res2.Remove(0, 13);
            res2 = res2.Remove(res2.Length - 23, 23);
            //***************From XML to Code**************************
            return res2;
        }

        //получает оригинальный пароль и кодирует его перед записью в XML
        public static string PassToXML(string originalPass)
        {
            //******************form XML****************
            char[] arCha = originalPass.ToCharArray();//массив чар
            int[] arInt = new int[arCha.Length];//такой же размер
            for (int i = 0; i < arCha.Length; i++)
            {
                arInt[i] = (((int)arCha[i]) + 5);
            }
            for (int i = 0; i < arInt.Length; i++)
            {
                arCha[i] = (char)arInt[i];
            }
            string pre = "9p45bmgavjert";
            string post = "uiluipldg6849we2n6y8u79";
            string res = new string(arCha);
            res = pre + res + post;
            //*****************from xml**********************
            return res;
        }
        public static string GetConnection()
        {
           string connectionString = ConfigurationManager.ConnectionStrings["MySQL_Database_CAO"].ConnectionString;
            string pass = "";
            //сделано потому что строка "дешифрующая" полученый пароль бросает искл. сит.
            //тоесть если пароль не задан то это будет происходить,
            //мы ни чего не теряем так как у нас будет общая иск. сит. для всего подключения(или подключение будет при пароле "")
            if (ConfigurationManager.AppSettings["pass"]!="")
            {
                pass = PassToCode(ConfigurationManager.AppSettings["pass"]);
            }            
            string DBName = ConfigurationManager.AppSettings["DBName"];
            string userName = ConfigurationManager.AppSettings["userName"];
            string IP = ConfigurationManager.AppSettings["servIP"];
            connectionString =  string.Format(connectionString, IP, userName, pass,DBName);           
            return connectionString;
        }
    }
}
