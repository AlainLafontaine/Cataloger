using BaseWinform.Controls;
using BaseWinform.EventsArgs;
using BaseWinform.Interfaces;
using BaseWinform.Utilitaires;

namespace BaseWinform.Composantes
{
    public delegate void EnvoyerMessageHandler(object sender, ChildComposanteMessageEventAgs e);

    public partial class ChildComposante : NavigationCtrl, IBaseComposante
    {
        public event EventHandler? InitComposante2;

        public List<ParametrePresenterURL> Parametres { get => ObtenirParent<BaseComposante>()!.Parametres; }
        public ITransfertData? TransfertData { get => ObtenirParent<BaseComposante>()!.TransfertData; }
        
        public void AcceptChanges() => ObtenirParent<BaseComposante>()!.AcceptChanges();
        public void RemiseAZeroIsDirty() => ObtenirParent<BaseComposante>()!.RemiseAZeroIsDirty();

        public ChildComposante()
        {
            InitializeComponent();
        }

        // Alain à destruire
        public virtual void MajEtatControl() {}
    }
}
