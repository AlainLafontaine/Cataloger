using BaseWinform.EventsArgs;
using BaseWinform.Interfaces;
using BaseWinform.Services;
using System.ComponentModel;

namespace BaseWinform.Controls
{
    public partial class BtnPrecedent : BtnSimpleBase, INavigationCtrl
    {
        public NavigationService? NavigationService { get; set; }

        [Category("Navigation")]
        [Description("Se déclenche pour demander si la navigation vers la page précédente est permise.")]
        public event EventHandler<AutoriserPrecedentEventArgs>? AutorisePrecedent;

        [Category("Data")]
        [Description("Se déclenche pour demander si on doit récupérer une valeur pour le desti.")]
        public event EventHandler<TransfertDataEventArgs>? TransfertData;

        public BtnPrecedent()
        {
            InitializeComponent();
            this.Click += BtnPrecedent_Click;
        }

        private void BtnPrecedent_Click(object? sender, EventArgs e)
        {
            // Déclenche l'événement pour demander au parent
            var args = new AutoriserPrecedentEventArgs();

            AutorisePrecedent?.Invoke(this, args);

            if (args.EstPermis)
            {
                var argsValeur = new ObternirDataARetourne();

                TransfertData?.Invoke(this, argsValeur);
                this.NavigationService!.Precedent(argsValeur.Obtenir());
            }
        }
    }

    public class ObternirDataARetourne : TransfertDataEventArgs
    {
        public ITransfertData? Obtenir() => transfertData; 
    }
}
