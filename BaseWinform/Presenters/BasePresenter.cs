using BaseWinform.EventsArgs;
using BaseWinform.Interfaces;
using BaseWinform.Utilitaires;
using DevExpress.Utils;

namespace BaseWinform.Presenters
{
    /// <summary>
    /// Fonctionnalité de commune aux Presenter
    /// </summary>
    /// <typeparam name="I"></typeparam> Le type de la composante
    public class BasePresenter<I> : IAddAndRemoveChildPresenter, IChildPresenterDataShared, IUrlPresenter, IDirtyPresenter, IBasePresenter<I> where I : IBaseComposante
    {
        public event EnvoyerCorrespondanceHandler? envoyerCorrespondance = null;

        /// <summary>
        /// Référence sur la composante (usercontrol Winform) associé à ce Presenter
        /// </summary>
        public I? Composante { get; private set; }

        public bool Initialise { get; private set; } = false;

        public string Url { get; set; } = string.Empty;

        public bool IsDirty { get; set; } = false;

        /// <summary>
        /// Référence sur les données transférées à ce Presenter
        /// </summary>
        public ITransfertData? TransfertData { get; set; } = null;

        /// <summary>
        /// Liste des paramètres extrait de l'url du Presenter
        /// </summary>
        public List<ParametrePresenterURL> Parametres { get; set; } = new List<ParametrePresenterURL>();

        public IChildPresenterDataShared ChildPresenterDataShared { get => this; }

        /// <summary>
        /// Contient les Presenter enfants associées à ce Presenter
        /// Key: Le nom de la composante enfant
        /// value: Référence sur la composante enfant à l'aide de l'interface IChildPresenter
        /// </summary>
        protected Dictionary<string, IChildPresenter> childPresenters = new ();

        private Dictionary<string, object> childPresenterDataShared = new ();

        /// <summary>
        /// Représente le Presenter de base.
        /// </summary>
        /// <param name="composante"></param> Reçoit une référence sur le usercontrol de Winform
        public BasePresenter(I composante) 
        { 
            this.Composante = composante;
            ((IBaseComposante)this.Composante).InitComposante2 += this.InitPresenter;

            if (typeof(IInitObtenirPresenter).IsAssignableFrom(this.Composante.GetType()))
            {
                ((IInitObtenirPresenter)this.Composante).ObtenirBasePresenter += this.ObtenirBasePresenter;
            }
        }

        public void InjectionDonneesNavigation(
            List<ParametrePresenterURL> parametres,
            ITransfertData? transfertData
        )
        {
            Parametres = parametres;
            TransfertData = transfertData;
        }

        /// <summary>
        /// Demande l'initialisation de la composante associé à ce Presenter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void InitPresenter(object? sender, EventArgs? e)
        {
            // Transfert la demande aux Presenter enfant
            foreach (IChildPresenter item in childPresenters.Values)
            {
                // Initialisation du Presenter enfant 
                item.InitPresenter(sender, e);
            }

            Initialise = true;
        }

        public virtual void ReleasePresenter()
        {
            Composante = default;

            foreach (IChildPresenter child in childPresenters.Values)
            {
                child.ReleasePresenter();
            }
            childPresenters.Clear();
        }

        public void AcceptChanges() => RemiseAZeroIsDirty();

        public void RemiseAZeroIsDirty()
        {
            IsDirty = false;
            Composante?.RemiseAZeroIsDirty();
        }

        public void ObtenirBasePresenter(object? sender, EventArgs e)
        {
            ((BasePresenterEventArgs)e).BasePresenter = this;
        }

        /// <summary>
        /// Ajoute un Presenter enfant de ce Presenter actif (la composante affiché par l'application)
        /// </summary>
        /// <param name="childPresenter"></param>
        public void Ajout(IChildPresenter childPresenter) => childPresenters.Add(childPresenter.GetType().Name, childPresenter);

        /// <summary>
        /// Retire un Presenter enfant de ce Presenter
        /// </summary>
        /// <param name="childPresenter"></param>
        public void Retire(IChildPresenter childPresenter)
        {
            childPresenters.Remove(childPresenter.GetType().Name);
        }

        public T? Obtenir<T>(string id)
        {
            object? data = default;
            childPresenterDataShared.TryGetValue(id, out data);
            return data != null ? (T)data : default;
        }

        public void Deposer<T>(string id, T t)
        {
            Guard.ArgumentNotNull(t, nameof(t));

            childPresenterDataShared[id] = (object)t!;
        }

        /// <summary>
        /// Demande de mettre à jour l'état des contôles de la composante
        /// Cette méthode est appellé en background lorsque l'application 
        /// n'est pas occupé
        /// </summary>
        public virtual void MajEtatControl()
        {
            // Transfert la demande aux Presenter enfant
            foreach (IChildPresenter item in childPresenters.Values)
            {
                if (item.Initialise)
                {
                    // Demande de mettre à jour l'état des contôles de la composante enfant
                    item.MajEtatControl();
                }
            }
        }

        public dynamic? ObtenirParametre<T>(string nomParametre)
        {
            ParametrePresenterURL? param = Parametres.Find(x => x.Nom.ToUpper() == nomParametre.ToUpper());
            dynamic? retour = null;

            if (param != null)
            {
                switch (typeof(T).Name)
                {
                    case "Int64":
                        retour = long.Parse(param?.Valeur ?? "");
                        break;

                    case "Double":
                        retour = Double.Parse(param?.Valeur ?? "");
                        break;

                    case "String":
                        retour = param?.Valeur ?? null;
                        break;

                    case "Boolean":
                        retour = Boolean.Parse(param?.Valeur ?? "false");
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            return retour;
        }

        public IEnumerable<IChildPresenter> ObtenirChildPresenterIterateur()
        {
            return childPresenters.Values.ToList<IChildPresenter>();
        }

        public void TransmettreCorrespondance(object sender, CorrespondanceEventArgs e)
        {
            envoyerCorrespondance -= ((IChildPresenter)sender).RecevoirCorrespondance;
            envoyerCorrespondance?.Invoke(sender, e);
            envoyerCorrespondance += ((IChildPresenter)sender).RecevoirCorrespondance;
        }

        /// <summary>
        /// Retour une composante enfant
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T ChildPresenter<T>() => (T)childPresenters[typeof(T).Name];
    }
}
