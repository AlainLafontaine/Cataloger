using BaseWinform.Composantes;
using Cataloger.Presenters;
using Zzz.App.Core.Configuration;
using Zzz.App.Core.IoC;

namespace Cataloger.Composantes
{
    public delegate void BtnActionDisponibleHandler(bool disponible, TypeBouton[] typeBoutons);
    public delegate void BtnActionVisibleHandler(bool visible, bool disponible, TypeBouton[] typeBoutons);

    public partial class CatalogerChildComposante : ChildComposante
    {
        // Déclaration de l’événement
        public event BtnActionDisponibleHandler? btnActionDisponible = null;
        public event BtnActionVisibleHandler? btnActionVisible = null;

        protected IConfigurationApp? config { get => ObtenirParent<CatalogerComposante>()?.config; }
        protected SystemParameterService? systemParametersPresenter { get => ObtenirParent<CatalogerComposante>()?.systemParametersPresenter; }

        public CatalogerChildComposante()
        {
            
            InitializeComponent();
        }

        protected void BtnActionDisponible(bool disponible, TypeBouton[] typeBoutons)
        {
            btnActionDisponible!(disponible, typeBoutons);
        }

        protected void BtnActionVisible(bool visible, bool disponible, TypeBouton[] typeBoutons)
        {
            btnActionVisible!(visible, disponible, typeBoutons);
        }
    }
}
