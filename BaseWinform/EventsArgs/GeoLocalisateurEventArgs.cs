using DevExpress.XtraReports.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.EventsArgs
{
    public class GeoLocalisateurEventArgs : EventArgs
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Accuracy { get; set; }
        public DateTimeOffset Timespan { get; set; }
    }
}
