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
using System.Windows.Media;
using System.Windows.Threading;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;
 
namespace comPort_wpf
{
    delegate void dMessge();
    public partial class MainWindow : Window
    {
       
        public delegate void Mydelegate(); 
        private static Settings1 settings = new();
        private string[] ports = new[] { "" };
        public static ComStruct comInitStruct = new();
        public static SerialPort MyPort = new();
        public ContextMenu? mPortContextMenu = new ContextMenu();
        private delegate void UpTDelegate(string text);
        public Terminal term = new Terminal() { HEX = "", ASCII = "" };
        public StringToHex stringToHex = new StringToHex();
        bool btnStop = false;


        ObservableCollection<string> arrBoudRateObserv;
        ObservableCollection<string> arrParityObserv;
        ObservableCollection<string> arrDataObserv;
        ObservableCollection<string> arrStopObserv;
        ObservableCollection<string> arrPortNameObserv;
        //bool btnVisibleClock = true;

        // public ComSearch_ comsearch = new ();
        //string[] arrBoudRate = new[] { "110", "300", "600", "1200", "2400", "4800", "9600", "14400", "19200", "38400", "56000", "57600", "115200", "128000", "256000" };
        //string[] arrBit =      new[] { "5","6","7","8" };
        //string[] arrParitet =  new[] { "нет.","нечёт.","чёт.","марк.","пробел" };
        //string[] arrStop =     new[] { "1","1.5","2" };
        ////public static readonly DependencyProperty ComProperty;
        public ObservableCollection<Terminal> terminalTx { get; set; }
        public ObservableCollection<Terminal> terminalRx { get; set; }
        //public ObservableCollection<ComSearch_> listViewCom { get; set; }
        //public ObservableCollection<DBit> dBits { get; set; }
           // Binding binding;
        public static ComStruct ComInitStruct { get => comInitStruct; set => comInitStruct = value; }
        
        //List<Terminal> p=new List<Terminal>();
        ObservableCollection<Terminal> collection = new ObservableCollection<Terminal>();
        public double GetWidthMainWindow() => mainWindow.Width;
        public void SetWidthMainWindow(object value) => mainWindow.Width = (double)value;

