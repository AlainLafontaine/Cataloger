using BaseWinform.Controls;

namespace Cataloger.Controls
{
    partial class LeftSideToolbar
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private new void InitializeComponent()
        {
            btnSystemParam = new BtnNavigation();
            ctnBottom = new BaseContainer();
            ctnFill = new BaseContainer();
            btnTest = new BtnNavigation();
            btnPropect = new BtnNavigation();
            btnLibraryMngr = new BtnNavigation();
            ((System.ComponentModel.ISupportInitialize)ctnBottom).BeginInit();
            ctnBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ctnFill).BeginInit();
            ctnFill.SuspendLayout();
            SuspendLayout();
            // 
            // btnSystemParam
            // 
            btnSystemParam.Appearance.BackColor = SystemColors.Control;
            btnSystemParam.Appearance.BorderColor = Color.Transparent;
            btnSystemParam.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSystemParam.Appearance.ForeColor = SystemColors.ControlText;
            btnSystemParam.Appearance.Options.UseBackColor = true;
            btnSystemParam.Appearance.Options.UseBorderColor = true;
            btnSystemParam.Appearance.Options.UseFont = true;
            btnSystemParam.Appearance.Options.UseForeColor = true;
            btnSystemParam.AppearanceDisabled.BackColor = Color.FromArgb(248, 248, 248);
            btnSystemParam.AppearanceDisabled.BorderColor = Color.FromArgb(255, 255, 255);
            btnSystemParam.AppearanceDisabled.ForeColor = Color.FromArgb(128, 128, 128);
            btnSystemParam.AppearanceDisabled.Options.UseBackColor = true;
            btnSystemParam.AppearanceDisabled.Options.UseBorderColor = true;
            btnSystemParam.AppearanceDisabled.Options.UseForeColor = true;
            btnSystemParam.AppearanceHovered.BackColor = Color.FromArgb(221, 221, 221);
            btnSystemParam.AppearanceHovered.BorderColor = Color.FromArgb(221, 221, 221);
            btnSystemParam.AppearanceHovered.ForeColor = SystemColors.ControlText;
            btnSystemParam.AppearanceHovered.Options.UseBackColor = true;
            btnSystemParam.AppearanceHovered.Options.UseBorderColor = true;
            btnSystemParam.AppearanceHovered.Options.UseForeColor = true;
            btnSystemParam.AppearancePressed.BackColor = Color.FromArgb(204, 204, 204);
            btnSystemParam.AppearancePressed.BorderColor = Color.FromArgb(204, 204, 204);
            btnSystemParam.AppearancePressed.ForeColor = SystemColors.ControlText;
            btnSystemParam.AppearancePressed.Options.UseBackColor = true;
            btnSystemParam.AppearancePressed.Options.UseBorderColor = true;
            btnSystemParam.AppearancePressed.Options.UseForeColor = true;
            btnSystemParam.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            btnSystemParam.Location = new Point(16, 22);
            btnSystemParam.LookAndFeel.UseDefaultLookAndFeel = false;
            btnSystemParam.Name = "btnSystemParam";
            btnSystemParam.NavigationService = null;
            btnSystemParam.PermetPrecedent = false;
            btnSystemParam.Size = new Size(128, 47);
            btnSystemParam.TabIndex = 0;
            btnSystemParam.Text = "Parameters";
            btnSystemParam.TypeBtn = TypeBtnKind.none;
            btnSystemParam.URL = "systems-parameters";
            // 
            // ctnBottom
            // 
            ctnBottom.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            ctnBottom.Controls.Add(btnSystemParam);
            ctnBottom.Dock = DockStyle.Bottom;
            ctnBottom.Location = new Point(0, 363);
            ctnBottom.Name = "ctnBottom";
            ctnBottom.ShowCaption = false;
            ctnBottom.Size = new Size(160, 86);
            ctnBottom.TabIndex = 1;
            // 
            // ctnFill
            // 
            ctnFill.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            ctnFill.Controls.Add(btnLibraryMngr);
            ctnFill.Controls.Add(btnPropect);
            ctnFill.Controls.Add(btnTest);
            ctnFill.Dock = DockStyle.Fill;
            ctnFill.Location = new Point(0, 0);
            ctnFill.Name = "ctnFill";
            ctnFill.ShowCaption = false;
            ctnFill.Size = new Size(160, 363);
            ctnFill.TabIndex = 2;
            // 
            // btnTest
            // 
            btnTest.Appearance.BackColor = SystemColors.Control;
            btnTest.Appearance.BorderColor = Color.Transparent;
            btnTest.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnTest.Appearance.ForeColor = SystemColors.ControlText;
            btnTest.Appearance.Options.UseBackColor = true;
            btnTest.Appearance.Options.UseBorderColor = true;
            btnTest.Appearance.Options.UseFont = true;
            btnTest.Appearance.Options.UseForeColor = true;
            btnTest.AppearanceDisabled.BackColor = Color.FromArgb(248, 248, 248);
            btnTest.AppearanceDisabled.BorderColor = Color.FromArgb(255, 255, 255);
            btnTest.AppearanceDisabled.ForeColor = Color.FromArgb(128, 128, 128);
            btnTest.AppearanceDisabled.Options.UseBackColor = true;
            btnTest.AppearanceDisabled.Options.UseBorderColor = true;
            btnTest.AppearanceDisabled.Options.UseForeColor = true;
            btnTest.AppearanceHovered.BackColor = Color.FromArgb(221, 221, 221);
            btnTest.AppearanceHovered.BorderColor = Color.FromArgb(221, 221, 221);
            btnTest.AppearanceHovered.ForeColor = SystemColors.ControlText;
            btnTest.AppearanceHovered.Options.UseBackColor = true;
            btnTest.AppearanceHovered.Options.UseBorderColor = true;
            btnTest.AppearanceHovered.Options.UseForeColor = true;
            btnTest.AppearancePressed.BackColor = Color.FromArgb(204, 204, 204);
            btnTest.AppearancePressed.BorderColor = Color.FromArgb(204, 204, 204);
            btnTest.AppearancePressed.ForeColor = SystemColors.ControlText;
            btnTest.AppearancePressed.Options.UseBackColor = true;
            btnTest.AppearancePressed.Options.UseBorderColor = true;
            btnTest.AppearancePressed.Options.UseForeColor = true;
            btnTest.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            btnTest.Location = new Point(16, 185);
            btnTest.LookAndFeel.UseDefaultLookAndFeel = false;
            btnTest.Name = "btnTest";
            btnTest.NavigationService = null;
            btnTest.PermetPrecedent = false;
            btnTest.Size = new Size(128, 46);
            btnTest.TabIndex = 0;
            btnTest.Text = "Test";
            btnTest.TypeBtn = TypeBtnKind.none;
            btnTest.URL = "Tests";
            // 
            // btnPropect
            // 
            btnPropect.Appearance.BackColor = SystemColors.Control;
            btnPropect.Appearance.BorderColor = Color.Transparent;
            btnPropect.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPropect.Appearance.ForeColor = SystemColors.ControlText;
            btnPropect.Appearance.Options.UseBackColor = true;
            btnPropect.Appearance.Options.UseBorderColor = true;
            btnPropect.Appearance.Options.UseFont = true;
            btnPropect.Appearance.Options.UseForeColor = true;
            btnPropect.AppearanceDisabled.BackColor = Color.FromArgb(248, 248, 248);
            btnPropect.AppearanceDisabled.BorderColor = Color.FromArgb(255, 255, 255);
            btnPropect.AppearanceDisabled.ForeColor = Color.FromArgb(128, 128, 128);
            btnPropect.AppearanceDisabled.Options.UseBackColor = true;
            btnPropect.AppearanceDisabled.Options.UseBorderColor = true;
            btnPropect.AppearanceDisabled.Options.UseForeColor = true;
            btnPropect.AppearanceHovered.BackColor = Color.FromArgb(221, 221, 221);
            btnPropect.AppearanceHovered.BorderColor = Color.FromArgb(221, 221, 221);
            btnPropect.AppearanceHovered.ForeColor = SystemColors.ControlText;
            btnPropect.AppearanceHovered.Options.UseBackColor = true;
            btnPropect.AppearanceHovered.Options.UseBorderColor = true;
            btnPropect.AppearanceHovered.Options.UseForeColor = true;
            btnPropect.AppearancePressed.BackColor = Color.FromArgb(204, 204, 204);
            btnPropect.AppearancePressed.BorderColor = Color.FromArgb(204, 204, 204);
            btnPropect.AppearancePressed.ForeColor = SystemColors.ControlText;
            btnPropect.AppearancePressed.Options.UseBackColor = true;
            btnPropect.AppearancePressed.Options.UseBorderColor = true;
            btnPropect.AppearancePressed.Options.UseForeColor = true;
            btnPropect.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            btnPropect.Location = new Point(16, 21);
            btnPropect.LookAndFeel.UseDefaultLookAndFeel = false;
            btnPropect.Name = "btnPropect";
            btnPropect.NavigationService = null;
            btnPropect.PermetPrecedent = false;
            btnPropect.Size = new Size(128, 46);
            btnPropect.TabIndex = 1;
            btnPropect.Text = "Propect";
            btnPropect.TypeBtn = TypeBtnKind.none;
            btnPropect.URL = "propects";
            // 
            // btnLibraryMngr
            // 
            btnLibraryMngr.Appearance.BackColor = SystemColors.Control;
            btnLibraryMngr.Appearance.BorderColor = Color.Transparent;
            btnLibraryMngr.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLibraryMngr.Appearance.ForeColor = SystemColors.ControlText;
            btnLibraryMngr.Appearance.Options.UseBackColor = true;
            btnLibraryMngr.Appearance.Options.UseBorderColor = true;
            btnLibraryMngr.Appearance.Options.UseFont = true;
            btnLibraryMngr.Appearance.Options.UseForeColor = true;
            btnLibraryMngr.AppearanceDisabled.BackColor = Color.FromArgb(248, 248, 248);
            btnLibraryMngr.AppearanceDisabled.BorderColor = Color.FromArgb(255, 255, 255);
            btnLibraryMngr.AppearanceDisabled.ForeColor = Color.FromArgb(128, 128, 128);
            btnLibraryMngr.AppearanceDisabled.Options.UseBackColor = true;
            btnLibraryMngr.AppearanceDisabled.Options.UseBorderColor = true;
            btnLibraryMngr.AppearanceDisabled.Options.UseForeColor = true;
            btnLibraryMngr.AppearanceHovered.BackColor = Color.FromArgb(221, 221, 221);
            btnLibraryMngr.AppearanceHovered.BorderColor = Color.FromArgb(221, 221, 221);
            btnLibraryMngr.AppearanceHovered.ForeColor = SystemColors.ControlText;
            btnLibraryMngr.AppearanceHovered.Options.UseBackColor = true;
            btnLibraryMngr.AppearanceHovered.Options.UseBorderColor = true;
            btnLibraryMngr.AppearanceHovered.Options.UseForeColor = true;
            btnLibraryMngr.AppearancePressed.BackColor = Color.FromArgb(204, 204, 204);
            btnLibraryMngr.AppearancePressed.BorderColor = Color.FromArgb(204, 204, 204);
            btnLibraryMngr.AppearancePressed.ForeColor = SystemColors.ControlText;
            btnLibraryMngr.AppearancePressed.Options.UseBackColor = true;
            btnLibraryMngr.AppearancePressed.Options.UseBorderColor = true;
            btnLibraryMngr.AppearancePressed.Options.UseForeColor = true;
            btnLibraryMngr.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            btnLibraryMngr.Location = new Point(16, 73);
            btnLibraryMngr.LookAndFeel.UseDefaultLookAndFeel = false;
            btnLibraryMngr.Name = "btnLibraryMngr";
            btnLibraryMngr.NavigationService = null;
            btnLibraryMngr.PermetPrecedent = false;
            btnLibraryMngr.Size = new Size(128, 46);
            btnLibraryMngr.TabIndex = 2;
            btnLibraryMngr.Text = "Library";
            btnLibraryMngr.TypeBtn = TypeBtnKind.none;
            btnLibraryMngr.URL = "libraries-manager";
            // 
            // LeftSideToolbar
            // 
            Appearance.BackColor = SystemColors.Control;
            Appearance.Options.UseBackColor = true;
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ctnFill);
            Controls.Add(ctnBottom);
            Name = "LeftSideToolbar";
            Size = new Size(160, 449);
            Load += LeftSideToolbar_Load;
            ((System.ComponentModel.ISupportInitialize)ctnBottom).EndInit();
            ctnBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ctnFill).EndInit();
            ctnFill.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private BaseWinform.Controls.BtnNavigation btnSystemParam;
        private BaseContainer ctnBottom;
        private BaseContainer ctnFill;
        private BaseWinform.Controls.BtnNavigation btnTest;
        private BtnNavigation btnLibraryMngr;
        private BtnNavigation btnPropect;
    }
}
