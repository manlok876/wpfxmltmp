using System;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Resources;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Win32;

namespace WPF_XML_FL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FighterData EditableData { get; set; }
        private ObservableCollection<FighterData> Fighters { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            EditableData = new FighterData();

            _addCmd = new AddCmd(this);
            _selectCmd = new SelectCmd(this);
            _saveCmd = new SaveCmd(this);
            _loadCmd = new LoadCmd(this);
            _exitCmd = new ExitCmd(this);
        }


        #region Commands
        private void True_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        AddCmd _addCmd;
        public AddCmd AddCommand
        {
            get { return _addCmd; }
        }
        public class AddCmd : ICommand
        {
            MainWindow w;
            public AddCmd(MainWindow window)
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
            }
        }

        SelectCmd _selectCmd;
        public SelectCmd SelectCommand
        {
            get { return _selectCmd; }
        }
        public class SelectCmd : ICommand
        {
            MainWindow w;
            public SelectCmd(MainWindow window)
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
                w.EditableData = w.Fighters[(int) parameter];
            }
        }

        SaveCmd _saveCmd;
        public SaveCmd SaveCommand
        {
            get { return _saveCmd; }
        }
        public class SaveCmd : ICommand
        {
            MainWindow w;
            public SaveCmd(MainWindow window)
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
                saveFileDialog.Filter = "*.xml";
                if (saveFileDialog.ShowDialog() == true)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<FighterData>));
                    using (TextWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        serializer.Serialize(writer, w.Fighters);
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
            MainWindow w;
            public LoadCmd(MainWindow window)
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
                openFileDialog.Filter = "*.xml";
                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        XmlSerializer deserializer = new XmlSerializer(typeof(List<FighterData>));
                        using (TextReader reader = new StreamReader(openFileDialog.FileName))
                        {
                            object obj = deserializer.Deserialize(reader);
                            w.Fighters = obj as List<FighterData>;
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
                        w.Fighters = new List<FighterData>();
                    }
                }
            }
        }

        ExitCmd _exitCmd;
        public ExitCmd ExitCommand
        {
            get { return _exitCmd; }
        }
        public class ExitCmd : ICommand
        {
            MainWindow w;
            public ExitCmd(MainWindow window)
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
                w.Close();
            }
        }

        #endregion
    }
}
