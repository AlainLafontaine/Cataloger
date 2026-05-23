using BaseWinform.Services;
using BaseWinform.Utilitaires;
using DevExpress.XtraEditors;
using Zzz.App.Core.IoC;

namespace BaseWinform.Controls
{
    public partial class BaseCtrl : XtraUserControl
    {
        static public IFactory? Factory { get; set; } = null;

        protected CustomBorder customBorder;

        public BaseCtrl()
        {
            InitializeComponent();
            customBorder = new CustomBorder(this);
        }

        public List<T> GetAllCtrlOfType<T>(Control parent)
        {
            var result = new List<T>();

            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is T btn) result.Add(btn);

                // Parcours récursif si le contrôle contient d'autres contrôles
                if (ctrl.HasChildren) result.AddRange(GetAllCtrlOfType<T>(ctrl));
            }

            return result;
        }

        public T? ObtenirParent<T>() where T : class
        {
            if (Parent != null)
            {
                Control ctrl = Parent;

                while (ctrl is not T)
                {
                    ctrl = ctrl.Parent!;
                }

                return ctrl as T;
            }

            return default(T);
        }
        
        public bool IsDesignMode()
        {
            return ObtenirService<IsDesignModeService>() == null ? true : false; 
        }

        protected T? ObtenirService<T>()
        {
            return (T?)Factory?.Create(typeof(T));
        }

        protected static IWin32Window GetSafeOwner(Control c)
        {
            // Priorité : le Form parent réel
            var form = c?.FindForm();
            if (form != null) return form;

            // Sinon, le top-level control s’il est un Form
            if (c?.TopLevelControl is Form topForm) return topForm;

            // Sinon, le top-level control (contrôle possédant un handle)
            if (c?.TopLevelControl != null) return c.TopLevelControl;

            // Comme filet de sécurité : un Form actif ou le premier ouvert
            return (IWin32Window)(Form.ActiveForm ?? Application.OpenForms.Cast<Form>().FirstOrDefault())!;
        }

        protected void ThrowSiDesignMode()
        {
            if (IsDesignMode()) throw new NotSupportedException("Exécution en mode Design non supportée.");
        }
    }

    /* Méthode suggére par copilote pour détecter si on est en mode design

        ------------------------------------
        Première méthode
        protected bool IsDesignMode =>
            DesignMode
            || (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            || (Site?.DesignMode == true);

        ------------------------------------
        Deuxième méthode
        /// <summary>
        /// Détection design-time robuste (VS Designer).
        /// </summary>
        protected static bool IsDesignTimeSafe(Control c)
        {
            // Cas standard Designer
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return true;

            // Quand Site est établi par le designer
            if (c?.Site?.DesignMode == true)
                return true;

            // Filet de secours : process Visual Studio
            try
            {
                var proc = Process.GetCurrentProcess().ProcessName;
                if (string.Equals(proc, "devenv", StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            catch
            {
                // Ne rien propager au Designer
            }

            return false;
        }

        ------------------------------------
        Troisème méthode
        protected bool IsDesignTimeNow()
        {
            // Dès qu'on a un parent/ISite, ça devient fiable
            if (this.DesignMode || this.IsAncestorSiteInDesignMode)
                return true;

            // Ajout « opportuniste » : utile en .NET Framework, parfois faux en .NET 6/7/8
            if (System.ComponentModel.LicenseManager.UsageMode ==
                System.ComponentModel.LicenseUsageMode.Designtime)
                return true;

            return false;
        }

        Information utile de copilote afin de définir une mèthode de notre cru qui va fonctionner

        c’est bien que ton code “runtime” s’exécute dans le Designer (donc ta détection renvoie faux alors que tu es en design).
        Le problème ne vient pas de toi : avec le Designer WinForms “out‑of‑process” (.NET 6/7/8), plusieurs heuristiques historiques 
        sont devenues non fiables dans le constructeur :

        LicenseManager.UsageMode peut valoir Runtime dans le Designer .NET (bug suivi) — donc test trompeur. [github.com]
        Le process n’est plus uniquement devenv.exe, il y a DesignToolsServer.exe (Designer hors‑proc) → le test du nom de process est à proscrire. [learn.microsoft.com], [learn.microsoft.com]
        Control.DesignMode n’est pas fiable dans le constructeur (Site pas encore établi) et reste souvent false pour les contrôles imbriqués. [learn.microsoft.com], [david-gouveia.com]
    */
}
