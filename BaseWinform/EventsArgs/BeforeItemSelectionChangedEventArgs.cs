using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.EventsArgs
{
    public class BeforeItemSelectionChangedEventArgs
    {
        private int indexCourant;
        private int indexAvenir;
        private bool annulerSelection = false;

        public int IndexCourant { get => indexCourant; }

        public int IndexAvenir { get => indexAvenir; }

        public BeforeItemSelectionChangedEventArgs(int indexCourant, int indexAvenir)
        {
            this.indexCourant = indexCourant;
            this.indexAvenir = indexAvenir;
        }

        public bool AnnulerSelection { 
            get => annulerSelection; 
            set => annulerSelection = value;
        }
    }
}
