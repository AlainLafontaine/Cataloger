using BaseWinform.Composantes;
using DevExpress.CodeParser;
using Cataloger.Presenters;
using Zzz.App.Core.Configuration;

namespace Cataloger.Composantes
{
    public enum TypeBouton
    {
        Enregistrer,
        Annuller,
        Supprimer,
        Precedent
    }

    public partial class CatalogerComposante : BaseComposante
    {
        public IConfigurationApp? config = null;
        public SystemParameterService? systemParametersPresenter = null;
        
        public CatalogerComposante()
        {
            InitializeComponent();
        }

        public void InitPresenterEtServiceEssentiel(
            IConfigurationApp config,
            SystemParameterService systemParametersPresenter
        )
        {
            this.config = config;
            this.systemParametersPresenter = systemParametersPresenter;
        }

        public override void MajEtatControl() 
        {
        }

        // Injecte notre collection
        protected override ControlCollection CreateControlsInstance() => new ImaControlCollection(this);

        protected void BtnActionDisponible(bool disponible, TypeBouton[] typeBoutons)
        {
            foreach (var item in typeBoutons)
            {
                switch (item)
                {
                    case TypeBouton.Enregistrer:
                        btnEnregistrer.Enabled = disponible;
                        break;

                    case TypeBouton.Annuller:
                        btnAnnuler.Enabled = disponible;
                        break;

                    case TypeBouton.Supprimer:
                        btnSupprimer.Enabled = disponible;
                        break;

                    case TypeBouton.Precedent:
                        btnPrecedent.Enabled = disponible;
                        break;
                }
            }
        }

        protected void BtnActionVisible(bool visible, bool disponible, TypeBouton[] typeBoutons)
        {
            foreach (var item in typeBoutons)
            {
                switch (item)
                {
                    case TypeBouton.Enregistrer:
                        btnEnregistrer.Visible = visible;
                        btnEnregistrer.Enabled = disponible;
                        break;

                    case TypeBouton.Annuller:
                        btnAnnuler.Visible = visible;
                        btnAnnuler.Enabled = disponible;
                        break;

                    case TypeBouton.Supprimer:
                        btnSupprimer.Visible = visible;
                        btnSupprimer.Enabled = disponible;
                        break;

                    case TypeBouton.Precedent:
                        btnPrecedent.Visible = visible;
                        btnPrecedent.Enabled = disponible;
                        break;
                }
            }
        }

        private void ImaComposante_Leave(object sender, EventArgs e)
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.GetType() == typeof(CatalogerChildComposante))
                {
                    CatalogerChildComposante child = (CatalogerChildComposante)ctrl;

                    child.btnActionDisponible -= BtnActionDisponible;
                    child.btnActionVisible -= BtnActionVisible;
                }
            }
        }

        // La collection personnalisée
        private class ImaControlCollection : BaseControlCollection
        {
            public ImaControlCollection(Control owner) : base(owner) { }

            private CatalogerComposante OwnerCompo => (CatalogerComposante)Owner;

            public override void Add(Control? value)
            {
                if (value == null) return;

                if (value is CatalogerChildComposante)
                {
                    CatalogerChildComposante child = (CatalogerChildComposante)value;

                    child.btnActionDisponible += OwnerCompo.BtnActionDisponible;
                    child.btnActionVisible += OwnerCompo.BtnActionVisible;
                }
                else {
                    List<CatalogerChildComposante> ctrls = OwnerCompo.GetAllCtrlOfType<CatalogerChildComposante>(value);

                    foreach (CatalogerChildComposante ctrl in ctrls)
                    {
                        ctrl.btnActionDisponible += OwnerCompo.BtnActionDisponible;
                        ctrl.btnActionVisible += OwnerCompo.BtnActionVisible;
                    }
                }


                base.Add(value);
            }

            public override void Remove(Control? value) => base.Remove(value); // ou interdire : throw new NotSupportedException(...)
        }
    }
}
