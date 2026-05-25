namespace Ima.Windows.UserControls
{
    partial class AideResolutionErreur
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
            lblNombreErreur = new DevExpress.XtraEditors.LabelControl();
            lblValeurNbErreur = new DevExpress.XtraEditors.LabelControl();
            lblMessage = new DevExpress.XtraEditors.LabelControl();
            lblValeurMessage = new DevExpress.XtraEditors.LabelControl();
            iterateurDataCtrl = new BaseWinform.Controls.IterateurDataCtrl();
            SuspendLayout();
            // 
            // lblNombreErreur
            // 
            lblNombreErreur.Anchor = AnchorStyles.Top;
            lblNombreErreur.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNombreErreur.Appearance.Options.UseFont = true;
            lblNombreErreur.Location = new Point(1258, 11);
            lblNombreErreur.Name = "lblNombreErreur";
            lblNombreErreur.Size = new Size(147, 23);
            lblNombreErreur.TabIndex = 0;
            lblNombreErreur.Text = "Nombre d'erreur:";
            // 
            // lblValeurNbErreur
            // 
            lblValeurNbErreur.Anchor = AnchorStyles.Top;
            lblValeurNbErreur.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblValeurNbErreur.Appearance.Options.UseFont = true;
            lblValeurNbErreur.Location = new Point(1411, 11);
            lblValeurNbErreur.Name = "lblValeurNbErreur";
            lblValeurNbErreur.Size = new Size(11, 23);
            lblValeurNbErreur.TabIndex = 1;
            lblValeurNbErreur.Text = "_";
            // 
            // lblMessage
            // 
            lblMessage.Anchor = AnchorStyles.Top;
            lblMessage.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMessage.Appearance.Options.UseFont = true;
            lblMessage.Location = new Point(14, 11);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(79, 23);
            lblMessage.TabIndex = 3;
            lblMessage.Text = "Message:";
            // 
            // lblValeurMessage
            // 
            lblValeurMessage.Anchor = AnchorStyles.Top;
            lblValeurMessage.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblValeurMessage.Appearance.Options.UseFont = true;
            lblValeurMessage.Location = new Point(99, 11);
            lblValeurMessage.Name = "lblValeurMessage";
            lblValeurMessage.Size = new Size(11, 23);
            lblValeurMessage.TabIndex = 4;
            lblValeurMessage.Text = "_";
            // 
            // iterateurDataCtrl
            // 
            iterateurDataCtrl.Anchor = AnchorStyles.Top;
            iterateurDataCtrl.Location = new Point(320, 49);
            iterateurDataCtrl.Name = "iterateurDataCtrl";
            iterateurDataCtrl.NbrPuce = 5;
            iterateurDataCtrl.SelectedIndex = -1;
            iterateurDataCtrl.Size = new Size(801, 47);
            iterateurDataCtrl.TabIndex = 5;
            iterateurDataCtrl.TypeAccesDirectItem = BaseWinform.Controls.TypeAccesDirectItem.Liste;
            iterateurDataCtrl.SelectedItemChanged += iterateurDataCtrl_SelectedItemChanged;
            // 
            // AideResolutionErreur
            // 
            Appearance.BackColor = Color.Salmon;
            Appearance.Options.UseBackColor = true;
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(iterateurDataCtrl);
            Controls.Add(lblValeurMessage);
            Controls.Add(lblMessage);
            Controls.Add(lblValeurNbErreur);
            Controls.Add(lblNombreErreur);
            Name = "AideResolutionErreur";
            Size = new Size(1436, 103);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblNombreErreur;
        private DevExpress.XtraEditors.LabelControl lblValeurNbErreur;
        private DevExpress.XtraEditors.LabelControl lblMessage;
        private DevExpress.XtraEditors.LabelControl lblValeurMessage;
        private BaseWinform.Controls.IterateurDataCtrl iterateurDataCtrl1;
        private BaseWinform.Controls.IterateurDataCtrl iterateurDataCtrl;
    }
}
