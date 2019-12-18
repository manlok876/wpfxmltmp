using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Win32;

namespace WPF_XML_FL
{
    class FightersList : INotifyPropertyChanged
    {
        public ObservableCollection<FighterData> FilteredFighters { get; set; }
        public ObservableCollection<FighterData> Fighters { get; set; }

        public string SelectedClub { get; set; }
        public ObservableCollection<string> Clubs { get; set; }
        public string SelectedCity { get; set; }
        public ObservableCollection<string> Cities { get; set; }

        FighterData _editableData;
        public FighterData EditableData
        {
            get { return _editableData; }
            set
            {
                _editableData = value;
                OnNotifyPropertyChanged("EditableData");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnNotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public FightersList()
        {
            _editableData = new FighterData();

            Fighters = new ObservableCollection<FighterData>();
            FilteredFighters = new ObservableCollection<FighterData>();
            Fighters.Add(new FighterData());
            Fighters.Add(new FighterData());
            Fighters.Add(new FighterData());

            SelectedCity = SelectedClub = "*";
            ApplyFilter();

            _addCmd = new AddCmd(this);
            _delCmd = new DeleteCmd(this);
            _saveCmd = new SaveCmd(this);
            _loadCmd = new LoadCmd(this);

            Clubs = new ObservableCollection<string> { };
            Cities = new ObservableCollection<string> { };
            Fighters.CollectionChanged += UpdateClubs;
            Fighters.CollectionChanged += UpdateCities;
            
            UpdateCities(this, null);
            UpdateClubs(this, null);
        }

        private void UpdateClubs(object sender, NotifyCollectionChangedEventArgs e)
        {
            string savedFilter = SelectedClub;
            var clubs = (from fighter in Fighters
                         select fighter.Club).Distinct();
            Clubs.Clear();
            Clubs.Add("*");
            foreach (string club in clubs)
            {
                Clubs.Add(club);
            }
            if (Clubs.Contains(savedFilter))
            {
                SelectedClub = savedFilter;
            }
            else
            {
                SelectedClub = "*";
            }
        }

        private void UpdateCities(object sender, NotifyCollectionChangedEventArgs e)
        {
            string savedFilter = SelectedCity;
            var cities = (from fighter in Fighters
                          select fighter.City).Distinct();
            Cities.Clear();
            Cities.Add("*");
            foreach (string city in cities)
            {
                Cities.Add(city);
            }
            if (Cities.Contains(savedFilter))
            {
                SelectedCity = savedFilter;
            }
            else
            {
                SelectedCity = "*";
            }
        }

        public void ApplyFilter()
        {
            FilteredFighters.Clear();
            var filtered = from fighter in Fighters
                           where (SelectedClub == "*" || fighter.Club == SelectedClub) &&
                           (SelectedCity == "*" || fighter.City == SelectedCity)
                           select fighter;
            foreach (var fighter in filtered)
            {
                FilteredFighters.Add(fighter);
            }
        }

        #region Commands

        AddCmd _addCmd;
        public AddCmd AddCommand
        {
            get { return _addCmd; }
        }
        public class AddCmd : ICommand
        {
            FightersList w;
            public AddCmd(FightersList window)
            {
                w = window;
            }
            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }
            public void Execute(object parameter)
            {
                w.Fighters.Add(w.EditableData);
                w.EditableData = new FighterData();
            }
        }

        DeleteCmd _delCmd;
        public DeleteCmd DeleteCommand
        {
            get { return _delCmd; }
        }
        public class DeleteCmd : ICommand
        {
            FightersList w;
            public DeleteCmd(FightersList window)
            {
                w = window;
            }
            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }
            public void Execute(object parameter)
            {
                w.Fighters.Remove(w.EditableData);
                w.EditableData = new FighterData();
            }
        }

        SaveCmd _saveCmd;
        public SaveCmd SaveCommand
        {
            get { return _saveCmd; }
        }
        public class SaveCmd : ICommand
        {
            FightersList w;
            public SaveCmd(FightersList window)
            {
                w = window;
            }
            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }
            public void Execute(object parameter)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML Document(*.xml)|*.xml|All Files(*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<FighterData>));
                    List<FighterData> ListToSave = new List<FighterData>(w.Fighters);
                    using (TextWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        serializer.Serialize(writer, ListToSave);
                    }
                }
            }
        }

        LoadCmd _loadCmd;
        public LoadCmd LoadCommand
        {
            get { return _loadCmd; }
        }
        public class LoadCmd : ICommand
        {
            FightersList w;
            public LoadCmd(FightersList window)
            {
                w = window;
            }
            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }
            public void Execute(object parameter)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "XML Document(*.xml)|*.xml|All Files(*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        XmlSerializer deserializer = new XmlSerializer(typeof(List<FighterData>));
                        using (TextReader reader = new StreamReader(openFileDialog.FileName))
                        {
                            object obj = deserializer.Deserialize(reader);
                            var LoadedList = obj as List<FighterData>;
                            w.Fighters.Clear();
                            foreach (FighterData fighter in LoadedList)
                            {
                                w.Fighters.Add(fighter);
                            }
                        }
                    }
                    catch (FileNotFoundException e)
                    {
                        w.Fighters = null;
                    }
                    catch (InvalidOperationException e)
                    {
                        w.Fighters = null;
                    }
                    if (w.Fighters == null)
                    {
                        w.Fighters.Clear();
                    }
                }
            }
        }

        #endregion

    }
}
