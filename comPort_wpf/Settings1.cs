using System.Windows;

namespace comPort_wpf
{
    // Этот класс позволяет обрабатывать определенные события в классе параметров:
    //  Событие SettingChanging возникает перед изменением значения параметра.
    //  Событие PropertyChanged возникает после изменения значения параметра.
    //  Событие SettingsLoaded возникает после загрузки значений параметров.
    //  Событие SettingsSaving возникает перед сохранением значений параметров.
    internal  partial class Settings1
    {
        public Settings1()
        {
            // // Для добавления обработчиков событий для сохранения и изменения параметров раскомментируйте приведенные ниже строки:
            //
            this.SettingChanging += this.SettingChangingEventHandler;
            //
            this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }

        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e)
        {
            // Добавьте здесь код для обработки события SettingChangingEvent.
           // MessageBox.Show($"SettingChangingEventHandler: ");
        }

        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Добавьте здесь код для обработки события SettingsSaving.
        }
    }
}


//< setting name = "Port" serializeAs = "String" >

//    < value > Com1 </ value >
//</ setting >
//< setting name = "BoudRate" serializeAs = "String" >

//    < value > 19200 </ value >
//</ setting >
//< setting name = "Parity" serializeAs = "String" >

//    < value > 1 </ value >
//</ setting >
//< setting name = "Data" serializeAs = "String" >

//    < value > 8 </ value >
//</ setting >
//< setting name = "Stop" serializeAs = "String" >

//    < value > 1 </ value >
//</ setting >