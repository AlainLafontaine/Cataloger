using BaseWinform.AccesAction;
using BaseWinform.Composantes;
using BaseWinform.Entites;
using BaseWinform.EventsArgs;
using BaseWinform.Interfaces;
using BaseWinform.Presenters;
using BaseWinform.Services;
using BaseWinform.Utilitaires;
using DevExpress.CodeParser;
using DevExpress.XtraEditors;

namespace BaseWinform.Forms
{
    public partial class BaseForm : XtraForm
    {
        protected NavigationService? navigationService = null;

        private dynamic? presenter;

        private IsDesignModeService? isDesignModeService = null;

        /* Version avec AlertControl
        private readonly AlertControl alertCtrl = new();
        private readonly List<Form> openAlerts = new(); */
        private readonly WinformNotifierManager? winformNotifierManager = null;
        private readonly System.Windows.Forms.Timer? afficheMsgHelp = null;
        private readonly List<(WinformActionMessage, string?)>? messageEnAttente = null;

        //private bool suspendApplication_Idle = false;

        public BaseForm() : base()
        {
            InitializeComponent();
        }

        public BaseForm(
            NavigationService navigationService,
            IsDesignModeService isDesignModeService
        ) : base()
        {
            // Service pour tester si nous sommes en mode design
            this.isDesignModeService = isDesignModeService;

            // sécurité supplémentaire
            ThrowSiDesignMode();

            // Initialisation de la form
            InitializeComponent();

            // Pour la notification des alertes
            winformNotifierManager = new WinformNotifierManager(this);

            /* Version avec AlertControl
            InitAlert();*/

            // Mise en place des connection pour l'envoie de message
            PresenterDirectAccessAction.transmettreWinformActionMessage += AfficherWinformActionMessage;
            NavigationService.transmettreWinformActionMessage += AfficherWinformActionMessage;

            // Affectation des services
            this.navigationService = navigationService;

            // Initialisation du help pour l'affichage des messages
            messageEnAttente = new();
            afficheMsgHelp = new() { Interval = 0_200 };
            afficheMsgHelp!.Tick += (s, e) =>
            {
                if (messageEnAttente?.Count > 0)
                {
                    (WinformActionMessage msg, string? title) prochainMsg = messageEnAttente.First();
                    messageEnAttente.Remove(prochainMsg);
                    winformNotifierManager?.Show(prochainMsg.msg, prochainMsg.title);
                }
                else
                {
                    //suspendApplication_Idle = false;
                    afficheMsgHelp.Stop();
                }
            };

            // Mettre en place le processus 
            Application.Idle += Application_Idle;
        }

        public void LoadPresenter(dynamic presenter)
        {
            BaseComposante composante = (BaseComposante)presenter.Composante;

            // Alain à supprimer Début
            composante.InjectionDonneesNavigation(presenter.Parametres, presenter.TransfertData);
            // Alain à supprimer Fin

            if (this.presenter != null)
            {
                ((BasePresenter)this.presenter).transmettreWinformActionMessage -= AfficherWinformActionMessage;
            }

            this.presenter = presenter;
            ((BasePresenter)this.presenter).transmettreWinformActionMessage += AfficherWinformActionMessage;

            // Création des ChildPresenters pour les ChildComposantes inclus en mode design
            List<ChildComposante> ctrls = GetAllCtrlOfType<ChildComposante>(composante);

            foreach (ChildComposante ctrl in ctrls)
            {
                Type? interfaceChildComposante = ObtenirChildPresenter(ctrl);

                if (interfaceChildComposante is not null)
                {
                    Type type = BaseComposante.childPresenters[interfaceChildComposante.Name];
                    IChildPresenter childPresenter = (IChildPresenter)PresenterDirectAccessAction.factory!.Create(type);

                    childPresenter.InjectionComposante(ctrl, presenter);
                    presenter.Ajout(childPresenter);
                    ((IBasePresenter)this.presenter).envoyerCorrespondance += childPresenter.RecevoirCorrespondance;
                }
            }

            NouvelleComposante(composante);
            composante.Dock = DockStyle.Fill;
            FixeNavigationServiceToCtrl<INavigationCtrl>(this);
        }

        public void RestorePresenter(dynamic presenter)
        {
            BaseComposante composante = (BaseComposante)presenter.Composante;

            if (this.presenter is not null)
            {
                foreach (IChildPresenter childPresenter in this.presenter.ObtenirChildPresenterIterateur())
                {
                    ((IBasePresenter)this.presenter).envoyerCorrespondance -= childPresenter.RecevoirCorrespondance;
                }

                ((BasePresenter)this.presenter).transmettreWinformActionMessage -= AfficherWinformActionMessage;
            }

            RestoreComposante(composante);
            this.presenter = presenter;
            ((BasePresenter)this.presenter).transmettreWinformActionMessage += AfficherWinformActionMessage;
            this.presenter.RestorePresenter();

            composante.Dock = DockStyle.Fill;
            FixeNavigationServiceToCtrl<INavigationCtrl>(this);
        }

        public void ShowNotification(WinformActionMessage message, string? title = null)
        {
            if (messageEnAttente?.Count == 0)
            {
                //suspendApplication_Idle = true;
                messageEnAttente.Add((message, title));
                afficheMsgHelp?.Start();
            }
            else
            {
                messageEnAttente?.Add((message, title));
            }
        }

