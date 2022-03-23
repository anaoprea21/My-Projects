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
    /// Interaction logic for ReplaceAll.xaml
    /// </summary>
    public partial class ReplaceAll : Window
    {
        private string p_text;
        public string text {
            get { return p_text; } 
            set 
            { 
                p_text = value;        
            } 
        }
        private string p_replace;
        public string replace {
            get { return p_replace; }
            set
            {
                p_replace = value;
            }
        }
        public ReplaceAll()
        {
            InitializeComponent();
        }

        private void Button_Submit(object sender, RoutedEventArgs e)
        {
            text = textboxText.Text;
            replace = textboxReplace.Text;
            Close();
        }
    }
}
