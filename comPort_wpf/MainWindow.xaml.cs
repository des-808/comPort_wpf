using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace comPort_wpf
{
    public partial class MainWindow : Window
    {
        //public delegate void Mydelegate();
        private static Settings1 settings = new();
        private string[] ports = new[] { "" };
        private static ComStruct comInitStruct = new();
        public static SerialPort MyPort = new();
        public ContextMenu? mPortContextMenu = new ContextMenu();
        private delegate void UpTDelegate(string text);
        public Terminal term = new Terminal() { HEX = "", ASCII = "" };
        public StringToHex stringToHex = new StringToHex();
        bool btnStop = false;
        bool btnVisibleClock = true;

        // public ComSearch_ comsearch = new ();
        //string[] arrBoudRate = new[] { "110", "300", "600", "1200", "2400", "4800", "9600", "14400", "19200", "38400", "56000", "57600", "115200", "128000", "256000" };
        //string[] arrBit =      new[] { "5","6","7","8" };
        //string[] arrParitet =  new[] { "нет.","нечёт.","чёт.","марк.","пробел" };
        //string[] arrStop =     new[] { "1","1.5","2" };
        ////public static readonly DependencyProperty ComProperty;
        public ObservableCollection<Terminal> terminalTx { get; set; }
        public ObservableCollection<Terminal> terminalRx { get; set; }
        public ObservableCollection<ComSearch_> listViewCom { get; set; }
        public static ComStruct ComInitStruct { get => comInitStruct; set => comInitStruct = value; }

        public double GetWidthMainWindow() => mainWindow.Width;
        public void SetWidthMainWindow(object value) => mainWindow.Width = (double)value;

        public MainWindow()
        {
            InitializeComponent();
            settingsLoadTopLeftHeightWidth();
            Settings1 settings = new Settings1();
            Signals_Visible();
            Clock_Visible();
            ToolBar_Visible();
            Timer_0_Clock();
            SearchPorts(out ports);
            ToolBarComDeInit();
            terminalTx = new ObservableCollection<Terminal>(); TerminalTXList.ItemsSource = terminalTx;
            terminalRx = new ObservableCollection<Terminal>(); TerminalRXList.ItemsSource = terminalRx;
            this.DataContext = MyPort;
            btnStop = (bool)btnStop_rx.IsChecked;
           // comInitStruct.print_AppSettings();
            //Xbox.Text = Convert.ToString(X) ;
            //Ybox.Text = Convert.ToString(Y);
            //System.Windows.Controls.Button btn = new System.Windows.Controls.Button();
            //btn.Content = "Created with C#";
            //ContextMenu contextmenu = new();
            //btn.ContextMenu = contextmenu;
            ////btn.ContextMenuOpening = 0;
            //MenuItem mi     = new(){ Header = "File"           };
            //MenuItem mia    = new(){ Header = "New"            }; mi.Items.Add(mia);
            //MenuItem mib    = new(){ Header = "Open"           }; mi.Items.Add(mib);
            //MenuItem mib1   = new(){ Header = "Recently Opened"}; mib.Items.Add(mib1);
            //MenuItem mib1a  = new(){ Header = "Text.xaml"      }; mib1.Items.Add(mib1a);
            //mia.IsCheckable = true;
            //mib.IsCheckable = false;
            //mib1.IsCheckable = false;
            //mib1a.IsCheckable = true;
            //contextmenu.Items.Add(mi);
            //btn.ContextMenu = contextmenu;
            //gridToolBar.Children.Add(btn);
            ////////////////////var widthMainWindow = mainWindow.Width;
            ////////////////////X = (int)mainWindow.Width;
            ////////////////////Y = (int)mainWindow.Height;
            ////////////////////var xzzx = mainWindow.Title;
            ////////////////////xzzx += " X =" + X + "," + " Y =" + Y;
            ////////////////////mainWindow.Title = xzzx;
            ///

            //MessageBox.Show($"Port: { ConfigurationManager.AppSettings["Port"]} ,Boudrate: {ConfigurationManager.AppSettings["BoudRate"]}");

        }


        //public class SizeWindow
        //{
        //    int x;
        //    int y;
        //    // Создаем переменную делегата
        //    RAzmerOknaHandler? taken;
        //    public SizeWindow(int sum) => this.sum = sum;
        //    // Регистрируем делегат
        //    public void RegisterHandler(RAzmerOknaHandler del)
        //    {
        //        taken = del;
        //    }
        //    public void Add(int sum) => this.sum += sum;
        //    public void Take(int sum)
        //    {
        //        if (this.sum >= sum)
        //        {
        //            this.sum -= sum;
        //            // вызываем делегат, передавая ему сообщение
        //            taken?.Invoke($"Со счета списано {sum} у.е.");
        //        }
        //        else
        //        {
        //            taken?.Invoke($"Недостаточно средств. Баланс: {this.sum} у.е.");
        //        }
        //    }
        //}
        private void appSettingsSet(string str, double X)
        {
            //int Y = Convert.ToInt32(ConfigurationManager.AppSettings["Y"]);
            ConfigurationManager.AppSettings[str] = X.ToString();
        }
        private string appSettingsGet(string str)
        {
            string? result;
            result = ConfigurationManager.AppSettings[str];
            if (result == null) result = "";
            return result;
            //return Convert.ToInt32(ConfigurationManager.AppSettings["Y"]);
        }
        private void settingsSaveTopLeftHeightWidth()
        {
            settings.HeightMainWindow = mainWindow.Height;
            settings.WidthMainWindow = mainWindow.Width;
            settings.TopMainWindow = mainWindow.Top;
            settings.LeftMainWindow = mainWindow.Left;
            appSettingsSet("X", mainWindow.Top);
            appSettingsSet("Y", mainWindow.Left);
            settings.Save();
            //X = Convert.ToInt32(appSettingsGet("X"));
            //Y = Convert.ToInt32(appSettingsGet("Y"));
            //MessageBox.Show(X + "  " + Y);
        }
        private void settingsLoadTopLeftHeightWidth()
        {
            mainWindow.Height = settings.HeightMainWindow;
            mainWindow.Width = settings.WidthMainWindow;
            mainWindow.Top = settings.TopMainWindow;
            mainWindow.Left = settings.LeftMainWindow;
            //X = Convert.ToInt32(appSettingsGet("X"));
            //Y = Convert.ToInt32(appSettingsGet("Y"));
        }
        private void Timer_0_Clock()
        {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, e) => { tBlockClock.Text = DateTime.Now.ToString("HH:mm:ss"); };
            timer.Start();
        }
        private static void SearchPorts(out string[] ports) => ports = SerialPort.GetPortNames();
        private void ComPortInit()
        {
            //MyPort.PortName = ComInitStruct.pName;//  "COM7";
            //MyPort.BaudRate = ComInitStruct.baudRat;//  115200;
            //MyPort.Parity =   ComInitStruct.parity;// Parity.None ;//Parity.None
            //MyPort.DataBits = ComInitStruct.dBit;//  8;
            //MyPort.StopBits = ComInitStruct.sBit;//  StopBits.One;

            //MyPort = new(
            //              ConfigurationManager.AppSettings["Port"],
            //              Convert.ToInt32(ConfigurationManager.AppSettings["BoudRate"]),
            //              (Parity)StringToParity(ConfigurationManager.AppSettings["Parity"]),
            //              Convert.ToInt32(ConfigurationManager.AppSettings["Data"]),
            //              (StopBits)Convert.ToDouble(ConfigurationManager.AppSettings["Stop"]));


            MyPort.PortName = comInitStruct.ReadSetting( "Port");//  "COM7";
            MyPort.BaudRate = Convert.ToInt32(comInitStruct.ReadSetting("BoudRate"));//  115200;
            MyPort.Parity = (Parity)Convert.ToInt32(StringToParity(comInitStruct.ReadSetting("Parity")));// Parity.None ;//Parity.None
            MyPort.DataBits = Convert.ToInt32(comInitStruct.ReadSetting("Data"));//  8;
            MyPort.StopBits = (StopBits)Convert.ToDouble(comInitStruct.ReadSetting("Stop"));//  StopBits.One;
        }
        private void ToolBarComInit()
        {
            box1.IsEnabled = true;
            box2.IsEnabled = true;
            box3.IsEnabled = true;
            box4.IsEnabled = true;
            box5.IsEnabled = true;
            box1.Text = "порт: " + ComInitStruct.pName;//  "COM7";
            box2.Text = "скор.: " + ComInitStruct.baudRat.ToString();//  115200;
            box3.Text = "бит: " + ComInitStruct.dBit.ToString();//  8;
            box4.Text = "паритет: " + ComInitStruct.parity.ToString();// Parity.None ;//Parity.None
            box5.Text = "стоп бит: " + ComInitStruct.sBit.ToString();//  StopBits.One;
        }
        private void ToolBarComDeInit()
        {
            box1.Text = "порт:---";
            box2.Text = "скор.:---";
            box3.Text = "бит:---";
            box4.Text = "паритет:---";
            box5.Text = "стоп бит:---";
            box1.IsEnabled = false;
            box2.IsEnabled = false;
            box3.IsEnabled = false;
            box4.IsEnabled = false;
            box5.IsEnabled = false;
        }
        private void Exit_programm(object sender, RoutedEventArgs e)
        {

            MyPort.Close();
            comInitStruct.Save();
            this.Close(); // закрытие окна
        }
        private void Clear_rx(object sender, RoutedEventArgs e) { term.SetCountRx(0); terminalRx.Clear(); }
        private void Clear_tx(object sender, RoutedEventArgs e) { term.SetCountTx(0); terminalTx.Clear(); }
        private bool Close_port()
        {
            bool _port = true;
            toolbar_sostoyanie.Text = ComInitStruct.pName + " Отключен";
            try { MyPort.Close(); MyPort.DataReceived -= DataReceivedHandler; }
            catch (NullReferenceException) { toolbar_sostoyanie.Text = "CommPort не инициализирован"; /*MessageBox.Show("CommPort no Initialize!!!");*/  }
            finally { _port = false; }
            ToolBarComDeInit();
            return _port;
        }
        private bool Open_port()
        {
            int i = 0;
            bool _port = true;
            if (MyPort.IsOpen) { MyPort.Close(); MyPort.DataReceived -= DataReceivedHandler; }
            ComPortInit();
            ToolBarComInit();
            try
            {
                MyPort.Open();
                //if (MyPort.IsOpen() == true) { };
                MyPort.DataReceived += DataReceivedHandler;
                //MyPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                toolbar_sostoyanie.Text = MyPort.PortName + " Подключен";
            }
            catch (UnauthorizedAccessException) { i = 1; /*MessageBox.Show(comInitStruct.pName+" уже используется");*/ }
            catch (FileNotFoundException) { i = 2;/* MessageBox.Show(comInitStruct.pName + " Не существует");*/ }
            catch (System.IO.IOException){ i = 3; /*MessageBox.Show($"Превышен таймаут подключения к: {MyPort.PortName}");*/ }
            finally
            {
                if (i > 0) { _port = false; ToolBarComDeInit(); }
                if (i == 1) { toolbar_sostoyanie.Text = ComInitStruct.pName + " уже используется"; }
                else if (i == 2) { toolbar_sostoyanie.Text = ComInitStruct.pName + " Не существует"; }
                else if (i == 3) { toolbar_sostoyanie.Text = ComInitStruct.pName + " Превышен таймаут подключения"; }
            }
            return _port;
        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(25);
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            char[] delimiterCharss = { '\r', '\n' };
            string[] podstroki = indata.Split(delimiterCharss, StringSplitOptions.RemoveEmptyEntries);
            if (btnStop) { return; }
            foreach (var item in podstroki)
            {
                Dispatcher.Invoke(DispatcherPriority.Send, new UpTDelegate(ReadData), item);
            }
        }
        private void ReadData(string data)
        {
            string dataHex = stringToHex.ToHex(data);
            terminalRx.Add(new Terminal { Count = term.CountsRx(), Time = term.Tim(), HEX = dataHex, ASCII = data });
            //terminalRx.SelectedIndex = terminalRx.Items.Count - 1;
            //terminalRx.ScrollIntoView(terminalRx.SelectedItem);
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////
        private void send_tx_message(object sender, RoutedEventArgs e)
        {
            if (tBoxMessage_tx.Text == "") return;
            string data = tBoxMessage_tx.Text;
            string dataHex = stringToHex.ToHex(data);
            byte[] ba = Encoding.Default.GetBytes(data);
            try
            {
                MyPort.Write(ba, 0, ba.Length);
                terminalTx.Add(new Terminal { Count = term.CountsTx(), Time = term.Tim(), HEX = dataHex, ASCII = data });
            }
            catch (Exception) { string port = MyPort.PortName; MessageBox.Show("Шо то не так с " + port); }
        }
        private void btnConect_Checked(object sender, RoutedEventArgs e)
        {
            //toolbar_sostoyanie.Text = "кнопка конект нажата";
            btnDisConect.IsChecked = false;
            //comInitStruct.pName = tBoxMessage_tx.Text;
            if (!Open_port()) { btnConect.IsChecked = false; };
        }
        private void btnDisConect_Checked(object sender, RoutedEventArgs e)
        {
            //toolbar_sostoyanie.Text = "кнопка дисконект нажата";
            btnConect.IsChecked = false;
            Close_port();
        }

        //private void Exit_programm(object sender, MouseButtonEventArgs e) {
        //    settingsSaveTopLeftHeightWidth();
        //    try { Close_port(); }
        //    catch { System.Windows.MessageBox.Show("error close port"); }
        //}
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            settingsSaveTopLeftHeightWidth();
            try { Close_port(); }
            catch { System.Windows.MessageBox.Show("error close port"); }
            // this.Close(); // закрытие окна
            //const string message = "Are you sure that you would like to close the form?";
            //const string caption = "Form Closing";
            //var result = System.Windows.Forms.MessageBox.Show(message, caption,MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            //if (result == System.Windows.Forms.DialogResult.No) { e.Cancel = true; }
            //else { e.Cancel = false; }
        }
        private void Terminal_Settings(object sender, RoutedEventArgs e)
        {
            Settings win = new Settings();
            //win.ShowDialog();
            win.ShowDialog();
        }
        private void ComPortSettings(object sender, RoutedEventArgs e)
        {
            ComPortXAML comXAML = new();
            // var a = PointToScreen(new Point(Left, Top)).ToString();
            comXAML.ShowDialog();
        }
        private void Box_Click_Port(object sender, MouseButtonEventArgs e)
        {
            SearchPorts(out ports);
            ContextMenu? mPort = this.FindResource("mComPort") as ContextMenu;
            mPort.PlacementTarget = sender as System.Windows.Controls.Button;
            mPort.FontSize = 10;
            mPort.Name = "ComPortSearch";
            mPort.Items.Clear();
            mPort.ItemsSource = ports;
            mPort.IsOpen = true;
        }
        private void Box_Click_Boudrate(object sender, MouseButtonEventArgs e)
        {
            //ContextMenu? mBod = this.FindResource("mBoudrate") as ContextMenu;
            //mBod.PlacementTarget = sender as System.Windows.Controls.Button;
            ContextMenu mBod = new ContextMenu();
            foreach(var b in arrBoudRate) {mBod.Items.Add(b); }
            mBod.PlacementTarget = sender as System.Windows.Controls.Button;
            mBod.IsOpen = true;
        }
        private void Box_Click_DataBit(object sender, MouseButtonEventArgs e)
        {
            ContextMenu? mData = this.FindResource("mDataBit") as ContextMenu;
            mData.PlacementTarget = sender as Button;
            mData.IsOpen = true;
        }
        private void Box_Click_Parity(object sender, MouseButtonEventArgs e)
        {
            ContextMenu? mParity = this.FindResource("mParityBit") as ContextMenu;
            mParity.PlacementTarget = sender as System.Windows.Controls.Button;
            mParity.IsOpen = true;
        }
        private void Box_Click_StopBit(object sender, MouseButtonEventArgs e)
        {
            ContextMenu? mStop = this.FindResource("mStopBit") as ContextMenu;
            mStop.PlacementTarget = sender as System.Windows.Controls.Button;
            mStop.IsOpen = true;
        }
        //private void menuItem1_Click(object sender, System.EventArgs e)
        //{
        //    // Create a new OpenFileDialog and display it.
        //    OpenFileDialog fd = new OpenFileDialog();
        //    fd.DefaultExt = "*.*";
        //    fd.ShowDialog();
        //}
        private void Button_Click_Clock_Visible(object sender, RoutedEventArgs e)
        {
            if (settings.Panel_Clock){ settings.Panel_Clock = false;settings.Save();}
            else{settings.Panel_Clock = true;settings.Save();}
            Clock_Visible();
        }
        void Clock_Visible()
        {
            this.SatusBarClock.Visibility = settings.Panel_Clock ? Visibility.Collapsed : Visibility.Visible;
            checkClock.IsChecked = !settings.Panel_Clock;
        }
        private void Button_Click_ToolBar(object sender, EventArgs e)
        {
            if (settings.Panel_ToolBar) { settings.Panel_ToolBar = false;settings.Save();}
            else { settings.Panel_ToolBar = true;settings.Save();}
            ToolBar_Visible();
        }
        void ToolBar_Visible()
        {
            if (!settings.Panel_ToolBar) { StecPanel_ToolBar.Visibility = Visibility.Hidden; mainWindow.Background = StecPanel_ToolBar.Background;checkToolBar.IsChecked = settings.Panel_ToolBar; }
            else StecPanel_ToolBar.Visibility = Visibility.Visible; checkToolBar.IsChecked = settings.Panel_ToolBar;
        }
        private void Button_Click_Signals(object sender, EventArgs e)
        {
            if(settings.Panel_Signals){settings.Panel_Signals = false;settings.Save();}
            else { settings.Panel_Signals = true;settings.Save(); }
            Signals_Visible();
        }
        void Signals_Visible()
        {
            if (settings.Panel_Signals) { gridSignals.Height = 25; gridSignals.Visibility = Visibility.Visible; checkSignals.IsChecked = settings.Panel_Signals; }
            else { gridSignals.Height = 0; gridSignals.Visibility = Visibility.Hidden; mainWindow.Height = 445; checkSignals.IsChecked = settings.Panel_Signals; }
        }
        private void Button_Rx_Data(object sender, EventArgs e)
        {
            var x = GetWidthMainWindow();
            double widthStorona = 0;
            if ((double)x > 304) widthStorona = (double)x / 2;

            if (Rx_Data.IsChecked == true) { TerminalRXList.Visibility = Visibility.Visible; TerminalRXList.Width = widthStorona; }
            else { TerminalRXList.Visibility = Visibility.Hidden; TerminalRXList.Width = 0; TerminalTXList.Width += widthStorona; }
        }
        private void Button_Tx_Data(object sender, EventArgs e)
        {
            var x = GetWidthMainWindow();
            double widtthStorona = 0;
            widtthStorona = (double)(x) / 2;
            if (Tx_Data.IsChecked == true) { TerminalTXList.Visibility = Visibility.Visible; TerminalTXList.Width = widtthStorona; }
            else { TerminalTXList.Visibility = Visibility.Hidden; TerminalTXList.Width = 0; TerminalRXList.Width += widtthStorona; }
        }
        //private void menu1(object sender, RoutedEventArgs e)
        //{
        //    var a = (MenuItem)sender;
        //    string x = (string)a.Header;
        //    bool n = a.IsChecked;
        //    string xz = n.ToString();
        //    System.Windows.MessageBox.Show(x + "\r\n" + n);
        //    //b.IsChecked = true;
        //}
        //private void menu2(object sender, RoutedEventArgs e)
        //{
        //    var b = (MenuItem)sender;
        //    string x = (string)b.Header;
        //    System.Windows.MessageBox.Show(x);
        //    //b.IsChecked = true;
        //}
        //private void menu3(object sender, RoutedEventArgs e)
        //{
        //    var c = (MenuItem)sender;
        //    string x = (string)c.Header;
        //    System.Windows.MessageBox.Show(x);
        //    //b.IsChecked = true;
        //}
        private void TerminalTXList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Terminal p = (Terminal)TerminalTXList.SelectedItem;
            MessageBox.Show(p.Count);
        }
        private void TerminalRXList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Terminal p = (Terminal)TerminalRXList.SelectedItem;
            MessageBox.Show(p.Count);
        }
        private void btn_Stop_Rx_Check(object sender, RoutedEventArgs e)
        {
            if (btnStop_rx.IsChecked == false) { btnStop = false; }
            if (btnStop_rx.IsChecked == true) { btnStop = true; }
        }
        private void CopyRx(object sender, RoutedEventArgs e)
        {
            //Terminal p = (Terminal)TerminalRXList.SelectedItems;
            //if (p != null) { string copy = p.Count + " " + p.Time + " " + p.HEX + " " + p.ASCII; MessageBox.Show(copy); }
            //try {string copy = p.Count + " " + p.Time + " " + p.HEX + " " + p.ASCII; MessageBox.Show(copy); }
            //catch(NullReferenceException) {MessageBox.Show("потерян фокус"); }

            //foreach (Terminal item in TerminalRXList.SelectedItems) {
            //    //listView2.Items.Add(new ListViewItem(new string[] { item.Text, litem.SubItems[1].Text, "в очереди", item.SubItems[5].Text, item.SubItems[3].Text, item.SubItems[4].Text }))
            //    listView2.Items.Add(new ListViewItem(new string[] { item.Count, item.SubItems[1].Time, "в очереди", item.SubItems[5].Text, item.SubItems[3].Text, item.SubItems[4].Text }));
            //
            //}

            List<string> arr = new List<string>();
            for (int i = 0; i < TerminalRXList.SelectedItems.Count; i++)
            {
                Terminal item = (Terminal)TerminalRXList.SelectedItems[i];
                arr.Add(item.Count + " " + item.Time + " " + item.HEX + " " + item.ASCII);
            }

            try
            {
                StreamWriter sw = new StreamWriter("D:\\Test.txt");
                for (int i = 0; i < arr.Count; i++) { sw.WriteLine(arr[i]); }
                sw.Close();
            }
            catch (Exception ex) { Console.WriteLine("Exception: " + ex.Message); }
            finally { Console.WriteLine("Executing finally block."); }
        }
        private void O_Programme_Click(object sender, RoutedEventArgs e) => MessageBox.Show("   Не до реплика ComPort ToolKit");

        public int StringToParity(string str)
        {
            //string[] arrStr = new[]{ "q"};
            int i = 0;
            if (str != null)
            {
                for (; i < arrParitet.Length; i++)
                {
                    if (Equals(arrParitet[i], str)) { return i; };
                }
            }
            return i = 0;
        }
        private string[] arrBoudRate = new[] { "110", "300", "600", "1200", "2400", "4800", "9600", "14400", "19200", "38400", "56000", "57600", "115200", "128000", "256000" };
        private string[] arrBit = new[] { "5", "6", "7", "8" };
        private string[] arrParitet = new[] { "нет.", "нечёт.", "чёт.", "марк.", "пробел" };
        private string[] arrStop = new[] { "1", "1.5", "2" };

    }
    
    public class StringToHex
    {
        private string hexString = "";
        public string ToHex(string input)
        {
            hexString = "";
            byte[] ba = Encoding.Default.GetBytes(input);
            hexString = BitConverter.ToString(ba);
            hexString = hexString.Replace("-", " ");
            return hexString;
        }
    }
    public class Terminal
    {
        private static int countRx = 0;
        private static int countTx = 0;

        public string? Count { get; set; }
        public string? Time { get; set; }

        public required string HEX { get; set; }
        public required string ASCII { get; set; }
        public void SetCountRx(int c) { countRx = c; }
        public void SetCountTx(int c) { countTx = c; }
        //public int GetCount() { return count; }
        public string CountsRx() { countRx++; return countRx.ToString(); }
        public string CountsTx() { countTx++; return countTx.ToString(); }
        public string Tim() { return DateTime.Now.ToString("HH:mm:ss:fff"); }
    }
    static class Data
    {
        public static string Value { get; set; }
    }
    public class ComSearch_
    {
        public required Image Img { get; set; }
        public required string? PorT { get; set; }
        public required string? StatuS { get; set; }
    }
    public class ComStruct
    {
        private static string? portName;//COM1
        private static int baudRate;//   = 115200;       //115200
        private static Parity parityBits;//  = Parity.None;  //None
        private static int dataBits;//    = 8;            //5,6,7,8 
        private static StopBits stopBits;//   = StopBits.One; //One

        //public ComStruct(string str = "COM0", int bod = 115200, Parity x = Parity.None, int dBit = 8, StopBits y = StopBits.One)
        //{
        //    portName = str;//"COM7";
        //    baudRate = bod;//115200;
        //    parityBits = x;//Parity.None;
        //    dataBits = dBit;//8;
        //    stopBits = y;//StopBits.One;
        //}

        static ComStruct()
        {

            //ConfigurationManager.AppSettings["Port"] = "COM3";
            //ConfigurationManager.AppSettings["BoudRate"] = "115233";
            portName = ConfigurationManager.AppSettings["Port"];
            baudRate = Convert.ToInt32(ConfigurationManager.AppSettings["BoudRate"]);
            parityBits = (Parity)Convert.ToInt32(ConfigurationManager.AppSettings["Parity"]);
            dataBits = Convert.ToInt32(ConfigurationManager.AppSettings["Data"]);
            stopBits = (StopBits)Convert.ToInt32(ConfigurationManager.AppSettings["Stop"]);

            //pName = ConfigurationManager.AppSettings["Port"];
            //baudRat = Convert.ToInt32(ConfigurationManager.AppSettings["BoudRate"]);
            //parity = (Parity)Convert.ToInt32(ConfigurationManager.AppSettings["Parity"]);
            //dBit = Convert.ToInt32(ConfigurationManager.AppSettings["Data"]);
            //sBit = (StopBits)Convert.ToInt32(ConfigurationManager.AppSettings["Stop"]);

            //comInitStruct.pName = ConfigurationManager.AppSettings["Port"];
            //comInitStruct.baudRat = Convert.ToInt32(ConfigurationManager.AppSettings["BoudRate"]);
            //comInitStruct.parity = (Parity)Convert.ToInt32(ConfigurationManager.AppSettings["Parity"]);
            //comInitStruct.dBit = Convert.ToInt32(ConfigurationManager.AppSettings["Data"]);
            //comInitStruct.sBit = (StopBits)Convert.ToInt32(ConfigurationManager.AppSettings["Stop"]);

        }
        public string pName { get { return portName; } set { portName = value; } }
        public int baudRat { get { return baudRate; } set { baudRate = value; } }
        public Parity parity { get { return parityBits; } set { parityBits = value; } }
        public int dBit { get { return dataBits; } set { dataBits = value; } }
        public StopBits sBit { get { return stopBits; } set { stopBits = value; } }

        public void print()
        {
            MessageBox.Show(portName + "\r\n" + baudRate + "\r\n" + parityBits + "\r\n" + dataBits + "\r\n" + stopBits);
        } 
        public void print_AppSettings()
        {
            //MessageBox.Show($"{ReadSetting("Port")} + "\r\n" + {ReadSetting("BoudRate")} + "\r\n" + {ReadSetting("Parity")} + "\r\n" + {ReadSetting("Data")} + "\r\n" + {ReadSetting("Stop")}");
            MessageBox.Show($" Port:          {ReadSetting("Port")} \r\n BoudRAte: {ReadSetting("BoudRate")} \r\n Parity:        {ReadSetting("Parity")} \r\n DataBits:    {ReadSetting("Data")} \r\n StopBits:    {ReadSetting("Stop")}");
        }

        public void Save()
        {
            AddUpdateAppSettings("Port", portName);
            AddUpdateAppSettings("BoudRate", baudRate.ToString());
            AddUpdateAppSettings("Parity", parityBits.ToString());
            AddUpdateAppSettings("Data", dataBits.ToString());
            AddUpdateAppSettings("Stop", stopBits.ToString());
            
        }

       public void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
       public string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                //Console.WriteLine(result);
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                //Console.WriteLine("Error reading app settings");
                MessageBox.Show("Error reading app settings");
            }
            return "";
        }



        //private enum Parity
        //{
        //    None = 0,
        //    Odd = 1,
        //    Even = 2,
        //    Mark = 3,
        //    Space = 4,
        //}
        //private enum DataBits
        //{
        //    five = 5,
        //    six = 6,
        //    seven = 7,
        //    eight = 8,
        //}

        //private enum StopBits
        //{
        //    None = 0,
        //    One = 1,
        //    Two = 2,
        //    OnePointFive = 3,
        //}
    }
    public class Com : ComStruct
    {
        public required string Name { get; set; } = "";
        public required string BoudRate { get; set; }
        public required string PortName { get; set; }
        public required string Parity { get; set; }
        public required string DBit { get; set; }
        public required string SBit { get; set; }

        //public bool isChecked { get; set; } = false;
        public override string ToString() => $"{Name}";
    }
    //class MainVM 
    //{
    //    double x, y;
    //    public double X { get => x; set => Set(ref x, value); }
    //    public double Y { get => y; set => Set(ref y, value); }
    //    public ICommand Run { get; }

    //    async void Move()
    //    {
    //        var centerX = X;
    //        var centerY = Y;
    //        var R = 50;
    //        while (true)
    //        {
    //            const double delta = Math.PI * 10 / 180;
    //            for (double angle = 0; angle < 2 * Math.PI; angle += delta)
    //            {
    //                X = centerX + R * Math.Cos(angle);
    //                Y = centerY + R * Math.Sin(angle);
    //                await Task.Delay(TimeSpan.FromMilliseconds(50));
    //            }
    //        }
    //    }

    //    public MainVM() { Run = new RelayCommand(o => Move()); }
    //}
}



