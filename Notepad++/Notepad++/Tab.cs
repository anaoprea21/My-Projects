using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Notepad__
{
    class Tab : INotifyPropertyChanged
    {
        private string p_header;
        public string header {
            get { return p_header; }
            set
            {
                p_header = value;
                OnPropertyChanged(nameof(header));
            }
        }

        private string p_content;
        public string content
        {
            get { return p_content; }
            set
            {
                p_content = value;
                if (header.Contains("*") == false)
                    header = "*" + header;
                OnPropertyChanged(nameof(content));
            }
        }
        private string p_tag;
        public string tag
        {
            get { return p_tag; }
            set
            {
                p_tag = value;
                OnPropertyChanged(nameof(tag));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
