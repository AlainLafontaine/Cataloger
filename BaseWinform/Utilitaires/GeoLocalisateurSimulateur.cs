using BaseWinform.EventsArgs;
using BaseWinform.Interfaces;
using DevExpress.Map.Kml.Model;

namespace BaseWinform.Utilitaires
{
    public class GeoLocalisateurSimulateur : IGeoLocalisateur
    {
        public bool EnModeSimulation { get => true; }

        public Action<GeoLocalisateurEventArgs>? TransmettreCoord { get; set; } = null;
        public SynchronizationContext? UIContext { get; set; }

        private readonly System.Windows.Forms.Timer majGPS;

        private List<LatLonPoint>? simulateurLatLon = null;
        private int simulateurIndex = 0;

        public GeoLocalisateurSimulateur() 
        {
            majGPS = new() { Interval = 1_000 };
            majGPS.Tick += (s, e) => EnvoieCoordonneeGPS();
            InitSimulateur();
        }

        public bool Connexion()
        {
            majGPS.Start();
            return true;
        }

        public void Deconnexion()
        {
            majGPS.Stop();
        }

        private void EnvoieCoordonneeGPS()
        {
            if (TransmettreCoord != null)
            {
                if (simulateurIndex >= simulateurLatLon?.Count) simulateurIndex = 0;

                LatLonPoint pt = simulateurLatLon![simulateurIndex];
                GeoLocalisateurEventArgs arg = new GeoLocalisateurEventArgs
                {
                    Latitude = pt.Latitude,
                    Longitude = pt.Longitude, 
                    Accuracy = -1,
                    Timespan = new DateTimeOffset(),
                };

                TransmettreCoord(arg);
                simulateurIndex++;
            }
        }

        private void InitSimulateur()
        {
            if (simulateurLatLon == null)
            {
                simulateurLatLon = new List<LatLonPoint>();
                simulateurIndex = 0;
            }

            simulateurLatLon.Add(new LatLonPoint(45.2547700, -71.4521400));
            simulateurLatLon.Add(new LatLonPoint(45.2647800, -71.4621500));
            simulateurLatLon.Add(new LatLonPoint(45.2747900, -71.4721600));
            simulateurLatLon.Add(new LatLonPoint(45.2848000, -71.4821700));
            simulateurLatLon.Add(new LatLonPoint(45.2948100, -71.4921800));
            simulateurLatLon.Add(new LatLonPoint(45.3048200, -71.5021900));
            simulateurLatLon.Add(new LatLonPoint(45.3148300, -71.5122000));
            simulateurLatLon.Add(new LatLonPoint(45.3248400, -71.5222100));
        }
    }
}
