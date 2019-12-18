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
        public MainWindow()
        {
            DataContext = new FightersList();
            InitializeComponent();
        }

        private void FightersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var fightersList = DataContext as FightersList;
            fightersList.EditableData = FightersListBox.SelectedItem as FighterData;
        }

        private void ClubComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var fightersList = DataContext as FightersList;
            fightersList.ApplyFilter();
        }

        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var fightersList = DataContext as FightersList;
            fightersList.ApplyFilter();
        }
    }
}
