using BaseWinform.EventsArgs;
using BaseWinform.Interfaces;
using BaseWinform.Services;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace BaseWinform.Controls
{
    public partial class BtnNavigation : BtnSimpleBase, INavigationCtrl
    {
        public NavigationService? NavigationService { get; set; }

        [Category("Navigation")]
        [Description("Demande au parent l'url pour la navigation.")]
        public event EventHandler<WinformURLEventArgs>? ObtenirWinformURL;

        [Category("Data")]
        [Description("Se déclenche pour demander si on doit récupérer une valeur pour le destinataire.")]
        public event EventHandler<TransfertDataEventArgs>? transfertData;

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Navigation")]
        [Description("URL")]
        [AllowNull]
        public string URL { get; set; }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Navigation")]
        [Description("PermetPrecedent")]
        public bool PermetPrecedent { get; set; }

        public BtnNavigation()
            : base()
        {
            InitializeComponent();
            this.Click += BtnNavigation_Click;
        }

        private void BtnNavigation_Click(object? sender, EventArgs e) 
        {
            var args1 = new WinformURLEventArgs();
            var args2 = new ObternirDataAEnvoye();

            ObtenirWinformURL?.Invoke(this, args1);
            transfertData?.Invoke(this, args2);

            NavigationService?.Naviguer(
                (args1.WinformURL == string.Empty) ? URL : args1.WinformURL, 
                args2.Obtenir(),
                PermetPrecedent
            );
        }
    }

    public class ObternirDataAEnvoye : TransfertDataEventArgs
    {
        public ITransfertData? Obtenir() => transfertData;
    }
}
