using Microsoft.Win32;
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

namespace HangMan.View
{
    /// <summary>
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        public string user { get; set; }
        public string imagePath { get; set; }
        public NewUser()
        {
            InitializeComponent();
        }
        private void SearchImage(object sender, RoutedEventArgs e)
        {
            Gallery gallery = new Gallery();
            gallery.ShowDialog();
            path.Text = gallery.selected;
        }
        private void Submit(object sender, RoutedEventArgs e)
        {
            if (path.Text != "")
            {
                Close();
            }
            else
            {
                System.Windows.MessageBox.Show("You must upload an image to continue!", "Warning", MessageBoxButton.OK);
            }
        }
    }
}
