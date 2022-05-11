using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangMan.ViewModel
{
    class User : INotifyPropertyChanged
    {
        private string p_username;
        public string username
        {
            get { return p_username; }
            set
            {
                p_username = value;
                OnPropertyChanged(nameof(username));
            }
        }

        private string p_ImagePath;
        public string imagePath
        {
            get { return p_ImagePath; }
            set
            {
                p_ImagePath = value;
                OnPropertyChanged(nameof(imagePath));
            }
        }
        public User(string name, string path)
        {
            username = name;
            imagePath = path;
        }

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

    }
}
