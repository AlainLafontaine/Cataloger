using BaseWinform.Controls;
using BaseWinform.EventsArgs;
using DevExpress.Office.DigitalSignatures;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Resizing;
using System.ComponentModel;

namespace Ima.Windows.UserControls
{
    public partial class AideResolutionErreur : BaseCtrl
    {
        // ----- Événements -----

        [Category("BaseWinform")]
        [Description("Notifie le parent d'afficher le ChildComposante dans la Composante.")]
        public event EventHandler<AfficheChildComposanteEventArgs>? OnAfficherChildComposante;

        [Category("BaseWinform")]
        [Description("Notifie le parent que la liste des erreurs a été traité.")]
        public event EventHandler? OnFermerAideResolutionErreur;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("BaseWinform")]
        [Description("IterateurActif")]
        public bool IterateurActif { 
            get => iterateurActif; 
            set
            {
                iterateurActif = value;
                iterateurDataCtrl.Visible = value;
            }
        }

        private IEnumerable<ResolutionErreurInfo>? erreurs;
        private ResolutionErreurInfo? erreurActif;

        private bool iterateurActif = true;

        private Control? actifCtrl;
        public AideResolutionErreur()
        {
            InitializeComponent();
        }

        public void AddErreurs(IEnumerable<ResolutionErreurInfo> _erreurs)
        {
            iterateurDataCtrl.Clear();

            if (erreurs != null)
            {
                foreach (var erreur in this.erreurs!)
                {
                    if (erreur.Ctrl is Control ctrl)
                    {
                        if (ctrl.Name == nameof(TextEdit))
                        {
                            ((TextEdit)ctrl).KeyDown -= Ctrl_KeyDown!;
                        }
                        else if (erreur.Ctrl is BaseEdit be)
                        {
                            be.EditValueChanged -= Ctrl_EditValueChanged!;
                        }

                        ctrl.Enter -= Ctrl_Enter!;
                        ctrl.Paint -= Ctrl_AfficheContourErreur!;
                    }
                }
            }

            this.erreurs = _erreurs;
            lblValeurNbErreur.Text = this.erreurs.Count().ToString();

            foreach (var erreur in this.erreurs)
            {
                Control? ctrl = ObtenirControl(this.Parent!, erreur.NomControl);

                if (ctrl is null)
                {
                    throw new ArgumentNullException(erreur.NomControl);
                }

                if (ctrl.GetType() == typeof(TextEdit))
                {
                    ((TextEdit)ctrl!).KeyDown += Ctrl_KeyDown!;
                }
                else if (ctrl is BaseEdit be)
                {
                    be.EditValueChanged += Ctrl_EditValueChanged!;
                }
                else
                {
                    throw new NotImplementedException();
                }

                ctrl.Enter += Ctrl_Enter!;
                erreur.Ctrl = ctrl;
                iterateurDataCtrl.Add<ResolutionErreurInfo>(erreur, erreur.ErreurId);
            }

            iterateurDataCtrl.SelectedIndex = 0;
        }

        public void Clear()
        {
            iterateurDataCtrl.Clear();

            if (erreurs != null)
            {
                foreach (var erreur in this.erreurs!)
                {
                    if (erreur.Ctrl is Control ctrl)
                    {
                        if (ctrl.Name == nameof(TextEdit))
                        {
                            ((TextEdit)ctrl).KeyDown -= Ctrl_KeyDown!;
                        }
                        else if (ctrl is BaseEdit be)
                        {
                            be.EditValueChanged -= Ctrl_EditValueChanged!;
                        }

                        ctrl.Enter -= Ctrl_Enter!;
                        ctrl.Paint -= Ctrl_AfficheContourErreur!;
                    }
                }
            }

            erreurs = null;
        }

        private void iterateurDataCtrl_SelectedItemChanged(object sender, BaseWinform.EventsArgs.ItemChangedEventArgs e)
        {
            // Obtenir l'erreur et afficher la description de l'erreur
            var erreur = iterateurDataCtrl.ObtenirItem<ResolutionErreurInfo>(e.Index);

            if (actifCtrl is not null)
            {
                actifCtrl.Paint -= Ctrl_AfficheContourErreur!;
                actifCtrl.Refresh();
            }

            // Recherche le contrôle en erreur et guide l'utilisateur
            actifCtrl = erreur.Ctrl ?? (erreur.Ctrl = ObtenirControl(this.Parent!, erreur.NomControl)!);
            actifCtrl.Paint += Ctrl_AfficheContourErreur!;

            lblValeurMessage.Text = erreur.MessageErreur;
            OnAfficherChildComposante?.Invoke(this, new AfficheChildComposanteEventArgs(erreur.ChildComposanteType));

            if (erreur.CorrectionApplique)
            {
                BackColor = Color.FromArgb(128, 255, 128);
                lblValeurMessage.Text += " - (Ok)";
            }
            else
            {
                BackColor = Color.Salmon;
            }

            actifCtrl?.Select();
            erreurActif = erreur;
        }

