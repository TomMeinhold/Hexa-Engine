using HexaEngine.Core.Plugins;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace HexaEngineCreationKit.UIElements.Pages
{
    /// <summary>
    /// Interaction logic for PluginCreator.xaml
    /// </summary>
    public partial class PluginCreator : Page
    {
        private Plugin Plugin;

        public PluginCreator()
        {
            InitializeComponent();
            BuildInterface();
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "HexaEngine Plugin | *.hpln"
            };
            dialog.ShowDialog();
            Plugin.Save(new FileInfo(dialog.FileName));
            BuildInterface();
        }

        private void Button_Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "HexaEngine Plugin | *.hpln"
            };
            dialog.ShowDialog();
            Plugin = Plugin.Load(new FileInfo(dialog.FileName));
            BuildInterface();
        }

        private void BuildInterface()
        {
            if (Plugin is null)
            {
                Plugin = new Plugin();
            }

            References.Children.Clear();
            Scripts.Children.Clear();

            int refindex = 0;
            foreach (string str in Plugin.References)
            {
                TextBox box = new TextBox { Text = str };
                box.TextChanged += (ss, ee) => { Plugin.References[refindex] = box.Text; };
                References.Children.Add(box);
                refindex++;
            }

            int srcindex = 0;
            foreach (string str in Plugin.Scripts)
            {
                TextBox box = new TextBox { Text = str };
                box.TextChanged += (ss, ee) => { Plugin.Scripts[srcindex] = box.Text; };
                Scripts.Children.Add(box);
                refindex++;
            }

            RefAddTempTextBox();
            SrcAddTempTextBox();
        }

        private void RefAddTempTextBox()
        {
            TextBox refbox = new TextBox();
            void TextStage1(object s, EventArgs e)
            {
                refbox.TextChanged -= TextStage1;
                int index = Plugin.References.Count;
                Plugin.References.Add(refbox.Text);
                refbox.TextChanged += (ss, ee) => { Plugin.References[index] = refbox.Text; };
                RefAddTempTextBox();
            }
            refbox.TextChanged += TextStage1;
            References.Children.Add(refbox);
        }

        private void SrcAddTempTextBox()
        {
            TextBox srcbox = new TextBox();
            void TextStage1(object s, EventArgs e)
            {
                srcbox.TextChanged -= TextStage1;
                int index = Plugin.Scripts.Count;
                Plugin.Scripts.Add(srcbox.Text);
                srcbox.TextChanged += (ss, ee) => { Plugin.Scripts[index] = srcbox.Text; };
                SrcAddTempTextBox();
            }
            srcbox.TextChanged += TextStage1;
            Scripts.Children.Add(srcbox);
        }
    }
}