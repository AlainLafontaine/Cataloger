using BaseWinform.AccesAction;
using BaseWinform.Composantes;
using BaseWinform.Entites;
using BaseWinform.Forms;
using BaseWinform.Services;
using Cataloger.Composantes;
using Cataloger.Presenters;
using DevExpress.Utils;
using Zzz.App.Core.Configuration;
using static Zzz.App.Core.ConstantesNoyau;

namespace Cataloger
{
    public partial class MainForm : BaseForm
    {

        private CatalogerComposante? activeComposante = null;
        private bool suspendMajEtatControl = false;

        private readonly IConfigurationApp? config = null; 
        private readonly SystemParameterService? systemParametersPresenter = null;

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(
            NavigationService navigationService,
            IConfigurationApp config,
            SystemParameterService systemParametersPresenter,
            IsDesignModeService isDesignModeService
        ) : base(navigationService, isDesignModeService)
        {
            this.systemParametersPresenter = systemParametersPresenter;
            this.config = config;

            InitializeComponent();

            PresenterDirectAccessAction.transmettreWinformActionMessage += DisplayWinformActionMessage;
            NavigationService.transmettreWinformActionMessage += DisplayWinformActionMessage;

        }

        protected override void NouvelleComposante(BaseComposante composante)
        {
            suspendMajEtatControl = true;            
            zoneTravail.Controls.Clear();
            zoneTravail.Controls.Add((CatalogerComposante)composante);
            suspendMajEtatControl = false;
        }

        protected override void RestoreComposante(BaseComposante composante)
        {
            suspendMajEtatControl = true;
            zoneTravail.Controls.Clear();
            zoneTravail.Controls.Add((CatalogerComposante)composante);
            suspendMajEtatControl = false;
        }

        protected override void MajEtatControl()
        {
            if (!IsDesignMode() && !suspendMajEtatControl)
            {
                activeComposante?.MajEtatControl();
                base.MajEtatControl();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void DisplayWinformActionMessage(WinformActionMessage msg)
        {
            ShowNotification(msg);
        }
    }
}
