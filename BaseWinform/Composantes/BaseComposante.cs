using BaseWinform.Controls;
using BaseWinform.EventsArgs;
using BaseWinform.Interfaces;
using BaseWinform.Services;
using BaseWinform.Utilitaires;
using DevExpress.XtraEditors;
using System.ComponentModel;
using System.Reflection;


namespace BaseWinform.Composantes
{
    public partial class BaseComposante : NavigationCtrl, IBaseComposante, IInjectionDonneeesNavigation
    {
        static public Dictionary<string, Type> childPresenters = new();

        public static bool HasEventHandler(Control control, string eventName)
        {
            // Accède à la propriété interne "Events"
            PropertyInfo? eventsProperty = typeof(Control)
                .GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);

            EventHandlerList? eventHandlerList =
                (EventHandlerList?)eventsProperty?.GetValue(control) ?? null;

            // Récupère la clé interne de l'événement
            var eventKeyField = typeof(Control)
                .GetField("Event" + eventName,
                    BindingFlags.Static | BindingFlags.NonPublic);

            if (eventKeyField == null)
                return false;

            object? eventKey = eventKeyField.GetValue(null);

            return eventKey != null ? eventHandlerList?[eventKey] != null : false;
        }


    // Pour l'initialisation et libération de la composante par le presenter
    public event EventHandler? InitComposante2;
        
        // Permet d'obtenir le BasePresenter
        // Est utilisé lorsque l'on ajoute un ChildComposante dans le BaseComposante
        // Va permetre de configurer correctement le ChildPresente du ChildComposante
        public event EventHandler? ObtenirBasePresenter;

        // Alain à destruire
        public ITransfertData? TransfertData { get; set; } = null;

        // Alain à destruire
        public List<ParametrePresenterURL> Parametres { get; set; } = new List<ParametrePresenterURL>();

        private bool initializing = false;

        public BaseComposante()
        {
            InitializeComponent();
        }

        // Alain à destruire
        public virtual void InitComposante() { }

        // Alain à destruire
        public void InjectionDonneesNavigation(
            List<ParametrePresenterURL> parametres,
            ITransfertData? transfertData
        )
        {
            if (!IsDesignMode())
            {
                Parametres = parametres;
                TransfertData = transfertData;
            }
        }

        public virtual void MajEtatControl() { }

        public void RemiseAZeroIsDirty()
        {
            BasePresenterEventArgs args = new BasePresenterEventArgs();

            ObtenirBasePresenter?.Invoke(this, args);
            if (args.BasePresenter != null)
            {
                IDirtyPresenter dirtyPresenter = (IDirtyPresenter)args.BasePresenter;

                dirtyPresenter.IsDirty = false;
            }

            ResetEditorsModified(this);
        }

        // Méthode publique pour "revalider" l'état après un Save
        public void AcceptChanges() =>  RemiseAZeroIsDirty();

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

        // Injecte notre collection
        protected override ControlCollection CreateControlsInstance() => new BaseControlCollection(this);

        protected void RestoreChildComposante(ChildComposante childComposante)
        {
            ((BaseControlCollection)Controls).Restore(childComposante);
        }

        private void BaseComposante_Load(object sender, EventArgs e)
        {
            WireAllEditors(this);

            // Initialisation de la composante
            InitComposante2?.Invoke(sender, e);
        }

        // ---- Implémentation utilitaire ----
        private void WireAllEditors(Control parent)
        {
            BasePresenterEventArgs args = new BasePresenterEventArgs();

            ObtenirBasePresenter?.Invoke(this, args);
            if (args.BasePresenter != null)
            {
                IDirtyPresenter dirtyPresenter = (IDirtyPresenter)args.BasePresenter;

                foreach (Control c in parent.Controls)
                {
                    if (c is BaseEdit be)
                    {
                        be.EditValueChanged += (s, e) =>
                        {
                            if (!dirtyPresenter.IsDirty)
                            {
                                if (initializing) return;

                                if (!BaseEditIgnoreIsDirtyHelper.GetIgnoreIsDirty((BaseEdit)c))
                                {
                                    dirtyPresenter.IsDirty = true;
                                }
                            }
                        };
                    }

                    if (c is CheckedListBoxControl beCLBC)
                    {
                        beCLBC.ItemCheck += (s, e) =>
                        {
                            if (!dirtyPresenter.IsDirty)
                            {
                                if (initializing) return;
                                dirtyPresenter.IsDirty = true;
                            }
                        };
                    }

                    if (c.HasChildren)
                        WireAllEditors(c);
                }
            }
        }

