using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace comPort_wpf
{
    public partial class ComPortXAML : Window
    {
       private string[] ports;
        private ObservableCollection<ComSearch_> listViewCom;
        private int comSelectedValue = -1;
        ComSearch_ search_Com;

       static public SerialPort MyPort = new("COM0", 115200,Parity.None,8, StopBits.One);
        Thread writeThread = new Thread(Write);
        static bool _continue = true;
        public ComPortXAML()
        {
            InitializeComponent();
            ports = SerialPort.GetPortNames();
            listViewCom = new ObservableCollection<ComSearch_>(); ListViewCom_.ItemsSource = listViewCom;
            PortBox  .Items.Clear();
            BoudBox  .Items.Clear();
            DataBox  .Items.Clear();
            ParityBox.Items.Clear();
            StopBox  .Items.Clear();
            foreach (string p in ports      ) { PortBox  .Items.Add(p);}
            foreach (string b in arrBoudRate) { BoudBox  .Items.Add(b);}
            foreach (string b in arrBit     ) { DataBox  .Items.Add(b);}
            foreach (string b in arrParitet ) { ParityBox.Items.Add(b);}
            foreach (string b in arrStop    ) { StopBox  .Items.Add(b);}
            //PortBox.Text   = comInitStruct
            //BoudBox.Text   = 
            //DataBox.Text   = 
            //ParityBox.Text = 
            //StopBox.Text   = 
        }

        private void BtnEnter(object sender, RoutedEventArgs e)
        {
            //PropertyPath.ReferenceEquals(this, (Settings)sender);
            Close();
        }
        private void BtnCancel(object sender, RoutedEventArgs e)
        {
            Close();
        } 
        private void Rescan_ComPort(object sender, RoutedEventArgs e)
        {
           SearchPorts(out ports);
            PortBox.Items.Clear();
            string Port = "";
            string Status = "";
            Image icon = null;
            string? error;
            MyPort.ReadTimeout = 500;
            MyPort.WriteTimeout = 200;
            foreach (string p in ports) {
                // Port = p.ToString();
                MyPort.PortName = p;
                try { MyPort.Open();}
                catch(System.IO.IOException ev) { Status = "Не доступен ";error = ev.ToString();goto metka; }
                _continue = true;
                if(writeThread.IsAlive){  writeThread.Start(); }
                //else { writeThread. }
                //try {//if (MyPort.IsOpen == true) { Status = "Порт открыт "; }else { Status = "Порт не в работе "; }
                //}    catch(Exception ex) { MessageBox.Show(ex.ToString());}
                Status = MyPort.IsOpen ? "Порт открыт " : "Порт не в работе ";
                //writeThread.Join();

                writeThread.Interrupt();
                metka:
                MyPort.Close();
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
                catch (TimeoutException) { _continue = false;  }
            }
        }

        private void selectorPortBox(object sender, SelectionChangedEventArgs e)
        {
            PortBox.Text = PortBox.SelectedItem.ToString();
        }
        private void selectorBoudBox(object sender, SelectionChangedEventArgs e)
        {
            BoudBox.Text = BoudBox.SelectedItem.ToString();
            var select  =  BoudBox.SelectedIndex;
            BoudBox.TabIndex = select;
        }
        private void selectorDataBox(object sender, SelectionChangedEventArgs e)
        {
            DataBox.Text = DataBox.SelectedItem.ToString();
        }
        private void selectorParityBox(object sender, SelectionChangedEventArgs e)
        {
            ParityBox.Text = ParityBox.SelectedItem.ToString();
        }
        private void selectorStopBox(object sender, SelectionChangedEventArgs e)
        {
            StopBox.Text = StopBox.SelectedItem.ToString();
        }
        private void Click_selectPortNameBtn(object sender, RoutedEventArgs e)
        {
           // var tmp = listViewCom.SelectedItem.ToString();
           
           PortBox.Text = ListViewCom_.SelectedItems.ToString();

            
            //SearchPorts.Name = listViewCom.Select()
        }

        private string[] arrBoudRate = new[] { "110", "300", "600", "1200", "2400", "4800", "9600", "14400", "19200", "38400", "56000", "57600", "115200", "128000", "256000" };
        private string[] arrBit = new[] { "5", "6", "7", "8" };
        private string[] arrParitet = new[] { "нет.", "нечёт.", "чёт.", "марк.", "пробел" };
        private string[] arrStop = new[] { "1", "1.5", "2" };

        private void listview_item_selected(object sender, SelectionChangedEventArgs e)
        {
            //if(ListViewCom_.SelectedItem != null) { ListViewCom_.Remove(ListViewCom_.) }
           // var tmp = ListViewCom_.SelectedIndex.ToString();
            ListViewCom_.SelectedIndex = comSelectedValue = Int32.Parse(ListViewCom_.SelectedIndex.ToString());
            // MessageBox.Show(tmp);
            //var listtt = GetSelectedItem(listViewCom);
        }



        public static string GetSelectedItem(ListView list)
        {
            foreach (ListViewItem item in list.Items)
            {
                if (item.IsSelected)
                    return item.Name;
            }
            return null;
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