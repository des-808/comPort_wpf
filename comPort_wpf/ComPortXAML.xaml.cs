using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO.Ports;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using static comPort_wpf.MainWindow;

namespace comPort_wpf
{
        
    public partial class ComPortXAML : Window
    {
        dMessge messsage;
        private string[] ports;
        private ObservableCollection<ComSearch_> listViewCom;
        private int comSelectedValue = -1;
        ComSearch_ search_Com;
        string stroka = "";
        public static SerialPort? MyPort = new("COM0", 115200,Parity.None,8, StopBits.One);
        Thread writeThread = new Thread(Write);
        static bool _continue = true;
        // Объявляем событие Sobitie на основе делегата
        //public event Mydelegate Sobitie;
        private Settings1 settings = new Settings1();
        private ListBox listBoxCom = new ListBox();
        public ComPortXAML()
        {
            InitializeComponent();
            settingsLoadTopLeft();
            //CanvasVisiblity.Visibility = (settings.ComSearchWindow)? Visibility.Hidden:Visibility.Visible;
            ports_window_open_cloce_func();
            ports = SerialPort.GetPortNames();
            listViewCom = new ObservableCollection<ComSearch_>(); ListViewCom_.ItemsSource = listViewCom;

            
            foreach (string b in ports) { PortBox.Items.Add(b); }
            foreach (string b in arrBoudRate) { BoudBox.Items.Add(b); }
            foreach (string b in arrBit) { DataBox.Items.Add(b); }
            foreach (string b in arrParitet) { ParityBox.Items.Add(b); }
            foreach (string b in arrStop) { StopBox.Items.Add(b); }
            
            PortBox.Text = ComStruct.ReadSetting("Port");
            BoudBox.Text = ComStruct.ReadSetting("BoudRate");
            DataBox.Text = ComStruct.ReadSetting("Data");
            ParityBox.Text = perevodchikParitet (ComStruct.ReadSetting("Parity"));
            StopBox.Text = perevodchikStop(ComStruct.ReadSetting("Stop"));
            //MyPort = new(ComInitStruct.pName, ComInitStruct.baudRat, (Parity)ComInitStruct.parity, ComInitStruct.dBit, (StopBits)ComInitStruct.sBit);
        }
        public string perevodchikParitet(string str)
        {
            for (int i = 0; i < arrParitetEng.Length; i++) {
                if (Equals(arrParitetEng[i], str)) str = arrParitet[i]; 
            }
            return str;
        }

        public string perevodchikStop(string str)
        {
            for (int i = 0; i < arrStopEng.Length; i++)
            {
                if (Equals(arrStopEng[i], str)) str = arrStop[i];
            }
            return str;
        }

        //Cоздаем метод для события, который просто будет обращаться к событию
        public void MetoddlyaSobitiya()
        {
            //Можно вставить проверку наличия события
            //if (Sobitie !=null)
            //Sobitie();
        }
        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    settings.Save();
        //    base.OnClosing(e);
        //}

