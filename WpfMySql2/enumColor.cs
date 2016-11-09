using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfMySql2
{
   public sealed class enumColor
    {
        //static Color col1 = new Color() { A = 100, B = 220, G = 220, R = 220 };
        // public readonly static SolidColorBrush First = new SolidColorBrush(col1);
        public readonly static SolidColorBrush First = Brushes.Yellow;

        // static Color col2 = new Color() { A = 100, B = 180, G = 180, R = 180};
        public readonly static SolidColorBrush Second = Brushes.Blue;
        //  public readonly static SolidColorBrush Second = new SolidColorBrush(col2);

        //static Color col3 = new Color() { A = 100, B = 130, G = 130, R = 130 };
        public readonly static SolidColorBrush Third = Brushes.Green;
        // public readonly static SolidColorBrush Third = new SolidColorBrush(col3);

        //static Color col4 = new Color() { A = 100, B = 80, G = 80, R = 80 };
        public readonly static SolidColorBrush Fourth = Brushes.Magenta;
        // public readonly static SolidColorBrush Fourth = new SolidColorBrush(col4);

        // static Color col5 = new Color() { A = 100, B = 30, G = 30, R = 30 };
        public readonly static SolidColorBrush Fivth = Brushes.Gray;
        // public readonly static SolidColorBrush Fivth = new SolidColorBrush(col5);
    }
}