        /* Version avec AlertControl
        public void ShowNotification(string titre, string message, Image? image = null, object? tag = null, int? ms = null)
        {
            var info = new AlertInfo(titre, message, image) { Tag = tag };

            if (ms.HasValue) alertCtrl.AutoFormDelay = ms.Value;
            alertCtrl.Show(this, info);
        }*/

        protected virtual void NouvelleComposante(BaseComposante composante) { throw new NotImplementedException(); }

        protected virtual void RestoreComposante(BaseComposante composante) { throw new NotImplementedException(); }

        protected void ThrowSiDesignMode()
        {
            if (IsDesignMode()) throw new NotSupportedException("Exécution en mode Design non supportée.");
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (IsDesignMode()) return;
            OnInitialized();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Idle -= Application_Idle;
        }

        protected virtual void OnInitialized()
        {
        }

        protected virtual void MajEtatControl()
        {
            if (presenter?.Initialise ?? false)
            {
                presenter?.MajEtatControl();
            }
        }

        protected void Titre(string titre) => Text = titre;

        protected bool IsDesignMode()
        {
            return isDesignModeService == null ? true : false;
        }

        private Type? ObtenirChildPresenter(ChildComposante childComposante)
        {
            Type type = childComposante.GetType();
            var baseInterfaces = type.BaseType?.GetInterfaces() ?? Array.Empty<Type>();

            Type? childPresenter = type.GetInterfaces()
                                            .Except(baseInterfaces)
                                            .Where(i => !i.IsGenericType && i != typeof(IChildComposante))
                                            .ToList().Find(type => typeof(IChildComposante).IsAssignableFrom(type));

            return childPresenter;
        }

        private void AfficherWinformActionMessage(WinformActionMessage msg)
        {
            ShowNotification(msg);
        }

        private void BaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        /* Version avec AlertControl
        private void InitAlert()
        {
            // Positionnement & comportement
            alertCtrl.FormLocation = AlertFormLocation.BottomRight; // bas-droit de l’écran
            alertCtrl.AutoFormDelay = 4000; // durée d’affichage en ms (0 = reste ouvert)
            alertCtrl.AllowHtmlText = true; // si tu veux formater le texte

            // (Facultatif) Style
            //alertCtrl.AppearanceCaption.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            //alertCtrl.AppearanceText.Font = new Font("Segoe UI", 9F);


            // Intercepter l'ouverture pour positionner MANUELLEMENT
            alertCtrl.BeforeFormShow += Alert_BeforeFormShow;
            
            // Hook quand le Form d’alerte est prêt
            alertCtrl.FormLoad += Alert_FormLoad;
            
            // Nettoyage sur fermeture
            alertCtrl.FormClosing += Alert_FormClosing;     

            // (Facultatif) Abonner aux clics
            alertCtrl.AlertClick += (s, e) =>
            {
                // Réagir au clic sur la notification
                // e.Info.Text / e.Info.Caption / e.Info.Tag disponibles
            };
        }

        private void Alert_FormLoad(object sender, AlertFormLoadEventArgs e)
        {
            // On garde la référence du Form d’alerte ouvert
            openAlerts.Add(e.AlertForm);
            
            // Tri par position Y (de bas en haut : plus grand Top = plus bas à l’écran)
            openAlerts.Sort((a, b) => a.Top.CompareTo(b.Top));
        }

        private void Alert_FormClosing(object sender, AlertFormClosingEventArgs e)
        {
            openAlerts.Remove(e.AlertForm);
        }

        private void Alert_BeforeFormShow(object sender, AlertFormEventArgs e)
        {
            // Rect client du Form en coordonnées écran (gère DPI/bordures)
            Rectangle ownerClientScreen = RectangleToScreen(this.ClientRectangle);
            Size alertSize = e.AlertForm.Size;

            const int margin = 12;

            // Position initiale : coin inférieur droit du Form (client)
            int x = ownerClientScreen.Right - alertSize.Width - margin;
            int y = ownerClientScreen.Bottom - alertSize.Height - margin;

            // Empilement : récupérer les alertes déjà visibles pour ce Form
            // On part du bas et on remonte au-dessus de la plus basse présente
            // en évitant tout chevauchement.
            // La dernière alerte (la plus basse) a le Top le plus grand.
            foreach (var af in openAlerts.OrderByDescending(f => f.Top))
            {
                var proposed = new Rectangle(new Point(x, y), alertSize);
                if (proposed.IntersectsWith(af.Bounds))
                {
                    // remonter au-dessus de cette alerte + marge
                    y = af.Top - margin - alertSize.Height;
                }
            }

            e.AlertForm.StartPosition = FormStartPosition.Manual;
            e.AlertForm.Location = new Point(0, 0);  // new Point(x, y);
        }
        Fin version avec AlertControl */

        private void FixeNavigationServiceToCtrl<T>(Control parent) where T : INavigationCtrl
        {
            ThrowSiDesignMode();
            List<T> ctrls = GetAllCtrlOfType<T>(this);

            foreach (T ctrl in ctrls)
            {
                ctrl.NavigationService = navigationService;
            }
        }

        private List<T> GetAllCtrlOfType<T>(Control parent)
        {
            ThrowSiDesignMode();
            var result = new List<T>();

            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is T btn) result.Add(btn);

                // Parcours récursif si le contrôle contient d'autres contrôles
                if (ctrl.HasChildren) result.AddRange(GetAllCtrlOfType<T>(ctrl));
            }

            return result;
        }

        // Code exécuté quand l'application est idle
        private void Application_Idle(object? sender, EventArgs e) => MajEtatControl();
    }
}
