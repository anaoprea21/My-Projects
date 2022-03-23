using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Notepad__
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void MenuItem_Help(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Nume: Oprea\nPrenume: " +
                "Ana\nEmail: ana-v.oprea@student.unitbv.ro", "About", MessageBoxButton.OK);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var drive in Directory.GetLogicalDrives())
            {
                var item = new TreeViewItem();

                item.Header = drive;
                item.Tag = drive;

                item.Items.Add(null);

                //what to do when expanded
                item.Expanded += Folder_Expanded;

                folderView.Items.Add(item);
            }
        }
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;

            //detecting the null element added 
            if (item.Items.Count != 1)
                return;

            string path = item.Tag.ToString();
            string[] dirs = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);

            foreach (string dir in dirs)
            {
                string title = null;
                var subItem = new TreeViewItem();
                for (int i = 0; i < dir.Length; i++)
                {
                    title += dir[i];
                    if (dir[i] == 92)
                    {
                        title = "";
                    }
                }
                subItem.Header = title;
                subItem.Tag = dir;

                subItem.Items.Add(null);

                subItem.Expanded += Folder_Expanded;
                item.Items.Add(subItem);
            }

        }
    }
}
