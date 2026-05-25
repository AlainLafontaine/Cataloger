using BaseWinform.Composantes;
using BaseWinform.Entites;
using BaseWinform.EventsArgs;
using BaseWinform.Interfaces;
using BaseWinform.Utilitaires;
using Zzz.App.Core.IoC;

namespace BaseWinform.Presenters
{
    public class ChildPresenter<I> : IChildPresenter, IBasePresenter<I> where I : IBaseComposante
    {
        public event EnvoyerCorrespondanceHandler? envoyerCorrespondance;

        public I Composante
        {
            get => (_composante != null) ? _composante : throw new NotImplementedException();
            private set => _composante = value;
        }

        public bool Initialise { get; private set; } = false;

        public dynamic Parent
        {
            get => (_parent != null) ? _parent : throw new NotImplementedException();
            private set => _parent = value;
        }

        /// <summary>
        /// Référence sur les données transférées à ce Presenter
        /// </summary>
        public ITransfertData? TransfertData { get => Parent.TransfertData; }
        
        public ITransfertData? RetourneData { get => Parent.TransfertData; }


        /// <summary>
        /// Liste des paramètres extrait de l'url du Presenter
        /// </summary>
        public List<ParametrePresenterURL> Parametres { get => Parent.Parametres;  }

        public IChildPresenterDataShared ChildPresenterDataShared { get => Parent; }


        protected dynamic? _parent = default;
        private I? _composante = default;

        public ChildPresenter() { }

        public void InjectionComposante(object composante, object parent)
        {
            this.Composante = (I)composante;
            this._parent = parent;
        }

        #region Méthode de l'interface - IChildPresenter -

        public virtual void InitPresenter(object? sender, EventArgs? e) { Initialise = true; }

        public virtual void ReleasePresenter() { _composante = default; }

        public virtual void RestorePresenter() { }

        public void AfficherMsg(WinformActionMessage msg) => Parent.AfficherMsg(msg);

        public void RemiseAZeroIsDirty() => Parent?.RemiseAZeroIsDirty();

        public void AcceptChanges() => RemiseAZeroIsDirty();

        public virtual void MajEtatControl() { }

        public virtual void RecevoirCorrespondance(object sender, CorrespondanceEventArgs e) { }

        /// <summary>
        /// Envoie via le parent un message aux autres ChildPresenter
        /// </summary>
        /// <param name="e"></param>
        public void EnvoyerCorrespondance(CorrespondanceEventArgs e)
        {
            Parent.TransmettreCorrespondance(this, e);
        }
        #endregion
    }
}
