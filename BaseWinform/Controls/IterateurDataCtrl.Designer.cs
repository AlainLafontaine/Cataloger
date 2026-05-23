namespace BaseWinform.Controls
{
    partial class IterateurDataCtrl
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
            btnPremier = new BtnSimpleBase();
            btnPrecedent = new BtnSimpleBase();
            btnDernier = new BtnSimpleBase();
            btnSuivant = new BtnSimpleBase();
            lblItemCourant = new DevExpress.XtraEditors.LabelControl();
            accesDirectDataCtrl = new BaseCtrl();
            SuspendLayout();
            // 
            // btnPremier
            // 
            btnPremier.Appearance.BackColor = SystemColors.Control;
            btnPremier.Appearance.BorderColor = Color.FromArgb(108, 117, 125);
            btnPremier.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPremier.Appearance.ForeColor = Color.Black;
            btnPremier.Appearance.Options.UseBackColor = true;
            btnPremier.Appearance.Options.UseBorderColor = true;
            btnPremier.Appearance.Options.UseFont = true;
            btnPremier.Appearance.Options.UseForeColor = true;
            btnPremier.AppearanceDisabled.BackColor = Color.Transparent;
            btnPremier.AppearanceDisabled.BorderColor = Color.FromArgb(182, 186, 190);
            btnPremier.AppearanceDisabled.ForeColor = Color.FromArgb(128, 128, 128);
            btnPremier.AppearanceDisabled.Options.UseBackColor = true;
            btnPremier.AppearanceDisabled.Options.UseBorderColor = true;
            btnPremier.AppearanceDisabled.Options.UseForeColor = true;
            btnPremier.AppearanceHovered.BackColor = Color.FromArgb(108, 117, 125);
            btnPremier.AppearanceHovered.BorderColor = Color.FromArgb(108, 117, 125);
            btnPremier.AppearanceHovered.ForeColor = Color.White;
            btnPremier.AppearanceHovered.Options.UseBackColor = true;
            btnPremier.AppearanceHovered.Options.UseBorderColor = true;
            btnPremier.AppearanceHovered.Options.UseForeColor = true;
            btnPremier.AppearancePressed.BackColor = Color.FromArgb(95, 103, 110);
            btnPremier.AppearancePressed.BorderColor = Color.FromArgb(95, 103, 110);
            btnPremier.AppearancePressed.ForeColor = Color.White;
            btnPremier.AppearancePressed.Options.UseBackColor = true;
            btnPremier.AppearancePressed.Options.UseBorderColor = true;
            btnPremier.AppearancePressed.Options.UseForeColor = true;
            btnPremier.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            btnPremier.Location = new Point(13, 13);
            btnPremier.LookAndFeel.UseDefaultLookAndFeel = false;
            btnPremier.Name = "btnPremier";
            btnPremier.Size = new Size(60, 34);
            btnPremier.TabIndex = 0;
            btnPremier.Text = "<<";
            btnPremier.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnPremier.TypeBtn = TypeBtnKind.btn_outline_secondary;
            btnPremier.Click += btnPremier_Click;
            // 
            // btnPrecedent
            // 
            btnPrecedent.Appearance.BackColor = SystemColors.Control;
            btnPrecedent.Appearance.BorderColor = Color.FromArgb(108, 117, 125);
            btnPrecedent.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPrecedent.Appearance.ForeColor = Color.Black;
            btnPrecedent.Appearance.Options.UseBackColor = true;
            btnPrecedent.Appearance.Options.UseBorderColor = true;
            btnPrecedent.Appearance.Options.UseFont = true;
            btnPrecedent.Appearance.Options.UseForeColor = true;
            btnPrecedent.AppearanceDisabled.BackColor = Color.Transparent;
            btnPrecedent.AppearanceDisabled.BorderColor = Color.FromArgb(182, 186, 190);
            btnPrecedent.AppearanceDisabled.ForeColor = Color.FromArgb(128, 128, 128);
            btnPrecedent.AppearanceDisabled.Options.UseBackColor = true;
            btnPrecedent.AppearanceDisabled.Options.UseBorderColor = true;
            btnPrecedent.AppearanceDisabled.Options.UseForeColor = true;
            btnPrecedent.AppearanceHovered.BackColor = Color.FromArgb(108, 117, 125);
            btnPrecedent.AppearanceHovered.BorderColor = Color.FromArgb(108, 117, 125);
            btnPrecedent.AppearanceHovered.ForeColor = Color.White;
            btnPrecedent.AppearanceHovered.Options.UseBackColor = true;
            btnPrecedent.AppearanceHovered.Options.UseBorderColor = true;
            btnPrecedent.AppearanceHovered.Options.UseForeColor = true;
            btnPrecedent.AppearancePressed.BackColor = Color.FromArgb(95, 103, 110);
            btnPrecedent.AppearancePressed.BorderColor = Color.FromArgb(95, 103, 110);
            btnPrecedent.AppearancePressed.ForeColor = Color.White;
            btnPrecedent.AppearancePressed.Options.UseBackColor = true;
            btnPrecedent.AppearancePressed.Options.UseBorderColor = true;
            btnPrecedent.AppearancePressed.Options.UseForeColor = true;
            btnPrecedent.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            btnPrecedent.Location = new Point(81, 13);
            btnPrecedent.LookAndFeel.UseDefaultLookAndFeel = false;
            btnPrecedent.Name = "btnPrecedent";
            btnPrecedent.Size = new Size(60, 34);
            btnPrecedent.TabIndex = 1;
            btnPrecedent.Text = "<";
            btnPrecedent.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnPrecedent.TypeBtn = TypeBtnKind.btn_outline_secondary;
            btnPrecedent.Click += btnPrecedent_Click;
            // 
            // btnDernier
            // 
            btnDernier.Appearance.BackColor = SystemColors.Control;
            btnDernier.Appearance.BorderColor = Color.FromArgb(108, 117, 125);
            btnDernier.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDernier.Appearance.ForeColor = Color.Black;
            btnDernier.Appearance.Options.UseBackColor = true;
            btnDernier.Appearance.Options.UseBorderColor = true;
            btnDernier.Appearance.Options.UseFont = true;
            btnDernier.Appearance.Options.UseForeColor = true;
            btnDernier.AppearanceDisabled.BackColor = Color.Transparent;
            btnDernier.AppearanceDisabled.BorderColor = Color.FromArgb(182, 186, 190);
            btnDernier.AppearanceDisabled.ForeColor = Color.FromArgb(128, 128, 128);
            btnDernier.AppearanceDisabled.Options.UseBackColor = true;
            btnDernier.AppearanceDisabled.Options.UseBorderColor = true;
            btnDernier.AppearanceDisabled.Options.UseForeColor = true;
            btnDernier.AppearanceHovered.BackColor = Color.FromArgb(108, 117, 125);
            btnDernier.AppearanceHovered.BorderColor = Color.FromArgb(108, 117, 125);
            btnDernier.AppearanceHovered.ForeColor = Color.White;
            btnDernier.AppearanceHovered.Options.UseBackColor = true;
            btnDernier.AppearanceHovered.Options.UseBorderColor = true;
            btnDernier.AppearanceHovered.Options.UseForeColor = true;
            btnDernier.AppearancePressed.BackColor = Color.FromArgb(95, 103, 110);
            btnDernier.AppearancePressed.BorderColor = Color.FromArgb(95, 103, 110);
            btnDernier.AppearancePressed.ForeColor = Color.White;
            btnDernier.AppearancePressed.Options.UseBackColor = true;
            btnDernier.AppearancePressed.Options.UseBorderColor = true;
            btnDernier.AppearancePressed.Options.UseForeColor = true;
            btnDernier.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            btnDernier.Location = new Point(522, 13);
            btnDernier.LookAndFeel.UseDefaultLookAndFeel = false;
            btnDernier.Name = "btnDernier";
            btnDernier.Size = new Size(60, 34);
            btnDernier.TabIndex = 3;
            btnDernier.Text = ">>";
            btnDernier.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDernier.TypeBtn = TypeBtnKind.btn_outline_secondary;
            btnDernier.Click += btnDernier_Click;
            // 
            // btnSuivant
            // 
            btnSuivant.Appearance.BackColor = SystemColors.Control;
            btnSuivant.Appearance.BorderColor = Color.FromArgb(108, 117, 125);
            btnSuivant.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSuivant.Appearance.ForeColor = Color.Black;
            btnSuivant.Appearance.Options.UseBackColor = true;
            btnSuivant.Appearance.Options.UseBorderColor = true;
            btnSuivant.Appearance.Options.UseFont = true;
            btnSuivant.Appearance.Options.UseForeColor = true;
            btnSuivant.AppearanceDisabled.BackColor = Color.Transparent;
            btnSuivant.AppearanceDisabled.BorderColor = Color.FromArgb(182, 186, 190);
            btnSuivant.AppearanceDisabled.ForeColor = Color.FromArgb(128, 128, 128);
            btnSuivant.AppearanceDisabled.Options.UseBackColor = true;
            btnSuivant.AppearanceDisabled.Options.UseBorderColor = true;
            btnSuivant.AppearanceDisabled.Options.UseForeColor = true;
            btnSuivant.AppearanceHovered.BackColor = Color.FromArgb(108, 117, 125);
            btnSuivant.AppearanceHovered.BorderColor = Color.FromArgb(108, 117, 125);
            btnSuivant.AppearanceHovered.ForeColor = Color.White;
            btnSuivant.AppearanceHovered.Options.UseBackColor = true;
            btnSuivant.AppearanceHovered.Options.UseBorderColor = true;
            btnSuivant.AppearanceHovered.Options.UseForeColor = true;
            btnSuivant.AppearancePressed.BackColor = Color.FromArgb(95, 103, 110);
            btnSuivant.AppearancePressed.BorderColor = Color.FromArgb(95, 103, 110);
            btnSuivant.AppearancePressed.ForeColor = Color.White;
            btnSuivant.AppearancePressed.Options.UseBackColor = true;
            btnSuivant.AppearancePressed.Options.UseBorderColor = true;
            btnSuivant.AppearancePressed.Options.UseForeColor = true;
            btnSuivant.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            btnSuivant.Location = new Point(454, 13);
            btnSuivant.LookAndFeel.UseDefaultLookAndFeel = false;
            btnSuivant.Name = "btnSuivant";
            btnSuivant.Size = new Size(60, 34);
            btnSuivant.TabIndex = 2;
            btnSuivant.Text = ">";
            btnSuivant.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSuivant.TypeBtn = TypeBtnKind.btn_outline_secondary;
            btnSuivant.Click += btnSuivant_Click;
            // 
            // lblItemCourant
            // 
            lblItemCourant.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblItemCourant.Appearance.Options.UseFont = true;
            lblItemCourant.Location = new Point(601, 14);
            lblItemCourant.Name = "lblItemCourant";
            lblItemCourant.Size = new Size(126, 23);
            lblItemCourant.TabIndex = 4;
            lblItemCourant.Text = "Aucun élément";
            lblItemCourant.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            // 
            // accesDirectDataCtrl
            // 
            accesDirectDataCtrl.Location = new Point(160, 3);
            accesDirectDataCtrl.Name = "accesDirectDataCtrl";
            accesDirectDataCtrl.Size = new Size(277, 48);
            accesDirectDataCtrl.TabIndex = 5;
            accesDirectDataCtrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            // 
            // IterateurDataCtrl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(accesDirectDataCtrl);
            Controls.Add(lblItemCourant);
            Controls.Add(btnDernier);
            Controls.Add(btnSuivant);
            Controls.Add(btnPrecedent);
            Controls.Add(btnPremier);
            Name = "IterateurDataCtrl";
            Size = new Size(764, 54);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private BtnSimpleBase btnPremier;
        private BtnSimpleBase btnPrecedent;
        private BtnSimpleBase btnDernier;
        private BtnSimpleBase btnSuivant;
        private DevExpress.XtraEditors.LabelControl lblItemCourant;
        private BaseCtrl accesDirectDataCtrl;
    }
}
