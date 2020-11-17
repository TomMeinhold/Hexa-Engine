using HexaEngineCreationKit.UIElements.Pages;
using System.Windows;

namespace HexaEngineCreationKit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Plugin_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PluginCreator());
        }

        private void Archive_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ArchiveCreator());
        }
    }
}