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
            btnNavigation1 = new BtnNavigation();
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
            btnSystemParam.Location = new Point(16, 16);
            btnSystemParam.LookAndFeel.UseDefaultLookAndFeel = false;
            btnSystemParam.Name = "btnSystemParam";
            btnSystemParam.NavigationService = null;
            btnSystemParam.PermetPrecedent = false;
            btnSystemParam.Size = new Size(128, 32);
            btnSystemParam.TabIndex = 0;
            btnSystemParam.Text = "Paramètres";
            btnSystemParam.TypeBtn = TypeBtnKind.none;
            btnSystemParam.URL = "systems-parameters";
            // 
            // ctnBottom
            // 
            ctnBottom.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            ctnBottom.Controls.Add(btnSystemParam);
            ctnBottom.Dock = DockStyle.Bottom;
            ctnBottom.Location = new Point(0, 370);
            ctnBottom.Name = "ctnBottom";
            ctnBottom.ShowCaption = false;
            ctnBottom.Size = new Size(158, 79);
            ctnBottom.TabIndex = 1;
            // 
            // ctnFill
            // 
            ctnFill.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            ctnFill.Controls.Add(btnNavigation1);
            ctnFill.Dock = DockStyle.Fill;
            ctnFill.Location = new Point(0, 0);
            ctnFill.Name = "ctnFill";
            ctnFill.ShowCaption = false;
            ctnFill.Size = new Size(158, 370);
            ctnFill.TabIndex = 2;
            // 
            // btnNavigation1
            // 
            btnNavigation1.Appearance.BackColor = SystemColors.Control;
            btnNavigation1.Appearance.BorderColor = Color.Transparent;
            btnNavigation1.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnNavigation1.Appearance.ForeColor = SystemColors.ControlText;
            btnNavigation1.Appearance.Options.UseBackColor = true;
            btnNavigation1.Appearance.Options.UseBorderColor = true;
            btnNavigation1.Appearance.Options.UseFont = true;
            btnNavigation1.Appearance.Options.UseForeColor = true;
            btnNavigation1.AppearanceDisabled.BackColor = Color.FromArgb(248, 248, 248);
            btnNavigation1.AppearanceDisabled.BorderColor = Color.FromArgb(255, 255, 255);
            btnNavigation1.AppearanceDisabled.ForeColor = Color.FromArgb(128, 128, 128);
            btnNavigation1.AppearanceDisabled.Options.UseBackColor = true;
            btnNavigation1.AppearanceDisabled.Options.UseBorderColor = true;
            btnNavigation1.AppearanceDisabled.Options.UseForeColor = true;
            btnNavigation1.AppearanceHovered.BackColor = Color.FromArgb(221, 221, 221);
            btnNavigation1.AppearanceHovered.BorderColor = Color.FromArgb(221, 221, 221);
            btnNavigation1.AppearanceHovered.ForeColor = SystemColors.ControlText;
            btnNavigation1.AppearanceHovered.Options.UseBackColor = true;
            btnNavigation1.AppearanceHovered.Options.UseBorderColor = true;
            btnNavigation1.AppearanceHovered.Options.UseForeColor = true;
            btnNavigation1.AppearancePressed.BackColor = Color.FromArgb(204, 204, 204);
            btnNavigation1.AppearancePressed.BorderColor = Color.FromArgb(204, 204, 204);
            btnNavigation1.AppearancePressed.ForeColor = SystemColors.ControlText;
            btnNavigation1.AppearancePressed.Options.UseBackColor = true;
            btnNavigation1.AppearancePressed.Options.UseBorderColor = true;
            btnNavigation1.AppearancePressed.Options.UseForeColor = true;
            btnNavigation1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            btnNavigation1.Location = new Point(16, 16);
            btnNavigation1.LookAndFeel.UseDefaultLookAndFeel = false;
            btnNavigation1.Name = "btnNavigation1";
            btnNavigation1.NavigationService = null;
            btnNavigation1.PermetPrecedent = false;
            btnNavigation1.Size = new Size(130, 32);
            btnNavigation1.TabIndex = 0;
            btnNavigation1.Text = "Test";
            btnNavigation1.TypeBtn = TypeBtnKind.none;
            btnNavigation1.URL = "Tests";
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
        private BaseWinform.Controls.BtnNavigation btnNavigation1;
    }
}