        private void BtnEnter(object sender, RoutedEventArgs e)
        {
            settingsSaveTopLeft();
            ComStruct.AddUpdateAppSettings("Port", PortBox.Text);
            ComStruct.AddUpdateAppSettings("BoudRate", BoudBox.Text.ToString());
            ComStruct.AddUpdateAppSettings("Parity", ParityBox.Text.ToString());
            ComStruct.AddUpdateAppSettings("Data", DataBox.Text.ToString());
            ComStruct.AddUpdateAppSettings("Stop", StopBox.Text.ToString());
            Close();
        }
        public int StringToParity(string str)
        {
            int i = 0;
            if (str != null)
            {
                for (; i < arrParitet.Length; i++)
                {
                    if (Equals(arrParitet[i], str) || (Equals(arrParitetEng[i], str))) return i;
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
                    if (Equals(arrStop[i], str) || Equals(arrStopEng[i], str)) return i += 1;
                }
            }
            return i;
        }
        private void BtnCancel(object sender, RoutedEventArgs e)
        {
            settingsSaveTopLeft();
            Close();
        }
        private void settingsSaveTopLeft()
        {
            settings.TopComPortSet = window_settings_comPort_tools.Top;
            settings.LeftComPortSet = window_settings_comPort_tools.Left;
            settings.Save();
        }
        private void settingsLoadTopLeft()
        {
            window_settings_comPort_tools.Top = settings.TopComPortSet;
            window_settings_comPort_tools.Left = settings.LeftComPortSet;
        }
        private void Rescan_ComPort(object sender, RoutedEventArgs e)
        {
            listViewCom.Clear();
            SearchPorts(out ports);
            //PortBox.Items.Clear();
            string Status = "";
            Image? icon = null;
            string? error;
            MyPort.ReadTimeout = 200;//500
            MyPort.WriteTimeout = 200;
            foreach (string p in ports)
            {
                // Port = p.ToString();
                MyPort.PortName = p;
                try { MyPort.Open(); }
                catch (System.IO.IOException ev) { Status = "Не доступен "; error = ev.ToString(); goto metka; }
                _continue = true;
                if (writeThread.IsAlive) { writeThread.Start(); }
                //else { writeThread. }
                //try {//if (MyPort.IsOpen == true) { Status = "Порт открыт "; }else { Status = "Порт не в работе "; }
                //}    catch(Exception ex) { MessageBox.Show(ex.ToString());}
                Status = MyPort.IsOpen ? "Порт открыт " : "Порт не в работе ";
                //writeThread.Join();

                writeThread.Interrupt();
            metka:
                MyPort.Close();
                PortBox.Items.Clear();
                foreach (string v in ports) { PortBox.Items.Add(v); }
                listViewCom.Add(new ComSearch_ { Img = icon, PorT = p, StatuS = Status });
            }
            //writeThread.Join();
        }
        private static void SearchPorts(out string[] ports) { ports = SerialPort.GetPortNames(); }
        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = MyPort.ReadLine();
                    Console.WriteLine(message);
                }
                catch (TimeoutException) { }
            }
        }
        public static void Write()
        {
            string message = "";
            while (_continue)
            {
                try
                {
                    MyPort.WriteLine(message);
                }
                catch (TimeoutException) { _continue = false; }
            }
        }

        private void selectorPortBox(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                PortBox.Text = Convert.ToString(PortBox.SelectedItem);
                PortBox.TabIndex = PortBox.SelectedIndex;
            }
            catch (NullReferenceException ex) { MessageBox.Show(ex.ToString()); }

        }
        private void selectorBoudBox(object sender, SelectionChangedEventArgs e)
        {
            BoudBox.Text = Convert.ToString(BoudBox.SelectedItem);
            BoudBox.TabIndex = BoudBox.SelectedIndex;//select
        }
        private void selectorDataBox(object sender, SelectionChangedEventArgs e)
        {
            DataBox.Text = Convert.ToString(DataBox.SelectedItem);
            DataBox.TabIndex = DataBox.SelectedIndex;
        }
        private void selectorParityBox(object sender, SelectionChangedEventArgs e)
        {
            ParityBox.Text = Convert.ToString(ParityBox.SelectedItem);
            ParityBox.TabIndex = ParityBox.SelectedIndex;
        }
        private void selectorStopBox(object sender, SelectionChangedEventArgs e)
        {
            StopBox.Text = Convert.ToString(StopBox.SelectedItem);
            StopBox.TabIndex = StopBox.SelectedIndex;
 
        }
        private void Click_selectPortNameBtn(object sender, RoutedEventArgs e)
        {
            PortBox.TabIndex = ListViewCom_.SelectedIndex; 
            listBoxCom.Items.Add(ListViewCom_.SelectedIndex);
            foreach (object o in listBoxCom.Items)MessageBox.Show(o.ToString());
        }

        private string[] arrBoudRate = new[] { "110", "300", "600", "1200", "2400", "4800", "9600", "14400", "19200", "38400", "56000", "57600", "115200", "128000", "256000" };
        private string[] arrBit = new[] { "5", "6", "7", "8" };
        private string[] arrParitet = new[] { "нет.", "нечёт.", "чёт.", "марк.", "пробел" };
        private string[] arrParitetEng = new[] { "None", "Odd", "Even", "Mark", "Space" };
        private string[] arrStop = new[] { "1", "2", "1.5" };
        private string[] arrStopEng = new[] { "One", "Two", "OnePointFive" };

        //    None = 0,
        //    One = 1,
        //    Two = 2,
        //    OnePointFive = 3,
        private void listview_item_selected(object sender, SelectionChangedEventArgs e)
        {
            if(ListViewCom_.SelectedItem != null) { 
                search_Com = (ComSearch_)ListViewCom_.SelectedItem;
                PortBox.Text = search_Com.PorT; 
            }
            //ListViewCom_.SelectedIndex = comSelectedValue = Int32.Parse(ListViewCom_.SelectedIndex.ToString());
        }

        private void ports_window_open_close(object sender, RoutedEventArgs e)
        {
           settings.ComSearchWindow = settings.ComSearchWindow ? false : true; settings.Save();
            ports_window_open_cloce_func();
        }
        void ports_window_open_cloce_func()
        {
            if (settings.ComSearchWindow)
            {
                btnPorts.Content = "Порты<<";
                CanvasVisiblity.Visibility = Visibility.Visible;
                window_settings_comPort_tools.Width = 380;
                window_settings_comPort_tools.MinWidth = 380;
                window_settings_comPort_tools.MaxWidth = 380;
                settings.Save();
            } 
            else
            {
                btnPorts.Content = "Порты>>";
                CanvasVisiblity.Visibility = Visibility.Hidden;
                window_settings_comPort_tools.Width = 195;
                window_settings_comPort_tools.MinWidth = 195;
                window_settings_comPort_tools.MaxWidth = 195;
                settings.Save();
            }
            
        }

    }


    
}













//public class PortChat
//{
//    static bool _continue;
//    static SerialPort _serialPort;

//    public static void Main()
//    {
//        string name;
//        string message;
//        StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
//        Thread readThread = new Thread(Read);

