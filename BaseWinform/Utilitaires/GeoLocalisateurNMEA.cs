
using System.IO.Ports;
using System.Globalization;

using BaseWinform.EventsArgs;
using BaseWinform.Interfaces;
using Zzz.App.Core.Extensions;

namespace BaseWinform.Utilitaires
{
    public class GeoLocalisateurNMEA : IGeoLocalisateur
    {
        public bool EnModeSimulation { get => false; }

        public Action<GeoLocalisateurEventArgs>? TransmettreCoord { get; set; } = null;

        public SynchronizationContext? UIContext { get; set; }

        private SerialPort? gpsPort;
        private string[] portNames;

        public GeoLocalisateurNMEA()
        {
            portNames = SerialPort.GetPortNames();
        }

        public bool Connexion()
        {
            try
            {
                gpsPort = new SerialPort(portNames[0], 4800);
                gpsPort.DataReceived += gpsPort_DataReceived;
                gpsPort.Open();
            }
            catch { return false; }

            return true;
        }

        public void Deconnexion()
        {
            if (gpsPort != null && gpsPort.IsOpen)
            {
                gpsPort.Close();
            }
        }

        private void gpsPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string line = gpsPort?.ReadLine() ?? "";
/*
            BeginInvoke(new Action(() =>
            {
                txtNmea.AppendText(line + Environment.NewLine);
                ParseNmea(line);
            }));
*/
        }

        private void ParseNmea(string nmea)
        {
            if (nmea.StartsWith("$GPGGA"))
                ParseGga(nmea);

            if (nmea.StartsWith("$GPRMC"))
                ParseRmc(nmea);
        }

        private void ParseGga(string nmea)
        {
            var parts = nmea.Split(',');

            if (parts.Length < 6)
                return;

            string lat = parts[2];
            string latDir = parts[3];
            string lon = parts[4];
            string lonDir = parts[5];

            if (!string.IsNullOrEmpty(lat) && !string.IsNullOrEmpty(lon))
            {
                //                double latitude = ConvertToDecimal(lat, latDir);
                //                double longitude = ConvertToDecimal(lon, lonDir);

                //                lblLatitude.Text = $"Latitude: {latitude:F6}";
                //                lblLongitude.Text = $"Longitude: {longitude:F6}";
            }
        }

        private void ParseRmc(string nmea)
        {
            var parts = nmea.Split(',');

            // Sécurité
            if (parts.Length < 10)
                return;

            // Statut A = valide, V = invalide
            if (parts[2] != "A")
                return;

            string lat = parts[3];
            string latDir = parts[4];
            string lon = parts[5];
            string lonDir = parts[6];

            if (string.IsNullOrEmpty(lat) || string.IsNullOrEmpty(lon))
                return;

            //            double latitude = ConvertToDecimal(lat, latDir);
            //            double longitude = ConvertToDecimal(lon, lonDir);

            //            lblLatitude.Text = $"Latitude: {latitude:F6}";
            //            lblLongitude.Text = $"Longitude: {longitude:F6}";

            // Optionnel : vitesse
            if (double.TryParse(parts[7], System.Globalization.CultureInfo.InvariantCulture, out double speedKnots))
            {
                double speedKmh = speedKnots * 1.852;
                // Exemple :
                // lblSpeed.Text = $"Vitesse: {speedKmh:F1} km/h";
            }
        }


        // --------------------------------------------------------------------------------------


        private class tCoordGPS
        {
            public string latitude = string.Empty;
            public string longitude = string.Empty;
            public string nordSud = string.Empty;
            public string estOuest = string.Empty;
        }

        private class tCoordGeo
        {
            public double latitude;
            public double longitude;
        }



        private void EnvoieCoordonneeGPS()
        {
            if (TransmettreCoord != null)
            {
                try
                {
                    TransmettreCoord(ObtenirGeoLocalisateur());
                }
                catch { 
                
                }
            }
        }

        private GeoLocalisateurEventArgs ObtenirGeoLocalisateur()
        {
            // Acquerir la trame NMEA du port de communication
            string trameNMEA = AcquerirTrameNMEA();

            tCoordGPS coordGPS =  DecoderTrameNMEA_GPRMC(trameNMEA);
            tCoordGeo coorGeo = gpsToGeo(coordGPS);
            double precission = DecoderTrameNMEA_GPGSA(trameNMEA);

            return new GeoLocalisateurEventArgs
            {
                Latitude = coorGeo.latitude,
                Longitude = coorGeo.longitude,
                Accuracy = precission,
                Timespan = new DateTimeOffset(),
            };
        }


        private bool OuvrirPortGPS()
        {
            // On fait une première tentative avec le port 16.
            // On présume que l'utilisateur a activée XPort
            return OuvrirPortGPSSpecifque(16, "4800,n,8,1") ? true :
                   OuvrirPortGPSSpecifque(3, "9600,n,8,1") ? true :
                   OuvrirPortGPSSpecifque(6, "9600,n,8,1") ? true :
                   OuvrirPortGPSSpecifque(7, "9600,n,8,1") ? true :
                   OuvrirPortGPSSpecifque(9, "9600,n,8,1") ? true : false; 
        }

        private bool OuvrirPortGPSSpecifque(int port, string setting)
        {
            bool success = true;
            string donneesGPS = string.Empty;

            //Thread.Sleep(2000);
            donneesGPS = AcquerirTrameNMEA();

            if (donneesGPS.IsNullOrEmpty())
            {
                success = false;
            }

            return success;
        }

        private void FermerPortGPS()
        {

        }

        private string AcquerirTrameNMEA()
        {
            string trameNMEA = "à venir";

            return trameNMEA;
        }

        private tCoordGPS DecoderTrameNMEA_GPRMC(string trameNMEA)
        {


            return new tCoordGPS()
            {
                latitude = "46,25454",
                longitude = "-73,254",
                nordSud = "S",
                estOuest = "W"
            };
        }

        private double DecoderTrameNMEA_GPGSA(string trameNMEA)
        {


            return 3;
        }

        private tCoordGeo gpsToGeo(tCoordGPS coordGPS)
        {
//            int degLat;
//            int degLong;
            tCoordGeo coordGeo = new();

            coordGeo.latitude = Double.Parse(coordGPS.latitude);
            coordGeo.longitude = Double.Parse(coordGPS.longitude);

            /*
            degLat = (int)(Double.Parse(coordGPS.latitude) / 100);
            degLong = (int)(Double.Parse(coordGPS.longitude) / 100);

            coordGeo.latitude = degLat + ((Double.Parse(coordGPS.latitude) - (degLat * 100)) / 60);
            coordGeo.longitude = degLong + ((Double.Parse(coordGPS.longitude) - (degLong * 100)) / 60);

            if (coordGPS.nordSud == "S") coordGeo.latitude *= -1;
            if (coordGPS.estOuest == "W") coordGeo.longitude *= -1;
            */
            return coordGeo;
        }
    }
}