        private bool AnyEditorModified()
        {
            return GetAllEditors(this).Any(be => be.IsModified);
        }

        private IEnumerable<BaseEdit> GetAllEditors(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is BaseEdit be)
                    yield return be;
                if (c.HasChildren)
                {
                    foreach (var child in GetAllEditors(c))
                        yield return child;
                }
            }
        }

        private void ResetEditorsModified(Control parent)
        {
            foreach (var be in GetAllEditors(parent))
            {
                // Quelques éditeurs exposent IsModified en lecture seule.
                // Astuce : forcer une validation/commit du value cycle.
                be.DoValidate();
                // Autre astuce si nécessaire : re-fixer EditValue à lui-même pour "commiter"
                // var v = be.EditValue; be.EditValue = v;
            }
        }

        private void BeginUpdateAllEditors(Control parent)
        {
            foreach (var be in GetAllEditors(parent))
                be.Properties.BeginUpdate();
        }

        private void EndUpdateAllEditors(Control parent)
        {
            foreach (var be in GetAllEditors(parent))
                be.Properties.EndUpdate();
        }

        // La collection personnalisée
        protected class BaseControlCollection : ControlCollection
        {
            public BaseControlCollection(Control owner) : base(owner) { }

            private BaseComposante OwnerCompo => (BaseComposante)Owner;

            public override void Add(Control? value)
            {
                if (value == null) return;
                
                if (!OwnerCompo.IsDesignMode())
                {
                    if (value is ChildComposante)
                    {
                        CreerChildPresenter((ChildComposante)value);
                    }
                    else
                    {
                        List<ChildComposante> ctrls = OwnerCompo.GetAllCtrlOfType<ChildComposante>(value);

                        foreach (ChildComposante ctrl in ctrls)
                        {
                            CreerChildPresenter(ctrl);
                        }
                    }
                }

                base.Add(value);
            }

            public override void Remove(Control? value)
            {

                base.Remove(value);
            }

            public void Restore(Control? value)
            {
                base.Add(value);
            }

            private Type? ObtenirChildPresenter(ChildComposante childComposante )
            {
                Type type = childComposante.GetType();
                var baseInterfaces = type.BaseType?.GetInterfaces() ?? Array.Empty<Type>();

                Type? childPresenter = type.GetInterfaces()
                                                .Except(baseInterfaces)
                                                .Where(i => !i.IsGenericType && i != typeof(IChildComposante))
                                                .ToList().Find(type => typeof(IChildComposante).IsAssignableFrom(type));

                return childPresenter;
            }

            private void CreerChildPresenter(ChildComposante ctrl)
            {
                Type? interfaceChildComposante = ObtenirChildPresenter(ctrl);

                if (interfaceChildComposante is not null)
                {
                    BasePresenterEventArgs args = new BasePresenterEventArgs();
                    Type typeChildPresenter = childPresenters[interfaceChildComposante.Name];

                    OwnerCompo.ObtenirBasePresenter?.Invoke(this, args);

                    if (args.BasePresenter is not null)
                    {
                        IAddAndRemoveChildPresenter presenter = (IAddAndRemoveChildPresenter)args.BasePresenter;
                        IChildPresenter childPresenter = (IChildPresenter)Factory!.Create(typeChildPresenter);

                        childPresenter.InjectionComposante(ctrl, args.BasePresenter);
                        presenter.Ajout(childPresenter);
                        ((IBasePresenter)presenter).envoyerCorrespondance += childPresenter.RecevoirCorrespondance;

                        childPresenter.InitPresenter(this, null);
                    }
                }
            }
        }
    }
}
