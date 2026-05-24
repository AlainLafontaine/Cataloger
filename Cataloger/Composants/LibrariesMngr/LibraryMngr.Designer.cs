namespace Cataloger.Composants
{
    partial class LibraryMngr
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private new void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Name = "LibraryMngr";
            Size = new Size(800, 450);
            ResumeLayout(false);
        }
    }
}
