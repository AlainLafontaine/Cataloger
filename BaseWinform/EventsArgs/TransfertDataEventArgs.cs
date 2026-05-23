using BaseWinform.Interfaces;
using BaseWinform.Utilitaires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.EventsArgs
{
    public class TransfertDataEventArgs : EventArgs
    {
        protected ITransfertData? transfertData = null;

        public void Set(int valeur) { transfertData = DataARetourne.Set(valeur); }
        public void Set(double valeur) { transfertData = DataARetourne.Set(valeur); }
        public void Set(string valeur) { transfertData = DataARetourne.Set(valeur); }
        public void Set<T>(T valeur) { transfertData = DataARetourne.Set<T>(valeur); }
    }
}
