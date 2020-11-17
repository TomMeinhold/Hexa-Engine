namespace HexaEngineCreationKit.UIElements.Controls
{
    using Microsoft.Win32;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for VirtualPathFileControl.xaml
    /// </summary>
    public partial class VirtualPathFileControl : UserControl
    {
        public VirtualPathFileControl()
        {
            InitializeComponent();
        }

        private void FilePath_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void VirualPath_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void FilePath_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                CheckFileExists = true
            };
            dialog.ShowDialog();
        }

        private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ((StackPanel)Parent).Children.Remove(this);
        }
    }
}