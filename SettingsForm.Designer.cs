
namespace CourseWork
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fullScreenCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.wireCheckBox = new System.Windows.Forms.CheckBox();
            this.vSyncCheckBox = new System.Windows.Forms.CheckBox();
            this.multisamplingComboBox = new System.Windows.Forms.ComboBox();
            this.environmentComboBox = new System.Windows.Forms.ComboBox();
            this.postProcessingComboBox = new System.Windows.Forms.ComboBox();
            this.particlesEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // fullScreenCheckBox
            // 
            this.fullScreenCheckBox.AutoSize = true;
            this.fullScreenCheckBox.Location = new System.Drawing.Point(12, 12);
            this.fullScreenCheckBox.Name = "fullScreenCheckBox";
            this.fullScreenCheckBox.Size = new System.Drawing.Size(102, 19);
            this.fullScreenCheckBox.TabIndex = 1;
            this.fullScreenCheckBox.Text = "Во весь экран";
            this.fullScreenCheckBox.UseVisualStyleBackColor = true;
            this.fullScreenCheckBox.CheckedChanged += new System.EventHandler(this.fullScreenCheckBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Сглаживание";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "Качество окружения";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 169);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 15);
            this.label5.TabIndex = 6;
            this.label5.Text = "Постобработка";
            // 
            // wireCheckBox
            // 
            this.wireCheckBox.AutoSize = true;
            this.wireCheckBox.Location = new System.Drawing.Point(203, 128);
            this.wireCheckBox.Name = "wireCheckBox";
            this.wireCheckBox.Size = new System.Drawing.Size(102, 19);
            this.wireCheckBox.TabIndex = 9;
            this.wireCheckBox.Text = "Режим линий";
            this.wireCheckBox.UseVisualStyleBackColor = true;
            this.wireCheckBox.CheckedChanged += new System.EventHandler(this.wireCheckBox_CheckedChanged);
            // 
            // vSyncCheckBox
            // 
            this.vSyncCheckBox.AutoSize = true;
            this.vSyncCheckBox.Location = new System.Drawing.Point(120, 12);
            this.vSyncCheckBox.Name = "vSyncCheckBox";
            this.vSyncCheckBox.Size = new System.Drawing.Size(190, 19);
            this.vSyncCheckBox.TabIndex = 10;
            this.vSyncCheckBox.Text = "Вертикальная синхронизация";
            this.vSyncCheckBox.UseVisualStyleBackColor = true;
            this.vSyncCheckBox.CheckedChanged += new System.EventHandler(this.vSyncCheckBox_CheckedChanged);
            // 
            // multisamplingComboBox
            // 
            this.multisamplingComboBox.FormattingEnabled = true;
            this.multisamplingComboBox.Items.AddRange(new object[] {
            "Нет",
            "2",
            "4",
            "8"});
            this.multisamplingComboBox.Location = new System.Drawing.Point(12, 66);
            this.multisamplingComboBox.Name = "multisamplingComboBox";
            this.multisamplingComboBox.Size = new System.Drawing.Size(121, 23);
            this.multisamplingComboBox.TabIndex = 11;
            this.multisamplingComboBox.SelectedIndexChanged += new System.EventHandler(this.multisamplingComboBox_SelectedIndexChanged);
            // 
            // environmentComboBox
            // 
            this.environmentComboBox.FormattingEnabled = true;
            this.environmentComboBox.Items.AddRange(new object[] {
            "Низкое",
            "Среднее",
            "Высокое"});
            this.environmentComboBox.Location = new System.Drawing.Point(12, 126);
            this.environmentComboBox.Name = "environmentComboBox";
            this.environmentComboBox.Size = new System.Drawing.Size(121, 23);
            this.environmentComboBox.TabIndex = 12;
            this.environmentComboBox.SelectedIndexChanged += new System.EventHandler(this.environmentComboBox_SelectedIndexChanged);
            // 
            // postProcessingComboBox
            // 
            this.postProcessingComboBox.FormattingEnabled = true;
            this.postProcessingComboBox.Items.AddRange(new object[] {
            "Низкое",
            "Среднее",
            "Высокое"});
            this.postProcessingComboBox.Location = new System.Drawing.Point(12, 187);
            this.postProcessingComboBox.Name = "postProcessingComboBox";
            this.postProcessingComboBox.Size = new System.Drawing.Size(121, 23);
            this.postProcessingComboBox.TabIndex = 13;
            this.postProcessingComboBox.SelectedIndexChanged += new System.EventHandler(this.postProcessingComboBox_SelectedIndexChanged);
            // 
            // particlesEnabledCheckBox
            // 
            this.particlesEnabledCheckBox.AutoSize = true;
            this.particlesEnabledCheckBox.Location = new System.Drawing.Point(203, 189);
            this.particlesEnabledCheckBox.Name = "particlesEnabledCheckBox";
            this.particlesEnabledCheckBox.Size = new System.Drawing.Size(74, 19);
            this.particlesEnabledCheckBox.TabIndex = 14;
            this.particlesEnabledCheckBox.Text = "Частицы";
            this.particlesEnabledCheckBox.UseVisualStyleBackColor = false;
            this.particlesEnabledCheckBox.CheckedChanged += new System.EventHandler(this.particlesEnabledCheckBox_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 225);
            this.Controls.Add(this.particlesEnabledCheckBox);
            this.Controls.Add(this.postProcessingComboBox);
            this.Controls.Add(this.environmentComboBox);
            this.Controls.Add(this.multisamplingComboBox);
            this.Controls.Add(this.vSyncCheckBox);
            this.Controls.Add(this.wireCheckBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.fullScreenCheckBox);
            this.Name = "SettingsForm";
            this.Text = "Настройки";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox fullScreenCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox wireCheckBox;
        private System.Windows.Forms.CheckBox vSyncCheckBox;
        private System.Windows.Forms.ComboBox multisamplingComboBox;
        private System.Windows.Forms.ComboBox environmentComboBox;
        private System.Windows.Forms.ComboBox postProcessingComboBox;
        private System.Windows.Forms.CheckBox particlesEnabledCheckBox;
    }
}