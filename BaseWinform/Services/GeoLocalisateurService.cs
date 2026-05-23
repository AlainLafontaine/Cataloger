using BaseWinform.EventsArgs;
using BaseWinform.Interfaces;

namespace BaseWinform.Services
{
    public class GeoLocalisateurService : WinformService
    {
        public bool EnModeSimulation { get => geoLocalisateur?.EnModeSimulation ?? false; }

        public bool EstEnLigne { get => estEnLigne; }
        
        public IGeoLocalisateur? GeoLocalisateur
        {
            private get => geoLocalisateur;
            
            set {
                if (geoLocalisateur != null)
                {
                    if (estEnLigne)
                    {
                        geoLocalisateur.Deconnexion();
                    }

                    geoLocalisateur.UIContext = null;
                    geoLocalisateur.TransmettreCoord = null;
                }

                geoLocalisateur = value;
                if (geoLocalisateur != null)
                {
                    geoLocalisateur.UIContext = this.UIContext;
                    geoLocalisateur.TransmettreCoord = TransmettreLocalisation;

                    if (estEnLigne)
                    {
                        estEnLigne = geoLocalisateur.Connexion();
                    }
                }
            }
        }

        public SynchronizationContext? UIContext { 
            get => uiContext; 
            set {
                uiContext = value;
                if (geoLocalisateur != null)
                {
                    geoLocalisateur.UIContext = this.UIContext;
                }
            } 
        }

        private readonly Dictionary<Guid, Action<GeoLocalisateurEventArgs>> transmetteurs = new();
        private bool estEnLigne = false;
        private IGeoLocalisateur? geoLocalisateur = null;
        private SynchronizationContext? uiContext = null;
        private GeoLocalisateurEventArgs? dernierReception = null;

        public GeoLocalisateurService() 
        {
        }

        public Guid? Abonnement(Action<GeoLocalisateurEventArgs> transmettreCoord)
        {
            Guid guid = Guid.NewGuid();

            transmetteurs[guid] = transmettreCoord;

            if (dernierReception != null)
            {
                TransmettreLocalisation(dernierReception);
            }

            return guid;
        }

        public bool Desabonnement(Guid abonnementGPS)
        {
            return transmetteurs.Remove(abonnementGPS);
        }

        public bool Connexion()
        {
            estEnLigne = geoLocalisateur?.Connexion() ?? false;
            return estEnLigne;
        }

        public void Deconnexion()
        {
            geoLocalisateur?.Deconnexion();
            estEnLigne = false;
        }

        private void TransmettreLocalisation(GeoLocalisateurEventArgs args)
        {
            dernierReception = args;
            foreach (Action<GeoLocalisateurEventArgs> transmettre in transmetteurs.Values)
            {
                transmettre(args);
            }
        }
    }
}
