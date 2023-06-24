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

namespace comPort_wpf
{
    public partial class ComPortXAML : Window
    {
       private string[] ports;
        private ObservableCollection<ComSearch_> listViewCom;
       
        
       static public SerialPort MyPort = new("COM0", 115200,Parity.None,8, StopBits.One);
        Thread readThread = new Thread(Read);
        static bool _continue;
        public ComPortXAML()
        {
            InitializeComponent();
            listViewCom = new ObservableCollection<ComSearch_>(); ListViewCom.ItemsSource = listViewCom;

        }

        private void BtnEnter(object sender, RoutedEventArgs e)
        {
            //PropertyPath.ReferenceEquals(this, (Settings)sender);
            Close();
        }
        private void BtnCancel(object sender, RoutedEventArgs e)
        {
            //this.Close();
            Close();
        } 
        private void Rescan_ComPort(object sender, RoutedEventArgs e)
        {
           SearchPorts(out ports);
            string Port = "";
            string Status = "";
            Image icon = null;
            string? error;
            MyPort.ReadTimeout = 500;
            MyPort.WriteTimeout = 500;
            foreach (var p in ports) {
                Port = p.ToString();
                MyPort.PortName = Port;
                try { MyPort.Open();}
                catch(System.IO.IOException ev) { Status = "Не доступен ";error = ev.ToString();break; }
                readThread.Start();
                try {
                    if (MyPort.IsOpen == true) { Status = "Порт открыт "; }
                    else { Status = "Порт не в работе "; }  
                }
                catch(Exception ex) { MessageBox.Show(ex.ToString()); }

              
                //Status = MyPort.IsOpen ? "Не доступен" : "Доступен";
                readThread.Join();
                MyPort.Close();
                listViewCom.Add(new ComSearch_ {Img=icon, PorT = Port, StatuS = Status });
            }
          
        }



        //_serialPort.Open();
        //_continue = true;
        //readThread.Start();

        //Console.Write("Name: "); name = Console.ReadLine();
        //Console.WriteLine("Type QUIT to exit");

        

        //readThread.Join();
        //_serialPort.Close();

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
}
