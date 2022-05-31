using System;
using System.Windows.Forms;
using OpenTK.Mathematics;

namespace CourseWork
{
    public partial class SettingsForm : Form
    {
        private Configuration _configuration;

        public SettingsForm(Configuration configuration)
        {
            InitializeComponent();
            _configuration = configuration;

            vSyncCheckBox.Checked = _configuration.VSync;
            multisamplingComboBox.SelectedIndex = _configuration.Graphic.MsaaLevel;
            environmentComboBox.SelectedIndex = (int) _configuration.Graphic.EnvironmentLevel;
            postProcessingComboBox.SelectedIndex = (int) _configuration.Graphic.PostProcessLevel;
            wireCheckBox.Checked = _configuration.Graphic.WireMode;
            fullScreenCheckBox.Checked = _configuration.FullScreen;
            particlesEnabledCheckBox.Checked = _configuration.Graphic.ParticlesEnabled;
        }

        private void vSyncCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.VSync = vSyncCheckBox.Checked;
        }

        private void multisamplingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _configuration.Graphic.MsaaLevel = multisamplingComboBox.SelectedIndex;
        }

        private void environmentComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _configuration.Graphic.EnvironmentLevel = (uint) environmentComboBox.SelectedIndex;
        }

        private void postProcessingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _configuration.Graphic.PostProcessLevel = (uint) postProcessingComboBox.SelectedIndex;
        }

        private void wireCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.Graphic.WireMode = wireCheckBox.Checked;
        }

        private void fullScreenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.FullScreen = fullScreenCheckBox.Checked;
        }

        private void particlesEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _configuration.Graphic.ParticlesEnabled = particlesEnabledCheckBox.Checked;
        }
    }
}