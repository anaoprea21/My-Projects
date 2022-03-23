using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Notepad__
{
    class TabModel : INotifyPropertyChanged
    {
        private int p_index;
        public int index {
            get { return p_index; }
            set
            {
                p_index = value;
                OnPropertyChanged("index");
            }
        }
        private ObservableCollection<Tab> p_tabs { get; set; }
        public ObservableCollection<Tab> tabs
        {
            get
            { return p_tabs; }
            set
            {
                p_tabs = value;
                OnPropertyChanged("tabs");
            }
        }

        public TabModel()
        {
            tabs = new ObservableCollection<Tab>();
            index = 0;
        }

        //property changed
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
                throw new ArgumentNullException(GetType().Name + " does not contain property: " + propertyName);
        }

        //functions
        public void NewTab(object obj)
        {
            index += 1;
            Tab tab1 = new Tab()
            {
                header = "tab" + index.ToString()+" ",
                content = "",
                tag = null
            };
            tabs.Add(tab1);
            OnPropertyChanged("tabs");
        }
        public void RemoveTab(object obj)
        {
            Tab tab = tabs[index];
            if (tab.header.ToString().Contains("*"))
            {
                MessageBox.Show("File not saved!\nYou will lose all your progress!\nSave it first!", "Warning", MessageBoxButton.OK);
            }
            else
            {
                tabs.Remove(tab);
            }
            OnPropertyChanged("tabs");
        }
        public void OpenFile(object obj)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = ".txt";
            open.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";

            if (open.ShowDialog() == true)
            {
                Tab newTab = new Tab();

                string filename = open.FileName;
                string title = "";
                for (int i = 0; i < filename.Length; i++)
                {
                    title += filename[i];
                    if (filename[i] == 92)
                    {
                        title = "";
                    }
                }

                newTab.header = title;
                using (var reader = new StreamReader(filename))
                {
                    newTab.content = reader.ReadToEnd();
                }
               
                newTab.tag = filename;
                tabs.Add(newTab);
                OnPropertyChanged("tabs");
            }
        }
        public void SaveFile(object obj)
        {
            Tab tab = tabs[index];
            if (tab.tag != null)
            {
                System.IO.File.WriteAllText(tab.tag.ToString(), tab.content);
                string s = "";
                if (tab.header.ToString().Contains("*"))
                {
                    for (int i = 1; i < tab.header.ToString().Length; i++)
                    {
                        s += tab.header.ToString()[i];
                    }
                    tab.header = s;
                }
            }
            else
            {
                SaveAsFile(obj);
            }
            OnPropertyChanged("tabs");
        }
        public void SaveAsFile(object obj)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Text file|*.txt";
            save.Title = "Save a text File";
            save.ShowDialog();

            if (save.FileName != "")
            {
                Tab tab1 = tabs[index];
                System.IO.File.WriteAllText(save.FileName, tab1.content);
            }

            Tab tab = tabs[index];
            string title = "";

            for (int i = 0; i < save.FileName.Length; i++)
            {
                title += save.FileName[i];
                if (save.FileName[i] == 92)
                {
                    title = "";
                }
            }

            tab.header = title;
            tab.tag = save.FileName;
            OnPropertyChanged("tabs");
        }
        public void FindInFile(object obj)
        {
            Find window = new Find();
            window.ShowDialog();
            Tab tab = tabs[index];

            string findText = window.textbox.Text;
            string myText = tab.content;
            
            var regex = new Regex(Regex.Escape(findText));
            string newText = regex.Replace(myText, "***FOUND***" + findText + "***FOUND***", 1);

            Tab t = new Tab()
            {
                header = tabs[index].header,
                tag = tabs[index].tag,
                content = newText
            };

            tabs.Remove(tabs[index]);
            tabs.Add(t);
            OnPropertyChanged("tabs");
        }
        public void ReplaceInFile(object obj)
        {
            Tab tab = tabs[index];
            Replace window = new Replace();
            window.ShowDialog();

            string text = window.textboxText.Text;
            string replace = window.textboxReplace.Text;
            string myText = tab.content;

            var regex = new Regex(Regex.Escape(replace));
            string newText = regex.Replace(myText, text, 1);

            Tab t = new Tab()
            {
                header = tabs[index].header,
                tag = tabs[index].tag,
                content = newText
            };
            
            tabs.Remove(tabs[index]);
            tabs.Add(t);
            OnPropertyChanged("tabs");
        }
        public void ReplaceAllInFile(object obj)
        {
            Tab tab = tabs[index];
            ReplaceAll window = new ReplaceAll();
            window.ShowDialog();

            string text = window.textboxText.Text;
            string replace = window.textboxReplace.Text;
            string myText = tab.content;

            var regex = new Regex(Regex.Escape(replace));
            string newText = regex.Replace(myText, text);
            Console.WriteLine(newText);

            Tab t = new Tab()
            {
                header = tabs[index].header,
                tag = tabs[index].tag,
                content = newText
            };
            tabs.Remove(tabs[index]);
            tabs.Add(t);
            OnPropertyChanged("tabs");
        }

        ICommand newTabCommand;
        ICommand removeTabCommand;
        ICommand openFileCommand;
        ICommand saveFileCommand;
        ICommand saveAsFileCommand;

        ICommand findCommand;
        ICommand replaceCommand;
        ICommand replaceAllCommand;

        public ICommand newTabCom
        {
            get
            {
                if (newTabCommand == null)
                    newTabCommand = new RelayCommand(NewTab);
                return newTabCommand;
            }
        }
        public ICommand removeTabCom
        {
            get
            {
                if (removeTabCommand == null)
                    removeTabCommand = new RelayCommand(RemoveTab);
                return removeTabCommand;
            }
        }
        public ICommand openFileCom
        {
            get
            {
                if (openFileCommand == null)
                    openFileCommand = new RelayCommand(OpenFile);
                return openFileCommand;
            }
        }
        public ICommand saveFileCom
        {
            get
            {
                if (saveFileCommand == null)
                    saveFileCommand = new RelayCommand(SaveFile);
                return saveFileCommand;
            }
        }
        public ICommand saveAsFileCom
        {
            get
            {
                if (saveAsFileCommand == null)
                    saveAsFileCommand = new RelayCommand(SaveAsFile);
                return saveAsFileCommand;
            }
        }

        public ICommand findCom
        {
            get
            {
                if (findCommand == null)
                    findCommand = new RelayCommand(FindInFile);
                return findCommand;
            }
        }
        public ICommand replaceCom
        {
            get
            {
                if (replaceCommand == null)
                    replaceCommand = new RelayCommand(ReplaceInFile);
                return replaceCommand;
            }
        }
        public ICommand replaceAllCom
        {
            get
            {
                if (replaceAllCommand == null)
                    replaceAllCommand = new RelayCommand(ReplaceAllInFile);
                return replaceAllCommand;
            }
        }

    }
}
