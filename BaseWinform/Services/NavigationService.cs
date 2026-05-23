using AngleSharp.Dom;
using BaseWinform.AccesAction;
using BaseWinform.Attributes;
using BaseWinform.Entites;
using BaseWinform.Forms;
using BaseWinform.Interfaces;
using BaseWinform.Utilitaires;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;
using Zzz.App.Core.IoC;
using Zzz.App.Core.Types;

namespace BaseWinform.Services
{
    /// -----------------------------------------------------------------------
    /// <summary>
    /// Responsable de la gestion de la navigation de l'application 
    /// effectuer par une URL de destination.  La destination est un Presenter
    /// responsable de la gestion d'une composante (usercontrol)
    /// </summary>
    public class NavigationService : WinformService
    {
        /// <summary>
        /// Responsable de transmettre des messages suite à une action de 
        /// navigation
        /// </summary>
        static public Action<WinformActionMessage>? transmettreWinformActionMessage = null;

        /// <summary>
        /// Dictionnaire pour la gestion des url associée à une composante (usercontrol)
        /// </summary>
        static private Dictionary<string, Type> PresenterURLVersWinform { get; set; } = new Dictionary<string, Type>();


        /// <summary>
        /// Indique si on doit afficher les messages
        /// true: on affiche les messages
        /// false: on affiche pas les messages. On laisse la gestion
        /// à un autre niveau
        /// </summary>
        public bool AfficheMessage { get; set; }

        private IFactory factory;
        private BaseForm? mainForm;
        private dynamic? currentPresenter;
        private string currentPresenterURL = String.Empty;
        
        private List<string> presenterPrecedents = new List<string>();
        private List<dynamic> presenterPrecedents2 = new();

        /// <summary>
        /// Constructeur du service de navigation par url
        /// </summary>
        /// <param name="factory">
        /// Responsable de instanciation par DI des Presenter
        /// </param>
        /// <param name="restoreService">
        /// Responsable de rétablir l'état du Presenter après un retour
        /// au Presenter précédent
        /// </param>
        public NavigationService(
            IFactory factory
        ) 
        {
            PresenterDirectAccessAction.factory = factory;
            //this.restaureService = restoreService;
            this.factory = factory;
            AfficheMessage = true;
        }

        /// <summary>
        /// Affiche un message suite à une action
        /// </summary>
        /// <param name="msg"></param>
        public void AfficherMsg(WinformActionMessage msg)
        {
            transmettreWinformActionMessage?.Invoke(msg);
        }

        /// <summary>
        /// Permet d'obtenir la référence sur le factory
        /// </summary>
        /// <returns>
        /// La référence sur le factory
        /// </returns>
        public IFactory? ObtenirFactory() => factory;

