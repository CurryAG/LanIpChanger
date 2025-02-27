using System.Configuration;
using System.Data;
using System.Windows;

namespace LanIpChanger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string ConfigFile = "Config.JSON";
        public static string BackupFile = $"{ConfigFile}.OLD";
        public static Config Config = new Config();
    }

}
