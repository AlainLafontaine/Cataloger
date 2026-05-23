using BaseWinform.EventsArgs;

namespace BaseWinform.Interfaces
{
    public interface IGeoLocalisateur
    {
        bool EnModeSimulation { get; }

        Action<GeoLocalisateurEventArgs>? TransmettreCoord { get; set; }

        SynchronizationContext? UIContext { get; set; }

        bool Connexion();

        void Deconnexion();
    }
}
