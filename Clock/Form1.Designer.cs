﻿namespace Clock
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            clock1 = new Clock();
            SuspendLayout();
            // 
            // clock1
            // 
            clock1.Dock = DockStyle.Fill;
            clock1.Location = new Point(0, 0);
            clock1.Name = "clock1";
            clock1.Size = new Size(911, 740);
            clock1.TabIndex = 0;
            clock1.Text = "clock1";
            // 
            // Form1
            // 
            BackColor = Color.Black;
            ClientSize = new Size(911, 740);
            Controls.Add(clock1);
            Margin = new Padding(2, 3, 2, 3);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Clock clock1;
    }
}