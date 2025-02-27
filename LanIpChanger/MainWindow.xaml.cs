using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LanIpChanger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                Program();
            }
            catch (Exception ex)
            {
                if (File.Exists(App.BackupFile))
                {
                    JSONController.RestoreBackup();
                }
                else
                {
                    File.Delete(App.ConfigFile);
                }
                MessageBox.Show($"Произошла ошибка, была восстановлена последняя успешная конфигурация, перезапустите приложение\n\n\n\n{ex.Message}\n{ex.InnerException}\n{ex.Source}\n{ex.Data}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
        }
        private void Program()
        {
            List<AdapterData> Adapters = new List<AdapterData>();
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (!adapter.Name.Contains("Pseudo-Interface"))
                {
                    AdapterData Adapter = new AdapterData(adapter.Id, adapter.Name);
                    Adapters.Add(Adapter);
                }
            }
            if (!File.Exists(App.ConfigFile))
            {
                App.Config.Adapters = Adapters;
                App.Config.SelectedAdapterId = Adapters[0].Id;
                AdapterComboBox.SelectedIndex = 0;
            }
            else
            {
                JSONController.LoadConfig();
            }
            foreach (var item in Adapters)
            {
                bool founded = false;
                foreach (var item2 in App.Config.Adapters)
                {
                    if (item.Id == item2.Id)
                    {
                        founded = true;
                    }
                }
                if (!founded)
                {
                    App.Config.Adapters.Add(item);
                }
            }
            foreach (var adapter in App.Config.Adapters)
            {
                if (GetAdapterSettingsByAdapterId(adapter.Id).Count == 0)
                {
                    AdapterSetting adapterSetting = new AdapterSetting("По умолчанию", adapter.Id, true);
                    App.Config.AdapterSettings.Add(adapterSetting);
                    JSONController.SaveConfig();
                }
            }
            AdapterComboBox.ItemsSource = App.Config.Adapters;
            foreach (var item in AdapterComboBox.Items)
            {
                if ((item as AdapterData).Id == App.Config.SelectedAdapterId)
                {
                    AdapterComboBox.SelectedItem = item;
                    break;
                }
            }
            foreach (var item in NetSettingsCombobox.Items)
            {
                AdapterSetting temp = item as AdapterSetting;
                if (temp.AdapterId == App.Config.SelectedAdapterId && temp.Name == App.Config.SelectedAdapterSettingsName)
                {
                    NetSettingsCombobox.SelectedItem = item;
                }
            }
            JSONController.CreateBackup();
        }

        private void AdapterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AdapterData Selected = AdapterComboBox.SelectedItem as AdapterData;
            App.Config.SelectedAdapterId = Selected.Id;
            NetSettingsCombobox.ItemsSource = GetAdapterSettingsByAdapterId(Selected.Id);
            NetSettingsCombobox.SelectedIndex = 0;
        }


        public List<AdapterSetting> GetAdapterSettingsByAdapterId(string Id)
        {
            List<AdapterSetting> ToReturn = new List<AdapterSetting>();
            foreach (var adapterSetting in App.Config.AdapterSettings)
            {
                if (adapterSetting.AdapterId == Id)
                {
                    ToReturn.Add(adapterSetting);
                }
            }
            return ToReturn;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ShowConfirmation("Добавить настройку?"))
            {
                return;
            }
            AdapterData Selected = AdapterComboBox.SelectedItem as AdapterData;
            AdapterSetting NewSettings = new AdapterSetting($"Настройка {IntRandomGenerator.GenerateRandomNumber(8)}", Selected.Id);
            NewSettings.MainIP = "0.0.0.0";
            NewSettings.MaskIP = "0.0.0.0";
            NewSettings.GateAwayIP = "0.0.0.0";
            NewSettings.FirstDNS = "0.0.0.0";
            NewSettings.SecondDNS = "0.0.0.0";
            App.Config.AdapterSettings.Add(NewSettings);
            NetSettingsCombobox.ItemsSource = GetAdapterSettingsByAdapterId(Selected.Id);
            NetSettingsCombobox.SelectedIndex = NetSettingsCombobox.Items.Count - 1;
            JSONController.SaveConfig();
            MessageBox.Show("Успешно");
        }

        private void NetSettingsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AdapterSetting Selected = NetSettingsCombobox.SelectedItem as AdapterSetting;
            if (Selected == null)
            {
                return;
            }
            if (Selected.IsDefault)
            {
                IPTextBox.Text = string.Empty;
                IPTextBox.IsEnabled = false;
                MaskIPTextBox.Text = string.Empty;
                MaskIPTextBox.IsEnabled = false;
                GateawayIPTextBox.Text = string.Empty;
                GateawayIPTextBox.IsEnabled = false;
                FirstDNSTextBox.Text = string.Empty;
                FirstDNSTextBox.IsEnabled = false;
                SecondDNSTextBox.Text = string.Empty;
                SecondDNSTextBox.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                SaveButton.IsEnabled = false;
            }
            else
            {
                IPTextBox.Text = Selected.MainIP;
                IPTextBox.IsEnabled = true;
                MaskIPTextBox.Text = Selected.MaskIP;
                MaskIPTextBox.IsEnabled = true;
                GateawayIPTextBox.Text = Selected.GateAwayIP;
                GateawayIPTextBox.IsEnabled = true;
                FirstDNSTextBox.Text = Selected.FirstDNS;
                FirstDNSTextBox.IsEnabled = true;
                SecondDNSTextBox.Text = Selected.SecondDNS;
                SecondDNSTextBox.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                SaveButton.IsEnabled = true;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ShowConfirmation("Удалить настройку?"))
            {
                return;
            }
            AdapterSetting Selected = NetSettingsCombobox.SelectedItem as AdapterSetting;
            if (Selected == null)
            {
                return;
            }
            if (Selected.IsDefault)
            {
                MessageBox.Show("Нельзя удалить стандартную настройку (стандартный IP)");
                return;
            }
            for (int i = 0; i < App.Config.AdapterSettings.Count; i++)
            {
                if (App.Config.AdapterSettings[i].Name == Selected.Name && App.Config.AdapterSettings[i].AdapterId == Selected.AdapterId)
                {
                    App.Config.AdapterSettings.RemoveAt(i);
                    break;
                }
            }
            JSONController.SaveConfig();
            MessageBox.Show("Успешно");
            AdapterData Selected2 = AdapterComboBox.SelectedItem as AdapterData;
            NetSettingsCombobox.ItemsSource = GetAdapterSettingsByAdapterId(Selected2.Id);
            NetSettingsCombobox.SelectedIndex = 0;
        }
        public static bool ShowConfirmation(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            return result == MessageBoxResult.Yes;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            AdapterSetting Selected = NetSettingsCombobox.SelectedItem as AdapterSetting;
            GetStringWindow getStringWindow = new GetStringWindow("Редактирование названия", Selected.Name);
            if (getStringWindow.ShowDialog() == true)
            {
                for (int i = 0; i < App.Config.AdapterSettings.Count; i++)
                {
                    if (App.Config.AdapterSettings[i].Name == Selected.Name && App.Config.AdapterSettings[i].AdapterId == Selected.AdapterId)
                    {
                        if(!CheckFreeName(getStringWindow.ToReturn, Selected.AdapterId))
                        {
                            MessageBox.Show("Название уже используется");
                            return;
                        }
                        App.Config.AdapterSettings[i].Name = getStringWindow.ToReturn;
                        break;
                    }
                }
                JSONController.SaveConfig();
                MessageBox.Show("Сохранено");
            }

            AdapterData Selected2 = AdapterComboBox.SelectedItem as AdapterData;
            NetSettingsCombobox.ItemsSource = null; 
            NetSettingsCombobox.SelectedItem = null;
            NetSettingsCombobox.ItemsSource = GetAdapterSettingsByAdapterId(Selected2.Id);
            NetSettingsCombobox.SelectedIndex = 0;
            foreach (var item in NetSettingsCombobox.Items)
            {
                AdapterSetting temp = item as AdapterSetting;
                if (temp.AdapterId == Selected.AdapterId && temp.Name == getStringWindow.ToReturn)
                {
                    NetSettingsCombobox.SelectedItem = item;
                    break;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            AdapterSetting Selected = NetSettingsCombobox.SelectedItem as AdapterSetting;
            for (int i = 0; i < App.Config.AdapterSettings.Count; i++)
            {
                if (App.Config.AdapterSettings[i].Name == Selected.Name && App.Config.AdapterSettings[i].AdapterId == Selected.AdapterId)
                {
                    App.Config.AdapterSettings[i].MainIP = IPTextBox.Text;
                    App.Config.AdapterSettings[i].MaskIP = MaskIPTextBox.Text;
                    App.Config.AdapterSettings[i].GateAwayIP = GateawayIPTextBox.Text;
                    App.Config.AdapterSettings[i].FirstDNS = FirstDNSTextBox.Text;
                    App.Config.AdapterSettings[i].SecondDNS = SecondDNSTextBox.Text;
                    break;
                }
            }
            AdapterData Selected2 = AdapterComboBox.SelectedItem as AdapterData;
            NetSettingsCombobox.ItemsSource = GetAdapterSettingsByAdapterId(Selected2.Id);
            NetSettingsCombobox.SelectedIndex = 0;
            foreach (var item in NetSettingsCombobox.Items)
            {
                AdapterSetting temp = item as AdapterSetting;
                if (temp.AdapterId == Selected.AdapterId && temp.Name == Selected.Name)
                {
                    NetSettingsCombobox.SelectedItem = item;
                    break;
                }
            }
            JSONController.SaveConfig();
            if (sender == null && e == null)
            {
                return;
            }
            MessageBox.Show("Сохранено");
        }
        public bool CheckFreeName(string Name, string AdapterId)
        {
            foreach (var item in GetAdapterSettingsByAdapterId(AdapterId))
            {
                if (item.Name == Name)
                {
                    return false;
                }
            }
            return true;
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            string AdapterName = (AdapterComboBox.SelectedItem as AdapterData).Name;
            IPController.Reset(AdapterName);
            AdapterSetting SelectedAdapterSetting = NetSettingsCombobox.SelectedItem as AdapterSetting;
            SaveButton_Click(null, null);
            if (SelectedAdapterSetting.IsDefault)
            {
                MessageBox.Show("Успешно");
                App.Config.SelectedAdapterSettingsName = SelectedAdapterSetting.Name;
                JSONController.SaveConfig();
                return;
            }
            try
            {
                IPController.SetIP(AdapterName, SelectedAdapterSetting.MainIP, SelectedAdapterSetting.MaskIP, SelectedAdapterSetting.GateAwayIP);
                IPController.SetDns(AdapterName, SelectedAdapterSetting.FirstDNS, SelectedAdapterSetting.SecondDNS);
                MessageBox.Show("Успешно");
                App.Config.SelectedAdapterSettingsName = SelectedAdapterSetting.Name;
                JSONController.SaveConfig();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}