        /// <summary>
        /// Initialisation du service de navigation.  Partir d'un assemblies, 
        /// on met en place les éléments essentiel pour permettre au service
        /// de replire sa responsabilité de gestion pour la navigation par url
        /// </summary>
        /// 
        /// <param name="mainForm">
        /// Réference sur la form principale de l'application. Nous avons une 
        /// seul WinForm dans l'application.  Les composantes et composantes
        /// enfants sont des usercontrols
        /// 
        /// <param name="pourPresenter">
        /// Le type des Presenters à rechercher par réflexion 
        ///</param>
        /// 
        /// <param name="assemblies">
        /// La liste des assemblies pour la recherche des Presenters par 
        /// réflexion
        /// </param>
        /// 
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void Init(
            BaseForm mainForm, 
            Type pourPresenter, 
            List<Assembly> assemblies
        )
        {
            this.mainForm = mainForm;
            
            // Navigation ayant comment cible un presenter
            PresenterURLVersWinform = new Dictionary<string, Type>();

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type item in TypeGetter.GetClassTypesByBaseType(pourPresenter, assembly))
                {
                    string[] siGeneric = pourPresenter.Name.Split('`');


                    if (item.Name != pourPresenter.Name)
                    {
                        var attribues = Attribute.GetCustomAttributes(item, typeof(PresenterURLAttribute));

                        if (attribues != null)
                        {
                            foreach (var attribue in attribues)
                            {
                                PresenterURLVersWinform.Add(((PresenterURLAttribute)attribue).PresenterURL, item);
                            }
                        }
                        else
                        {
                            throw new NotImplementedException($"Attribut PresenterURL n'est pas défini pour ce presenter {item.Name}");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Responsable de remet à zéro les références sur les Presenter
        /// précédent
        /// </summary>
        public void RemetAZeroPresenterPrecedents()
        {
            presenterPrecedents.Clear();

            foreach (dynamic presenter in presenterPrecedents2)
            {
                ReleasePresenter(presenter);
            }

            presenterPrecedents2.Clear();
        }

        /// <summary>
        /// Responsable du chargement et de l'affichage de la fênetre
        /// de départ de l'application.
        /// </summary>
        /// <param name="presenterURL">
        /// URL du Presenter pour la fenêtre de départ de l'application
        /// </param>
        public void ShowPremierePage(string presenterURL)
        {
            // Création du Presenter de démarrage
            currentPresenter = CreerPresenter(presenterURL, null, false);

            if (currentPresenter != null)
            {
                currentPresenterURL = presenterURL;
            }
            mainForm!.LoadPresenter(currentPresenter);
        }

        /// <summary>
        /// Responsable d'initier une navigation vers un nouveau Presenter
        /// </summary>
        /// <param name="presenterURL">
        /// URL du presenter à afficher à l'écran
        /// </param>
        /// <param name="data">
        /// Référence sur un objet à transmettre au Presenter qui va devenir
        /// le Presenter courant (le Presenter active). Un seul Presenter 
        /// active à la fois.  Le Presenter peut contenir des Presenter 
        /// enfant
        /// </param>
        /// <param name="permetPrecedent">
        /// Indique si l'on permet un retour au Presenter précédent.
        /// true: on permet sinon pas de retour au Presenter précédent
        /// </param>
        public void Naviguer(
            string presenterURL, 
            ITransfertData? data = null, 
            bool permetPrecedent = false
        )
        {
            if (GuardSiDirty(currentPresenter)) return;

            // Création du Presenter
            dynamic? presenter = CreerPresenter(presenterURL, data, false);

            if (presenter != null)
            {
                if (permetPrecedent)
                {
                    // Alain à supprimer
                    currentPresenterURL = presenterURL;
                    presenterPrecedents.Add(currentPresenterURL);
                    // Alain à supprimer fin

                    presenterPrecedents2.Add(currentPresenter);
                }
                else
                {
                    ReleasePresenter(currentPresenter);
                    foreach (var p in presenterPrecedents2.Reverse<dynamic>()) { ReleasePresenter(p); }
                    presenterPrecedents2.Clear();
                }

                currentPresenter = presenter;
                mainForm!.LoadPresenter(currentPresenter);
            }
        }

        /// <summary>
        /// Responsable d'initier une navigation vers le Presenter précédent
        /// </summary>
        /// <param name="data">
        ///  Référence sur un objet à transmettre au Presenter précédent.
        /// </param>
        public void Precedent(ITransfertData? data)
        {
            // Vérifie si des données doit-être sauvegarder
            if (GuardSiDirty(currentPresenter)) return;

            // Chargement du presenter et de la composante associée
            dynamic? presenter = presenterPrecedents2[^1];
            
            presenterPrecedents.RemoveAt(presenterPrecedents.Count - 1);
            presenterPrecedents2.RemoveAt(presenterPrecedents2.Count - 1);

            ReleasePresenter(currentPresenter);
            currentPresenter = presenter;
            mainForm!.RestorePresenter(presenter);
        }

        private dynamic? CreerPresenter(
            string presenterURL,
            ITransfertData? data,
            bool restaureFormState
        )
        {
            // Recherche le contrôle a instancier selon url passé en paramètre
            Type? type = null;
            List<ParametrePresenterURL>? parametres;
            dynamic? presenter = null;

            if (RechercherPresenter(PresenterURLVersWinform, presenterURL, out type, out parametres))
            {
                presenter = factory!.Create(type);

                if (presenter != null)
                {
                    presenter.InjectionDonneesNavigation(parametres, data);
                    presenter.Url = presenterURL;
                }
                else
                {
                    // Alain To do   Problème - un exception

                }
            }

            return presenter;
        }

        /// <summary>
        /// Recherche le Presenter à instancier selon url passé en paramètre
        /// </summary>
        /// <returns>
        /// true si nous avons trouvé le Presenter à instancier
        /// </returns>
        private bool RechercherPresenter(
            Dictionary<string, Type> URLVers,
            string presenterURL,
            out Type? typeOfPresenter,
            out List<ParametrePresenterURL> parametres
        )
        {
            string[] subURLs = presenterURL.Split('/');
            string keyTrouvee = "";
            
            parametres = new List<ParametrePresenterURL>();

            foreach (string key in URLVers!.Keys)
            {
                string[] subKeys = key.Split("/");

                if (subKeys.Length == subURLs.Length)
                {
                    for (int index = 0; index < subKeys.Length; index++)
                    {
                        if (subKeys[index][0] == '{')
                        {
                            keyTrouvee += (index == 0) ? subKeys[index] : $"/{subKeys[index]}";

                            // Extraction du paramètres
                            string text = subKeys[index];
                            var match = Regex.Matches(text, @"\{([^}]*)\}")[0];
                            string nomParametre = match.Groups[1].Value;

                            parametres.Add(new ParametrePresenterURL(nomParametre, subURLs[index]));
                        }
                        else
                        {
                            if (subURLs[index].ToLower() == subKeys[index].ToLower())
                            {
                                keyTrouvee += (index == 0) ? subKeys[index] : $"/{subKeys[index]}";
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    if (keyTrouvee.ToLower() == key.ToLower())
                    {
                        break;
                    }
                    else
                    {
                        keyTrouvee = "";
                        parametres.Clear();
                    }
                }
            }

            // Vérifie si la clé existe dans le dictionnaire
            if (!URLVers.TryGetValue(keyTrouvee, out typeOfPresenter))
            {
                 AfficherMsg(new WinformBadRequestActionMessage($"Aucune composante trouvée pour '{presenterURL}'."));
                 return false;
            }

            return true;
        }

        private void ReleasePresenter(dynamic presenter)
        {
            foreach (IChildPresenter childPresenter in presenter.ObtenirChildPresenterIterateur())
            {
                ((IBasePresenter)presenter).envoyerCorrespondance -= childPresenter.RecevoirCorrespondance;
            }

            presenter.ReleasePresenter();
        }
        
        private bool GuardSiDirty(dynamic? presenter)
        {
            if (presenter != null && presenter?.IsDirty)
            {
                // Demander confirmation avant de fermer
                var result = MessageBox.Show("Des données ne sont pas engistrées. Voulez-vous vraiment quitter ?", "Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
