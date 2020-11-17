namespace HexaEngineCreationKit.UIElements.Pages
{
    using HexaEngine.Core.IO;
    using HexaEngineCreationKit.UIElements.Controls;
    using HexaEngineCreationKit.UIElements.Windows;
    using Microsoft.Win32;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;

    /// <summary>
    /// Interaction logic for ArchiveCreator.xaml
    /// </summary>
    public partial class ArchiveCreator : Page
    {
        public HexaEngineArchive Archive;

        public ArchiveCreator()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "HexaEngine Archive | *.hxa"
            };
            dialog.ShowDialog();
            Archive = HexaEngineArchive.Load(new FileInfo(dialog.FileName));
            BuildInterface();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Dictionary<string, FileInfo> files = new Dictionary<string, FileInfo>();
            foreach (var con in Files.Children)
            {
                if (con is VirtualPathFileControl control)
                {
                    if (control.FilePath.Text == string.Empty)
                    {
                        files.Add(control.VirualPath.Text, null);
                    }
                    else
                    {
                        files.Add(control.VirualPath.Text, new FileInfo(control.FilePath.Text));
                    }
                }
            }

            if (Archive is null)
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    Filter = "HexaEngine Archive | *.hxa"
                };
                dialog.ShowDialog();
                Archive = HexaEngineArchive.Pack(new FileInfo(dialog.FileName), files);
            }
            else
            {
                Archive.Update(files);
            }

            BuildInterface();
        }

        private void BuildInterface()
        {
            Files.Children.Clear();
            foreach (var entry in Archive.FileTable.TableEntries)
            {
                var element = new VirtualPathFileControl();
                element.VirualPath.Text = entry.VirtualPath;
                element.FilePath.IsEnabled = false;
                Files.Children.Add(element);
            }
        }

        private void Files_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                VirtualPathWindow window = new VirtualPathWindow();
                window.ShowDialog();
                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.
                foreach (var file in files)
                {
                    var element = new VirtualPathFileControl();
                    var filePath = new FileInfo(file);
                    element.VirualPath.Text = window.Path.Text + filePath.Name;
                    element.FilePath.Text = filePath.FullName;
                    element.FilePath.IsEnabled = false;
                    Files.Children.Add(element);
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            Archive?.Extract(new DirectoryInfo(dialog.SelectedPath));
        }
    }
}