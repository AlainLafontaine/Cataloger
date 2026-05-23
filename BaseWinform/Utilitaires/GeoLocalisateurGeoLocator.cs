using BaseWinform.EventsArgs;
using BaseWinform.Interfaces;
using Windows.Devices.Geolocation;

namespace BaseWinform.Utilitaires
{
    public class GeoLocalisateurGeoLocator : IGeoLocalisateur
    {
        public bool EnModeSimulation { get => false; }

        public Action<GeoLocalisateurEventArgs>? TransmettreCoord { get; set; } = null;

        public SynchronizationContext? UIContext { get; set; }

        private Geolocator? geolocator;

        public GeoLocalisateurGeoLocator()
        {
        }

        public bool Connexion()
        {
            (bool actif, Geolocator? locator) emplacementInfo = Task.Run(() =>
                                EstEmplacementActifAsync())
                                    .GetAwaiter()
                                    .GetResult();

            if (geolocator != null) 
            {
                geolocator.PositionChanged -= Geolocator_PositionChanged;
                geolocator = null;
            }

            if (emplacementInfo.actif)
            {
                geolocator = emplacementInfo.locator!;
                geolocator.PositionChanged += Geolocator_PositionChanged;
            }

            return true;
        }

        public void Deconnexion()
        {
            if (geolocator != null)
            {
                geolocator.PositionChanged -= Geolocator_PositionChanged;
                geolocator = null;
            }
        }

        private void Geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (TransmettreCoord == null) return;

            var coord = args.Position.Coordinate.Point.Position;
            var accuracy = args.Position.Coordinate.Accuracy;
            var timestamp = args.Position.Coordinate.Timestamp;

            GeoLocalisateurEventArgs arg = new ()
            {
                Latitude = coord.Latitude,
                Longitude = coord.Longitude,
                Accuracy = args.Position.Coordinate.Accuracy,
                Timespan = timestamp
            };

            UIContext?.Post(_ => { TransmettreCoord(arg); }, null);
        }

        private async Task<(bool actif, Geolocator? locator)> EstEmplacementActifAsync()
        {
            var access = await Geolocator.RequestAccessAsync();
            if (access != GeolocationAccessStatus.Allowed)
                return (false, null);

            try
            {
                var locator = new Geolocator
                {
                    DesiredAccuracy = PositionAccuracy.High, 
                    ReportInterval = 1000,  // Mise à jour toutes les 1 secondes
                    MovementThreshold = 1   // Mise à jour si déplacement ≥ 1 mètre
                };

                var getPositionTask = locator.GetGeopositionAsync().AsTask();
                var completed = await Task.WhenAny(getPositionTask, Task.Delay(TimeSpan.FromSeconds(10)));

                if (completed != getPositionTask)
                {
                    locator = null;
                    return (false, null);
                }

                return (true, locator);
            }
            catch
            {
                return (false, null);
            }
        }
    }
}
