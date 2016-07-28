using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;
namespace WpfMySql2
{
    /// <summary>
    /// Interaction logic for InfoDetail.xaml
    /// </summary>
    public partial class InfoDetail : Window
    {

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        [DllImport("user32.dll")]
        internal static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);


        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindow(IntPtr hWnd, GetWindowType uCmd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        IntPtr hWindow = IntPtr.Zero;
        string dateTimeString = null;
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
            if (combItem.Content.ToString() == "Abgeholt/Abgeschlossen")
            {
                if (MessageBoxResult.Yes == MessageBox.Show("einen neuen Auftrag in der CAO hinzufügen? \n Wenn ja, führen Sie das CAO (log), und warten Sie, und nicht etwa 15 Sekunden lang drücken.", "hinzufügen Auftrag?", MessageBoxButton.YesNo))
                {
                    hWindow = FindWindow("CAO.Faktura", null);
                    if (hWindow == IntPtr.Zero)
                    {
                        MessageBox.Show("CAO nicht started!\n Starten Sie die CAO (log\n und versuchen Sie es erneut.", "CAO nicht started!");
                    }
                    else
                    {
                        if (true)//проходит проверку если висит окно авторизации - нужно проверять дальше есть ли авторизация и не открыты ли другие окна
                        {
                            Console.WriteLine("CAO OK!");
                            Console.WriteLine(hWindow.ToString());

                            //открыть страницу нового ауфтрагов
                            INPUT[] pInputs = new INPUT[]
                       {
                  new INPUT()
                  {
                     type = InputType.KEYBOARD,
                     U = new InputUnion()
                     {
                         ki = new KEYBDINPUT()
                         {
                            wScan = ScanCodeShort.MENU,
                            wVk = VirtualKeyShort.MENU
                         }
                     }
                  },
                  new INPUT()
                  {
                     type = InputType.KEYBOARD,
                     U = new InputUnion()
                     {
                         ki = new KEYBDINPUT()
                         {
                            wScan = ScanCodeShort.CONTROL,
                            wVk = VirtualKeyShort.CONTROL,
                         }
                     }
                  },
                  new INPUT()
                  {
                     type = InputType.KEYBOARD,
                     U = new InputUnion()
                     {
                         ki = new KEYBDINPUT()
                         {
                            wScan = ScanCodeShort.KEY_U,
                            wVk = VirtualKeyShort.KEY_U,
                         }
                     }
                  },
                  new INPUT()
                  {
                     type = InputType.KEYBOARD,
                     U = new InputUnion()
                     {
                         ki = new KEYBDINPUT()
                         {
                            wScan = ScanCodeShort.KEY_U,
                            wVk = VirtualKeyShort.KEY_U,
                            dwFlags = KEYEVENTF.KEYUP
                         }
                     }
                  },
                  new INPUT()
                  {
                     type = InputType.KEYBOARD,
                     U = new InputUnion()
                     {
                         ki = new KEYBDINPUT()
                         {
                            wScan = ScanCodeShort.CONTROL,
                            wVk = VirtualKeyShort.CONTROL,
                            dwFlags = KEYEVENTF.KEYUP
                         }
                     }
                  },
                  new INPUT()
                  {
                     type = InputType.KEYBOARD,
                     U = new InputUnion()
                     {
                         ki = new KEYBDINPUT()
                         {
                            wScan = ScanCodeShort.MENU,
                            wVk = VirtualKeyShort.MENU,
                           dwFlags = KEYEVENTF.KEYUP
                         }
                     }
                  },
                       };
                            SetForegroundWindow(hWindow);
                            SendInput((uint)pInputs.Length, pInputs, INPUT.Size);//

                            Thread.Sleep(1000);

                            //создать новый ауфтраг, без данных
                            INPUT[] pInputsNew = new INPUT[]
                            {
                        new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.CONTROL,
                                      wVk = VirtualKeyShort.CONTROL

                                  }
                              }
                          },
                          new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.INSERT,
                                      wVk = VirtualKeyShort.INSERT
                                  }
                              }
                          },
                          new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.CONTROL,
                                      wVk = VirtualKeyShort.CONTROL,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                          new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.INSERT,
                                      wVk = VirtualKeyShort.INSERT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          }
                            };
                            SetForegroundWindow(hWindow);
                            SendInput((uint)pInputsNew.Length, pInputsNew, INPUT.Size);

                            Thread.Sleep(1000);

                            INPUT[] pInputKundList = new INPUT[]
                        {
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.F3,
                                      wVk = VirtualKeyShort.F3
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.F3,
                                      wVk = VirtualKeyShort.F3,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          }
                        };

                            SendInput((uint)pInputKundList.Length, pInputKundList, INPUT.Size);

                            Thread.Sleep(1000);

                            List<INPUT> BackSpace80 = new List<INPUT>();
                            for (int i = 0; i < 80; i++)
                            {
                                BackSpace80.Add(new INPUT()
                                {
                                    type = InputType.KEYBOARD,
                                    U = new InputUnion()
                                    {
                                        ki = new KEYBDINPUT()
                                        {
                                            wScan = ScanCodeShort.BACK,
                                            wVk = VirtualKeyShort.BACK
                                        }
                                    }
                                });

                                BackSpace80.Add(new INPUT()
                                {
                                    type = InputType.KEYBOARD,
                                    U = new InputUnion()
                                    {
                                        ki = new KEYBDINPUT()
                                        {
                                            wScan = ScanCodeShort.BACK,
                                            wVk = VirtualKeyShort.BACK,
                                            dwFlags = KEYEVENTF.KEYUP
                                        }
                                    }
                                });
                            }
                            INPUT[] arBack80 = BackSpace80.ToArray();

                            SendInput((uint)arBack80.Length, arBack80, INPUT.Size);

                            Thread.Sleep(300);

                            INPUT[] suchBegr = new INPUT[]//при открытии нового окна поиск в бд кундов автоматически стоит по suchbegrif
                         {
                              new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                     dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            //11 UP
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                     dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                             new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                     dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                             new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                     dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                             new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                     dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                             new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                     dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                             new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                     dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                             new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                     dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                             new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                     dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                             new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                     dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                             new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                     dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                             new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.UP,
                                      wVk = VirtualKeyShort.UP,
                                     dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            //1 down
                             new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.DOWN,
                                      wVk = VirtualKeyShort.DOWN
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.DOWN,
                                      wVk = VirtualKeyShort.DOWN,
                                     dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            //Shift + TAB zuruk zu suchfeld
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LSHIFT,
                                      wVk = VirtualKeyShort.LSHIFT
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LSHIFT,
                                      wVk = VirtualKeyShort.LSHIFT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          }
                         };

                            SendInput((uint)suchBegr.Length, suchBegr, INPUT.Size);

                            Thread.Sleep(500);

                            INPUT[] nameKunde = stringToInput(row["MATCHCODE"].ToString()).ToArray();
                            SendInput((uint)nameKunde.Length, nameKunde, INPUT.Size);

                            Thread.Sleep(2000);

                            INPUT[] f12 = new INPUT[]//подтверждение выбора кунда
                        {
                             new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.F12,
                                      wVk = VirtualKeyShort.F12
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.F12,
                                      wVk = VirtualKeyShort.F12,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          }
                       };

                            SendInput((uint)f12.Length, f12, INPUT.Size);

                            Thread.Sleep(1000);

                            INPUT[] f8 = new INPUT[]//выбор способа оплаты и доставки и продвижение к след. подокну
                         {
                             new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.KEY_7,
                                      wVk = VirtualKeyShort.KEY_7
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.KEY_7,
                                      wVk = VirtualKeyShort.KEY_7,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.RETURN,
                                      wVk = VirtualKeyShort.RETURN
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.RETURN,
                                      wVk = VirtualKeyShort.RETURN,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.KEY_1,
                                      wVk = VirtualKeyShort.KEY_1
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.KEY_1,
                                      wVk = VirtualKeyShort.KEY_1,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.RETURN,
                                      wVk = VirtualKeyShort.RETURN
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.RETURN,
                                      wVk = VirtualKeyShort.RETURN,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                             new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.F8,
                                      wVk = VirtualKeyShort.F8
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.F8,
                                      wVk = VirtualKeyShort.F8,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          }
                        };


                            SetForegroundWindow(hWindow);

                            SendInput((uint)f8.Length, f8, INPUT.Size);

                            Thread.Sleep(1000);

                            //заполнение артиклей и текста
                            INPUT[] textArtikel = new INPUT[]
                            {
                        new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LCONTROL,
                                      wVk = VirtualKeyShort.LCONTROL
                                  }
                              }
                          },
                        new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.KEY_T,
                                      wVk = VirtualKeyShort.KEY_T
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.KEY_T,
                                      wVk = VirtualKeyShort.KEY_T,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LCONTROL,
                                      wVk = VirtualKeyShort.LCONTROL,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          }
                            };


                            SendInput((uint)textArtikel.Length, textArtikel, INPUT.Size);

                            Thread.Sleep(1000);

                            //нужно немного подождать пока в грид сообразит что можно вносить текст
                            INPUT[] enterText = new INPUT[]
                            {
                           new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.RETURN,
                                      wVk = VirtualKeyShort.RETURN
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.RETURN,
                                      wVk = VirtualKeyShort.RETURN,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          }
                            };

                            SendInput((uint)enterText.Length, enterText, INPUT.Size);

                            Thread.Sleep(1000);
                            
                            string vonStr = ((ComboBoxItem)mitarbeiterAus.SelectedItem).Content.ToString();
                            string textAuftrag = this.Title + "\n" + vonStr + "\n" + dateTimeString + "\n" + reparBericht.Text;
                            INPUT[] RepnumDatumVonBericht = stringToInput(textAuftrag).ToArray();
                            SendInput((uint)RepnumDatumVonBericht.Length, RepnumDatumVonBericht, INPUT.Size);

                            Thread.Sleep(1000);

                            //нажим ИНСЕРТ для поиска и ввода артикля

                            INPUT[] offnenFenstArtikel = new INPUT[]
                            {
                        new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.INSERT,
                                      wVk = VirtualKeyShort.INSERT
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.INSERT,
                                      wVk = VirtualKeyShort.INSERT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          }
                            };


                            SendInput((uint)offnenFenstArtikel.Length, offnenFenstArtikel, INPUT.Size);

                            Thread.Sleep(1000);

                            //переход в поле поиска с изменением критерия поискан на зухбегриф
                            INPUT[] enerSuchFeld = new INPUT[]
                            {
                        new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.SHIFT,
                                      wVk = VirtualKeyShort.SHIFT
                                  }
                              }
                          },
                        new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.SHIFT,
                                      wVk = VirtualKeyShort.SHIFT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.LEFT,
                                      wVk = VirtualKeyShort.LEFT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.RIGHT,
                                      wVk = VirtualKeyShort.RIGHT,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.RIGHT,
                                      wVk = VirtualKeyShort.RIGHT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.RIGHT,
                                      wVk = VirtualKeyShort.RIGHT,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.RIGHT,
                                      wVk = VirtualKeyShort.RIGHT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.RIGHT,
                                      wVk = VirtualKeyShort.RIGHT,
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.RIGHT,
                                      wVk = VirtualKeyShort.RIGHT,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          },
                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB
                                  }
                              }
                          },

                            new INPUT()
                          {
                              type = InputType.KEYBOARD,
                              U = new InputUnion()
                              {
                                  ki = new KEYBDINPUT()
                                  {
                                      wScan = ScanCodeShort.TAB,
                                      wVk = VirtualKeyShort.TAB,
                                      dwFlags = KEYEVENTF.KEYUP
                                  }
                              }
                          }
                            };

                            SendInput((uint)enerSuchFeld.Length, enerSuchFeld, INPUT.Size);

                            Thread.Sleep(1000);

                            DataGridArtikel
                        }
                    }
                }
            }
        }

        List<INPUT> stringToInput(string str)
        {
            List<INPUT> listInput = new List<INPUT>();
            char[] charAr = str.ToCharArray();
            foreach (char ch in charAr)
            {
                switch (ch)
                {
                    case '0':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_0,
                                    wVk = VirtualKeyShort.KEY_0
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_0,
                                    wVk = VirtualKeyShort.KEY_0,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case '1':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_1,
                                    wVk = VirtualKeyShort.KEY_1
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_1,
                                    wVk = VirtualKeyShort.KEY_1,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case '2':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_2,
                                    wVk = VirtualKeyShort.KEY_2
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_2,
                                    wVk = VirtualKeyShort.KEY_2,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case '3':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_3,
                                    wVk = VirtualKeyShort.KEY_3
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_3,
                                    wVk = VirtualKeyShort.KEY_3,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case '4':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_4,
                                    wVk = VirtualKeyShort.KEY_4
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_4,
                                    wVk = VirtualKeyShort.KEY_4,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case '5':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_5,
                                    wVk = VirtualKeyShort.KEY_5
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_5,
                                    wVk = VirtualKeyShort.KEY_5,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case '6':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_6,
                                    wVk = VirtualKeyShort.KEY_6
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_6,
                                    wVk = VirtualKeyShort.KEY_6,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case '7':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_7,
                                    wVk = VirtualKeyShort.KEY_7
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_7,
                                    wVk = VirtualKeyShort.KEY_7,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case '8':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_8,
                                    wVk = VirtualKeyShort.KEY_8
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_8,
                                    wVk = VirtualKeyShort.KEY_8,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case '9':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_9,
                                    wVk = VirtualKeyShort.KEY_9
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_9,
                                    wVk = VirtualKeyShort.KEY_9,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'A':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_A,
                                    wVk = VirtualKeyShort.KEY_A
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_A,
                                    wVk = VirtualKeyShort.KEY_A,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'B':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_B,
                                    wVk = VirtualKeyShort.KEY_B
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_B,
                                    wVk = VirtualKeyShort.KEY_B,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'C':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_C,
                                    wVk = VirtualKeyShort.KEY_C
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_C,
                                    wVk = VirtualKeyShort.KEY_C,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'D':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_D,
                                    wVk = VirtualKeyShort.KEY_D
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_D,
                                    wVk = VirtualKeyShort.KEY_D,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'E':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_E,
                                    wVk = VirtualKeyShort.KEY_E
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_E,
                                    wVk = VirtualKeyShort.KEY_E,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'F':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_F,
                                    wVk = VirtualKeyShort.KEY_F
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_F,
                                    wVk = VirtualKeyShort.KEY_F,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'G':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_G,
                                    wVk = VirtualKeyShort.KEY_G
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_G,
                                    wVk = VirtualKeyShort.KEY_G,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'H':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_H,
                                    wVk = VirtualKeyShort.KEY_H
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_H,
                                    wVk = VirtualKeyShort.KEY_H,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'I':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_I,
                                    wVk = VirtualKeyShort.KEY_I
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_I,
                                    wVk = VirtualKeyShort.KEY_I,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'J':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_J,
                                    wVk = VirtualKeyShort.KEY_J
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_J,
                                    wVk = VirtualKeyShort.KEY_J,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'K':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_K,
                                    wVk = VirtualKeyShort.KEY_K
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_K,
                                    wVk = VirtualKeyShort.KEY_K,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'L':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_L,
                                    wVk = VirtualKeyShort.KEY_L
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_L,
                                    wVk = VirtualKeyShort.KEY_L,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'M':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_M,
                                    wVk = VirtualKeyShort.KEY_M
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_M,
                                    wVk = VirtualKeyShort.KEY_M,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'N':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_N,
                                    wVk = VirtualKeyShort.KEY_N
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_N,
                                    wVk = VirtualKeyShort.KEY_N,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'O':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_O,
                                    wVk = VirtualKeyShort.KEY_O
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_O,
                                    wVk = VirtualKeyShort.KEY_O,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'P':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_P,
                                    wVk = VirtualKeyShort.KEY_P
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_P,
                                    wVk = VirtualKeyShort.KEY_P,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'Q':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_Q,
                                    wVk = VirtualKeyShort.KEY_Q
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_Q,
                                    wVk = VirtualKeyShort.KEY_Q,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'R':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_R,
                                    wVk = VirtualKeyShort.KEY_R
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_R,
                                    wVk = VirtualKeyShort.KEY_R,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'S':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_S,
                                    wVk = VirtualKeyShort.KEY_S
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_S,
                                    wVk = VirtualKeyShort.KEY_S,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'T':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_T,
                                    wVk = VirtualKeyShort.KEY_T
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_T,
                                    wVk = VirtualKeyShort.KEY_T,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'U':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_U,
                                    wVk = VirtualKeyShort.KEY_U
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_U,
                                    wVk = VirtualKeyShort.KEY_U,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'V':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_V,
                                    wVk = VirtualKeyShort.KEY_V
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_V,
                                    wVk = VirtualKeyShort.KEY_V,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'W':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_W,
                                    wVk = VirtualKeyShort.KEY_W
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_W,
                                    wVk = VirtualKeyShort.KEY_W,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'X':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_X,
                                    wVk = VirtualKeyShort.KEY_X
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_X,
                                    wVk = VirtualKeyShort.KEY_X,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'Y':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_Y,
                                    wVk = VirtualKeyShort.KEY_Y
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_Y,
                                    wVk = VirtualKeyShort.KEY_Y,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case 'Z':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_Z,
                                    wVk = VirtualKeyShort.KEY_Z
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.KEY_Z,
                                    wVk = VirtualKeyShort.KEY_Z,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case ',':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.OEM_COMMA,
                                    wVk = VirtualKeyShort.OEM_COMMA
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.OEM_COMMA,
                                    wVk = VirtualKeyShort.OEM_COMMA,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case '+':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.OEM_PLUS,
                                    wVk = VirtualKeyShort.OEM_PLUS
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.OEM_PLUS,
                                    wVk = VirtualKeyShort.OEM_PLUS,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;

                    case '-':
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.OEM_MINUS,
                                    wVk = VirtualKeyShort.OEM_MINUS
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.OEM_MINUS,
                                    wVk = VirtualKeyShort.OEM_MINUS,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;



                    //!@#$%^&*()_=.

                    default:
                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.SPACE,
                                    wVk = VirtualKeyShort.SPACE
                                }
                            }
                        });

                        listInput.Add(new INPUT()
                        {
                            type = InputType.KEYBOARD,
                            U = new InputUnion()
                            {
                                ki = new KEYBDINPUT()
                                {
                                    wScan = ScanCodeShort.SPACE,
                                    wVk = VirtualKeyShort.SPACE,
                                    dwFlags = KEYEVENTF.KEYUP
                                }
                            }
                        });
                        break;
                }
            }
            return listInput;
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
                dateTimeString = MainWindow.rowDetail["dateTime"].ToString();
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
                    string internerVermStr = MySqlHelper.EscapeString(InternerVermerkTxt.Text);//убирает какието символы из СТРИНГА для того чтобы можно было этот текст 
                    string reparBetichtStr = MySqlHelper.EscapeString(reparBericht.Text);//вносить в бд(кажется это связано со старой версией бд)
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
