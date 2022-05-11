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
    /// Interaction logic for Gallery.xaml
    /// </summary>
    public partial class Gallery : Window
    {
        public string selected = "C:\\Users\\anaop\\source\\repos" +
            "\\HangMan\\HangMan\\Images\\";
        public Gallery()
        {
            InitializeComponent();
        }

        private void Ok(object sender, RoutedEventArgs e)
        {
            if(art.IsChecked==true)
            {
                selected += "art.jpg";
            }
            else
            {
                if(flower.IsChecked == true)
                {
                    selected += "flower.jpg";
                }
                else
                {
                    if (statue.IsChecked == true)
                    {
                        selected += "statue.jpg";
                    }
                    else
                    {
                        if (giphy.IsChecked == true)
                        {
                            selected += "giphy.gif";
                        }
                        else
                        {
                            if (giphy2.IsChecked == true)
                            {
                                selected += "giphy_lazyGuy.gif";
                            }
                        }
                    }
                }
            }

            Close();
        }
    }
}
