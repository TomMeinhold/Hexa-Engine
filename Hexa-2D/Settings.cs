using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            checkBox1.Checked = Properties.Settings.Default.Fullscreen;
            int i = 0;
            switch (Properties.Settings.Default.Width)
            {
                case 1920:
                    i = 0;
                    break;
                case 1280:
                    i = 1;
                    break;
                case 640:
                    i = 2;
                    break;
            }
            comboBox1.SelectedIndex = i;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    Properties.Settings.Default.Width = 1920;
                    Properties.Settings.Default.Height = 1080;
                    break;
                case 1:
                    Properties.Settings.Default.Width = 1280;
                    Properties.Settings.Default.Height = 720;
                    break;
                case 2:
                    Properties.Settings.Default.Width = 640;
                    Properties.Settings.Default.Height = 480;
                    break;
            }
            Properties.Settings.Default.Fullscreen = checkBox1.Checked;
            Properties.Settings.Default.Save();
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
