using System;
using System.Collections.Generic;
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

namespace LanIpChanger
{
    /// <summary>
    /// Логика взаимодействия для GetStringWindow.xaml
    /// </summary>
    public partial class GetStringWindow : Window
    {
        public string ToReturn = "";
        public GetStringWindow(string Title, string? Text)
        {
            InitializeComponent();
            if (Text != null)
            {
                MainTextBox.Text = Text;    
            }
            this.Title = Title;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ToReturn = MainTextBox.Text;
            DialogResult = true;
        }
    }
}
