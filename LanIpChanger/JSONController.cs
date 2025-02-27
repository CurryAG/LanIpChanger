using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LanIpChanger
{
    internal class JSONController
    {
        public static void SaveConfig()
        {
            string json = JsonSerializer.Serialize(App.Config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(App.ConfigFile, json);
            CreateBackup();
        }
        public static void LoadConfig()
        {
            string json = File.ReadAllText(App.ConfigFile);
            App.Config = JsonSerializer.Deserialize<Config>(json);
        }
        public static void CreateBackup()
        {
            if (CheckCorrectFile(App.ConfigFile))
            {
                File.Copy(App.ConfigFile, App.BackupFile, true);
            }
        }
        public static void RestoreBackup()
        {
            File.Copy(App.BackupFile, App.ConfigFile, true);
        }

        public static bool CheckCorrectFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return false;
                }
                string content = File.ReadAllText(filePath);

                return content.Length > 30;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}