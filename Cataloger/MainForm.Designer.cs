namespace Cataloger
{
    partial class MainForm
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
            leftSideToolbar = new Cataloger.Controls.LeftSideToolbar();
            statutBar = new Cataloger.Controls.StatutBar();
            zoneTravail = new Panel();
            separatorControl = new DevExpress.XtraEditors.SeparatorControl();
            ((System.ComponentModel.ISupportInitialize)separatorControl).BeginInit();
            SuspendLayout();
            // 
            // leftSideToolbar
            // 
            leftSideToolbar.Dock = DockStyle.Left;
            leftSideToolbar.Location = new Point(0, 0);
            leftSideToolbar.Name = "leftSideToolbar";
       //     leftSideToolbar.NomComposante = "Le nom de la composante (BaseCtrl, propriéte NomComposante)";
            leftSideToolbar.Size = new Size(155, 634);
            leftSideToolbar.TabIndex = 0;
            // 
            // statutBar
            // 
            statutBar.Dock = DockStyle.Bottom;
            statutBar.Location = new Point(155, 550);
            statutBar.Name = "statutBar";
         //   statutBar.NomComposante = "Le nom de la composante (BaseCtrl, propriéte NomComposante)";
            statutBar.Size = new Size(1303, 84);
            statutBar.TabIndex = 1;
            // 
            // zoneTravail
            // 
            zoneTravail.Dock = DockStyle.Fill;
            zoneTravail.Location = new Point(155, 0);
            zoneTravail.Name = "zoneTravail";
            zoneTravail.Size = new Size(1303, 550);
            zoneTravail.TabIndex = 2;
            // 
            // separatorControl
            // 
            separatorControl.Dock = DockStyle.Top;
            separatorControl.LineThickness = 2;
            separatorControl.Location = new Point(155, 0);
            separatorControl.Margin = new Padding(0);
            separatorControl.Name = "separatorControl";
            separatorControl.Padding = new Padding(0);
            separatorControl.Size = new Size(1303, 2);
            separatorControl.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1458, 634);
            Controls.Add(separatorControl);
            Controls.Add(zoneTravail);
            Controls.Add(statutBar);
            Controls.Add(leftSideToolbar);
            Name = "MainForm";
            Text = "Cataloger";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)separatorControl).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Controls.LeftSideToolbar leftSideToolbar;
        private Controls.StatutBar statutBar;
        private Panel zoneTravail;
        private DevExpress.XtraEditors.SeparatorControl separatorControl;
    }
}
