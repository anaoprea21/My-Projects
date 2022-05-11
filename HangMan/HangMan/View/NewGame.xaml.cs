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
using System.Windows.Threading;

namespace HangMan.View
{
    /// <summary>
    /// Interaction logic for NewGame.xaml
    /// </summary>
    public partial class NewGame : Window
    {
        DispatcherTimer timer;
        private int time = 120;
        public NewGame(string usern)
        {
            InitializeComponent();
            player.Text = "Welcome, " + usern + "!";
        }
        private void MenuItem_About(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Nume: Oprea\nPrenume: " +
               "Ana\nEmail: ana-v.oprea@student.unitbv.ro", "About", MessageBoxButton.OK);
        }
        private void Button_Start(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Ticker;
            timer.Start();
        }
        private void Ticker(object sender, EventArgs e)
        {
            time--;
            if (time > 0)
            {
                timerLabel.Content = string.Format("0{0}:{1}", time / 60, time % 60);
            }
            else
            {
                timer.Stop();
                MessageBox.Show("You lose!");
            }
            if (dead.Text.Equals("0"))
            {
                timer.Stop();
            }
            if(word.Text.Contains("*").Equals(false))
            {
                timer.Stop();
            }

        }
        private void MenuItem_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            time = 120;
        }
    }
}
