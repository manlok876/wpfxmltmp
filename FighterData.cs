using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;

namespace WPF_XML_FL
{
    public class FighterData : INotifyPropertyChanged
    {
        string _name;
        string _email;
        string _city;
        string _club;
        string _phone;
        DateTime _birthdate;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnNotifyPropertyChanged("Name");
            }
        }
        public string Email 
        {
            get { return _email; }
            set
            {
                _email = value;
                OnNotifyPropertyChanged("Email");
            }
        }
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnNotifyPropertyChanged("City");
            }
        }
        public string Club
        {
            get { return _club; }
            set
            {
                _club = value;
                OnNotifyPropertyChanged("Club");
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnNotifyPropertyChanged("Phone");
            }
        }
        public DateTime Birthdate
        {
            get { return _birthdate; }
            set
            {
                _birthdate = value;
                OnNotifyPropertyChanged("Birthdate");
            }
        }

        private void OnNotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public FighterData()
        {
            Name = "No Name";
            Email = "no@mail.com";
            City = "Nowhere";
            Club = "-";
            Phone = "8 800 555 35 35";
            Birthdate = new DateTime(2000, 1, 1);
        }
    }
}
