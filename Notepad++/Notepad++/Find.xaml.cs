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

namespace Notepad__
{
    /// <summary>
    /// Interaction logic for Find.xaml
    /// </summary>
    public partial class Find : Window
    {
        string text { get;  set; }
        public Find()
        {
            InitializeComponent();
        }
        public void Button_Submit(object sender, RoutedEventArgs e)
        {
            string text = textbox.Text;
            Close();
        }
    }
}