        public MainWindow()
        {
            InitializeComponent();
            //DataContext = new ApplicationViewModel(new DefaulDialogService(), new JsonFileService());
            settingsLoadTopLeftHeightWidth();
            //Settings1 settings = new Settings1();
            Signals_Visible();
            Clock_Visible();
            ToolBar_Visible();
            Timer_0_Clock();
            SearchPorts(out ports);
            ToolBarComDeInit();
            terminalTx = new ObservableCollection<Terminal>(); TerminalTXList.ItemsSource = terminalTx;
            terminalRx = new ObservableCollection<Terminal>(); TerminalRXList.ItemsSource = terminalRx;
            //this.DataContext = MyPort;
            btnStop = (bool)btnStop_rx.IsChecked;

            //TerminalRXList.

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

            SearchPorts(out ports);
            arrPortNameObserv = new(ports);        cBoxPortName.ItemsSource = arrPortNameObserv;// cBoxPortName.SelectedIndex = 0;  cBoxPortName.IsEnabled = false; 
            arrBoudRateObserv = new(arrBoudRate);  cBoxBoudRate.ItemsSource = arrBoudRateObserv;// cBoxBoudRate.SelectedIndex = 12; cBoxBoudRate.IsEnabled = false;
            arrDataObserv     = new(arrBit);       cBoxData.ItemsSource     = arrDataObserv;    // cBoxData.SelectedIndex = 3;      cBoxData.IsEnabled = false;
            arrParityObserv   = new(arrParitet);   cBoxParity.ItemsSource   = arrParityObserv;  // cBoxParity.SelectedIndex = 0;    cBoxParity.IsEnabled = false;
            arrStopObserv     = new(arrStop);      cBoxStop.ItemsSource     = arrStopObserv;    // cBoxStop.SelectedIndex = 0;      cBoxStop.IsEnabled = false;
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

        private void settingsSaveTopLeftHeightWidth()
        {
            settings.HeightMainWindow = mainWindow.Height;
            settings.WidthMainWindow = mainWindow.Width;
            settings.TopMainWindow = mainWindow.Top;
            settings.LeftMainWindow = mainWindow.Left;
            //appSettingsSet("X", mainWindow.Top);
            //appSettingsSet("Y", mainWindow.Left);
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
        public void ComPortInit()
        {
            //MyPort.PortName = ComStruct.ReadSetting("Port");//  "COM7";
            //MyPort.BaudRate = Convert.ToInt32(ComStruct.ReadSetting("BoudRate"));//  115200;
            //MyPort.Parity =   (Parity)Convert.ToInt32(StringToParity(ComStruct.ReadSetting("Parity")));// Parity.None ;//Parity.None
            //MyPort.DataBits = Convert.ToInt32(ComStruct.ReadSetting("Data"));//  8;
            //MyPort.StopBits = (StopBits)StringToStop(ComStruct.ReadSetting("Stop"));//  StopBits.One;

            MyPort.PortName = settings.portName;//  "COM7";
            MyPort.BaudRate = settings.BoudRate;//  115200;
            MyPort.Parity =   (Parity)Convert.ToInt32(StringToParity(settings.Parity));// Parity.None ;//Parity.None
            MyPort.DataBits = Convert.ToInt32(settings.Data);//  8;
            MyPort.StopBits = (StopBits)StringToStop(settings.Stop);//  StopBits.One;
        }
        private void ToolBarComInit()
        {
            //cBoxPortName.IsEnabled = true; cBoxPortName.Text = "порт: " + MyPort.PortName;//  "COM7";
            //cBoxBoudRate.IsEnabled = true; cBoxBoudRate.Text = "скор.: " + MyPort.BaudRate.ToString();//  115200;
            //cBoxData.IsEnabled =     true; cBoxData.Text = "бит: " + MyPort.DataBits.ToString();//  8;
            //cBoxParity.IsEnabled =   true; cBoxParity.Text = "паритет: " + MyPort.Parity.ToString();// Parity.None ;//Parity.None
            //cBoxStop.IsEnabled =     true; cBoxStop.Text = "стоп бит: " + MyPort.StopBits.ToString();//  StopBits.One;
            cBoxPortName.IsEnabled = true;  cBoxPortName.Text = "порт: " + settings.portName   ;//  "COM7";
            cBoxBoudRate.IsEnabled = true;  cBoxBoudRate.Text = "скор.: " + settings.BoudRate ;//  115200;
            cBoxData.IsEnabled = true;      cBoxData.Text = "бит: " + settings.Data ;//  8;
            cBoxParity.IsEnabled = true;    cBoxParity.Text = "паритет: " + settings.Parity ;// Parity.None ;//Parity.None
            cBoxStop.IsEnabled = true;      cBoxStop.Text = "стоп бит: " + settings.Stop ;//  StopBits.One;
        }
        private void ToolBarComDeInit()
        {
            //cBoxPortName.Text = "порт:---";     cBoxPortName.IsEnabled = false;
            //cBoxBoudRate.Text = "скор.:---";    cBoxBoudRate.IsEnabled = false;
            //cBoxData.Text     = "бит:---";      cBoxData.IsEnabled = false;
            //cBoxParity.Text   = "паритет:---";  cBoxParity.IsEnabled = false;
            //cBoxStop.Text     = "стоп бит:---"; cBoxStop.IsEnabled = false;
        }
        private void Exit_programm(object sender, RoutedEventArgs e)
        {
            ComStruct.AddUpdateAppSettings("Port",MyPort.PortName);
            ComStruct.AddUpdateAppSettings("BoudRate",MyPort.BaudRate.ToString());
            ComStruct.AddUpdateAppSettings("Parity", MyPort.Parity.ToString());
            ComStruct.AddUpdateAppSettings("Data", MyPort.DataBits.ToString());
            ComStruct.AddUpdateAppSettings("Stop", MyPort.StopBits.ToString());
            MyPort.Close();
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
           // ComPortInit();
            int i = 0;
            bool _port = true;
            if (MyPort.IsOpen) { MyPort.Close(); MyPort.DataReceived -= DataReceivedHandler; }
            ComPortInit();
            ToolBarComInit();
            //string x;
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
            catch (System.ArgumentOutOfRangeException e) { i = 4; MessageBox.Show(e.ToString());  }
            finally
            {
                if (i > 0) { _port = false; ToolBarComDeInit(); }
                if (i == 1) { toolbar_sostoyanie.Text = ComInitStruct.pName + " уже используется"; }
                else if (i == 2) { toolbar_sostoyanie.Text = ComInitStruct.pName + " Не существует"; }
                else if (i == 3) { toolbar_sostoyanie.Text = ComInitStruct.pName + " Превышен таймаут подключения"; }
                else if (i == 4) { toolbar_sostoyanie.Text = ComInitStruct.pName + " Указана несовместимая скорость Бод"; }
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
            btnConect.IsChecked = false;
            Close_port();
        }
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
            win.ShowDialog();
        }
        private void ComPortSettings(object sender, RoutedEventArgs e)
        {
            ComPortXAML comXAML = new();
            comXAML.ShowDialog();
        }
        private void Box_Click_Port(object sender, MouseButtonEventArgs e)
        {
            SearchPorts(out ports);
            //ContextMenu? mPort = this.FindResource("mComPort") as ContextMenu;
            ContextMenu? mPort = new ContextMenu();

            mPort.PlacementTarget = sender as System.Windows.Controls.Button;
            mPort.FontSize = 10;
            mPort.Name = "ComPortSearch";
            //mPort.ItemsSource.GetEnumerator().MoveNext();
            mPort.Items.Clear();
            mPort.ItemsSource = ports;
            mPort.IsOpen = true;
        }
        private void Box_Click_Boudrate(object sender, MouseButtonEventArgs e)
        {
            
        }
        private void Box_Click_DataBit(object sender, MouseButtonEventArgs e)
        {
            
        }
        private void Box_Click_Parity(object sender, MouseButtonEventArgs e)
        {
            
        }
        private void Box_Click_StopBit(object sender, MouseButtonEventArgs e)
        {
           
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
            else {                        gridSignals.Height = 0;  gridSignals.Visibility = Visibility.Hidden;  checkSignals.IsChecked = settings.Panel_Signals; }
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

            Terminal[] items = new Terminal[TerminalTXList.SelectedItems.Count];
            TerminalTXList.SelectedItems.CopyTo(items, 0);
            List<Terminal> tb3 = new List<Terminal>(items);
            
            //collection = (ObservableCollection<Terminal>)TerminalTXList.SelectedItems;
            //    if (collection != null)
            //    {
            //collection.Add(p);
            //MessageBox.Show(p);
            //    }
            //}

            //foreach (var item in p)
            //{
            //    if (p != null)
            //    {
            //        collection.Add(p);
            //        MessageBox.Show(p.Count);
            //    }

            //}
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
            int i = 0;
            if (str != null)
            {
                for (; i < arrParitet.Length; i++)
                {
                    if (Equals(arrParitet[i], str) || (Equals(arrParitetEng[i], str)) ) return i;
                }
            }
            return i = 0;
        }
        private string ParityToString(int p)
        {
            if (p <= arrParitet.Length) { return arrParitet[p]; }
            return "";
        }
        private int StringToStop(string str)
        {
            int i = 0;
            double d = 1;
            if (str != null)
            {
                for (; i < arrStop.Length; i++)
                {
                    if (Equals(arrStop[i], str) || Equals(arrStopEng[i], str))return i+=1;
                }
            }
            return i;
        }
        private string[] arrBoudRate = new[] { "110", "300", "600", "1200", "2400", "4800", "9600", "14400", "19200", "38400", "56000", "57600", "115200", "128000", "256000" };
        private string[] arrBit = new[] { "5", "6", "7", "8" };
        private string[] arrParitet = new[] { "нет.", "нечёт.", "чёт.", "марк.", "пробел" };
        private string[] arrParitetEng = new[] { "None", "Odd", "Even", "Mark", "Space" };
        private string[] arrStop = new[] { "1",  "2","1.5" };
        private string[] arrStopEng = new[] { "One",  "Two","OnePointFive" };
        private Delegate enter;
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
       public Terminal() { }
        //Terminal(string? count, string? time, string hEX, string aSCII)
        //{
        //    Count = count;
        //    Time = time;
        //    HEX = hEX;
        //    ASCII = aSCII;
        //}

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
    //public class DBit
    //{
    //    public static string Value { get; set; }
    //}
    //static class Data
    //{
    //    public static string Value { get; set; }
    //}

    public class ListWidth
    {
        private static double count;
        private static double time;
        private static double ascii;
        private static double hex;
        public double Count { get { return count; } set { count = value; } }
        public double Time { get { return time; } set { time = value; } }
        public double ASCII { get { return ascii; } set { ascii = value; } }
        public double HEX { get { return hex; } set { hex = value; } }
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
        private static int parityBits;//  = Parity.None;  //None
        private static int dataBits;//    = 8;            //5,6,7,8 
        private static StopBits stopBits;//   = StopBits.One; //One

         static ComStruct()
        {
            //portName = ComStruct.ReadSetting("Port");
            //baudRate = Convert.ToInt32(ReadSetting("BoudRate"));
            //dataBits = Convert.ToInt32(ReadSetting("Data"));
            //parityBits = Convert.ToInt32(ReadSetting("Parity"));
            //stopBits = (StopBits)Convert.ToDouble(ReadSetting("Stop"));

            //portName = Zaglushka.ReadSetting("Port");
            //baudRate = Convert.ToInt32(Zaglushka.baudRate);
            //parityBits = Convert.ToInt32(Zaglushka.parityBits);
            //dataBits = Convert.ToInt32(Zaglushka.dataBits);
            //stopBits = (StopBits)Convert.ToDouble(Zaglushka.stopBits);

        }
        public string pName { get { return portName; } set { portName = value; } }
        public int baudRat { get { return baudRate; } set { baudRate = value; } }
        public Parity parity { get { return (Parity)parityBits; } set { parityBits = Convert.ToInt32(value); } }
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

        public static void AddUpdateAppSettings(string key, string value)
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
        public static string ReadSetting(string key)
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
    //public class Com : ComStruct
    //{
    //    public required string PortName { get; set; }
    //    public required int BoudRate { get; set; }
    //    public required int Parity { get; set; }
    //    public required int DBit { get; set; }
    //    public required double SBit { get; set; }
    //}
    public static class Zaglushka {
        public static string? portName { get;  set; }
        public static string? baudRate { get;  set; }
        public static string? parityBits { get;  set; }
        public static string? dataBits { get;  set; }
        public static StopBits stopBits { get;  set; }

       
        //public static void Save()
        //{
        //    AddUpdateAppSettings("Port", portName);
        //    AddUpdateAppSettings("BoudRate", baudRate.ToString());
        //    AddUpdateAppSettings("Parity", parityBits.ToString());
        //    AddUpdateAppSettings("Data", dataBits.ToString());
        //    AddUpdateAppSettings("Stop", stopBits.ToString());
        //}

        public static void AddUpdateAppSettings(string key, string value)
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
        public static string ReadSetting(string key)
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



