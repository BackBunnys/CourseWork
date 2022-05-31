using System;
using System.Windows.Forms;

namespace CourseWork
{
    public partial class StartWindow : Form
    {
        private readonly Configuration _configuration;
        public StartWindow(Configuration configuration)
        {
            InitializeComponent();
            _configuration = configuration;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            new SettingsForm(_configuration).Show();
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            new HelpForm().Show();
        }
    }
}