        private void Ctrl_AfficheContourErreur(object sender, PaintEventArgs e)
        {
            Control ctrl = (Control)sender;
            ResolutionErreurInfo erreur = erreurs!.First(e => e.Ctrl == ctrl);

            using (Pen pen = erreur.CorrectionApplique ? new Pen(Color.Green, 2) : new Pen(Color.Red, 2))
            {
                e.Graphics.DrawRectangle(
                    pen,
                    new Rectangle(1, 1, ctrl.Width - 2, ctrl.Height - 2)
                );
            }
        }

        private void Ctrl_EditValueChanged(object sender, EventArgs e) {
            Control ctrl = (Control)sender;

            ctrl.Paint -= Ctrl_AfficheContourErreur!;
            if (ctrl is BaseEdit be)
            {
                be.EditValueChanged -= Ctrl_EditValueChanged!;
            }

            // Marquer que correction a été appliquée
            foreach (var erreur in erreurs!)
            {
                if (erreur.NomControl == ctrl.Name)
                {
                    erreur.CorrectionApplique = true;
                    break;
                }   
            }

            // Recherche l'index de la prochaine erreur
            int index = IndexProchaineErreur();

            if (index == -1)
                OnFermerAideResolutionErreur?.Invoke(sender, e);
            else 
                iterateurDataCtrl.SelectedIndex = index;
        }

        private void Ctrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (sender is BaseEdit be) 
                {
                    erreurActif!.CorrectionApplique = 
                        (be.EditValue is string s) ? s.Length != 0 : be.EditValue != null;

                    int index = IndexProchaineErreur();
                    
                    if (index != -1)
                    {
                        iterateurDataCtrl.SelectedIndex = index;
                    }
                }    
                
                e.SuppressKeyPress = false; 
            }
        }

        private void Ctrl_Enter(object sender, EventArgs e)
        {
            string name = ((Control)sender).Name;
            int index = erreurs!.Select((value, i) => new { value, i })
                                .FirstOrDefault(x => x.value.NomControl == name)?.i ?? -1;

            if (-1 != index && index != iterateurDataCtrl.SelectedIndex)
            {
                ResolutionErreurInfo erreur = erreurs!.FirstOrDefault(x => x.NomControl == name)!;

                if (erreur.Ctrl is BaseEdit be)
                {
                    erreur.CorrectionApplique = 
                        (be.EditValue is string s) ? s != string.Empty : 
                                                     be.EditValue != null;
                }

                iterateurDataCtrl.SelectedIndex = index;
            }
        }

        private void Ctrl_Leave(object sender, EventArgs e)
        {

        }

        private Control? ObtenirControl(Control parent, string nom)
        {
            Control? control = null;

            foreach (Control c in parent.Controls)
            {
                if (this == c) continue;

                if (c.Name == nom) { control = c; break; }

                // Recherche dans ses controls
                Control? c2 = ObtenirControl(c, nom);
                if (c2 != null) { control = c2; break; }
            }

            return control;
        }

        private int IndexProchaineErreur()
        {
            int index = 0;

            foreach (var erreur in erreurs!)
            {
                if (erreur.CorrectionApplique == false) break;
                index++;
            }

            return index == erreurs.Count() ? -1 : index;
        }
    }

    public class ResolutionErreurInfo 
    {
        public string ErreurId { get; set; }
        
        public string MessageErreur {  get; set; }

        public Type ChildComposanteType { get; set; }

        public string NomControl { get; set; }

        public Control? Ctrl { get; set; } = null;

        public bool CorrectionApplique { get; set; } = false;

        public ResolutionErreurInfo(
            string erreurId,
            string messageErreur,
            Type childComposanteType, 
            string nomControl
        )
        {
            ErreurId = erreurId;
            MessageErreur = messageErreur;
            ChildComposanteType = childComposanteType;
            NomControl = nomControl;
        }
    }
}
