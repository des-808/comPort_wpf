using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static comPort_wpf.MainWindow;

namespace comPort_wpf
{
    /// <summary>
    /// Логика взаимодействия для settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        //Phone phones = new Phone();
        //public ObservableCollection<ComStruct> tTerminalProbnList { get; set; }
        public Settings()
        {
            InitializeComponent();
            //tTerminalProbnList = new ObservableCollection<ComStruct>(); TerminalProbnList.ItemsSource = tTerminalProbnList;
            //tTerminalProbnList.Add(new ComStruct { baudRat = 110 });
            //tTerminalProbnList.Add(new ComStruct { baudRat = 300 });
            //tTerminalProbnList.Add(new ComStruct { baudRat = 600 });
            //tTerminalProbnList.Add(new ComStruct { baudRat = 1200 });
            //tTerminalProbnList.Add(new ComStruct { baudRat = 2400 });
            //tTerminalProbnList.Add(new ComStruct { baudRat = 4800 });
            //tTerminalProbnList.Add(new ComStruct { baudRat = 9600 });
            //tTerminalProbnList.Add(new ComStruct { baudRat = 14400 });
            //tTerminalProbnList.Add(new ComStruct { baudRat = 19200 });
            //tTerminalProbnList.Add(new ComStruct { baudRat = 38400 });
            //tTerminalProbnList.Add(new ComStruct { baudRat = 56400 });
            //tTerminalProbnList.Add(new ComStruct { baudRat = 57600 });
            //tTerminalProbnList.Add(new ComStruct { baudRat = 115200 });
            //tTerminalProbnList.Add(new ComStruct { baudRat = 128000 });
            //tTerminalProbnList.Add(new ComStruct { baudRat = 256000 });
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void rts(object sender, DragEventArgs e)
        {
            

        }

        private void rts(object sender, RoutedEventArgs e) => Data.Value = textbox1.Text;

        private void TerminalProbnList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ComStruct p = (ComStruct)TerminalProbnList.SelectedItem;
            //MessageBox.Show(p.baudRat.ToString());
        }
    }

    
}