//        // Create a new SerialPort object with default settings.
//        _serialPort = new SerialPort();

//        // Allow the user to set the appropriate properties.
//        _serialPort.PortName = SetPortName(_serialPort.PortName);
//        _serialPort.BaudRate = SetPortBaudRate(_serialPort.BaudRate);
//        _serialPort.Parity =   SetPortParity(_serialPort.Parity);
//        _serialPort.DataBits = SetPortDataBits(_serialPort.DataBits);
//        _serialPort.StopBits = SetPortStopBits(_serialPort.StopBits);
//        _serialPort.Handshake = SetPortHandshake(_serialPort.Handshake);

//        // Set the read/write timeouts
//        _serialPort.ReadTimeout = 500;
//        _serialPort.WriteTimeout = 500;

//        _serialPort.Open();
//        _continue = true;
//        readThread.Start();

//        Console.Write("Name: "); name = Console.ReadLine();
//        Console.WriteLine("Type QUIT to exit");

//        while (_continue)
//        {
//            message = Console.ReadLine();

//            if (stringComparer.Equals("quit", message))
//            {
//                _continue = false;
//            }
//            else
//            {
//                _serialPort.WriteLine( String.Format("<{0}>: {1}", name, message));
//            }
//        }

//        readThread.Join();
//        _serialPort.Close();
//    }

//    public static void Read()
//    {
//        while (_continue)
//        {
//            try
//            {
//                string message = _serialPort.ReadLine();
//                Console.WriteLine(message);
//            }
//            catch (TimeoutException) { }
//        }
//    }

//    // Display Port values and prompt user to enter a port.
//    public static string SetPortName(string defaultPortName)
//    {
//        string portName;

//        Console.WriteLine("Available Ports:");
//        foreach (string s in SerialPort.GetPortNames())
//        {
//            Console.WriteLine("   {0}", s);
//        }

//        Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
//        portName = Console.ReadLine();

//        if (portName == "" || !(portName.ToLower()).StartsWith("com"))
//        {
//            portName = defaultPortName;
//        }
//        return portName;
//    }
//    // Display BaudRate values and prompt user to enter a value.
//    public static int SetPortBaudRate(int defaultPortBaudRate)
//    {
//        string baudRate;

//        Console.Write("Baud Rate(default:{0}): ", defaultPortBaudRate);
//        baudRate = Console.ReadLine();

//        if (baudRate == "")
//        {
//            baudRate = defaultPortBaudRate.ToString();
//        }

//        return int.Parse(baudRate);
//    }

//    // Display PortParity values and prompt user to enter a value.
//    public static Parity SetPortParity(Parity defaultPortParity)
//    {
//        string parity;

//        Console.WriteLine("Available Parity options:");
//        foreach (string s in Enum.GetNames(typeof(Parity)))
//        {
//            Console.WriteLine("   {0}", s);
//        }

//        Console.Write("Enter Parity value (Default: {0}):", defaultPortParity.ToString(), true);
//        parity = Console.ReadLine();

//        if (parity == "")
//        {
//            parity = defaultPortParity.ToString();
//        }

//        return (Parity)Enum.Parse(typeof(Parity), parity, true);
//    }
//    // Display DataBits values and prompt user to enter a value.
//    public static int SetPortDataBits(int defaultPortDataBits)
//    {
//        string dataBits;

//        Console.Write("Enter DataBits value (Default: {0}): ", defaultPortDataBits);
//        dataBits = Console.ReadLine();

//        if (dataBits == "")
//        {
//            dataBits = defaultPortDataBits.ToString();
//        }

//        return int.Parse(dataBits.ToUpperInvariant());
//    }

//    // Display StopBits values and prompt user to enter a value.
//    public static StopBits SetPortStopBits(StopBits defaultPortStopBits)
//    {
//        string stopBits;

//        Console.WriteLine("Available StopBits options:");
//        foreach (string s in Enum.GetNames(typeof(StopBits)))
//        {
//            Console.WriteLine("   {0}", s);
//        }

//        Console.Write("Enter StopBits value (None is not supported and \n" +
//         "raises an ArgumentOutOfRangeException. \n (Default: {0}):", defaultPortStopBits.ToString());
//        stopBits = Console.ReadLine();

//        if (stopBits == "")
//        {
//            stopBits = defaultPortStopBits.ToString();
//        }

//        return (StopBits)Enum.Parse(typeof(StopBits), stopBits, true);
//    }
//    public static Handshake SetPortHandshake(Handshake defaultPortHandshake)
//    {
//        string handshake;

//        Console.WriteLine("Available Handshake options:");
//        foreach (string s in Enum.GetNames(typeof(Handshake)))
//        {
//            Console.WriteLine("   {0}", s);
//        }

//        Console.Write("Enter Handshake value (Default: {0}):", defaultPortHandshake.ToString());
//        handshake = Console.ReadLine();

//        if (handshake == "")
//        {
//            handshake = defaultPortHandshake.ToString();
//        }

//        return (Handshake)Enum.Parse(typeof(Handshake), handshake, true);
//    }