namespace HexaEngineCreationKit.UIElements.Windows
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for VirualPathWindow.xaml
    /// </summary>
    public partial class VirtualPathWindow : Window
    {
        public VirtualPathWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}