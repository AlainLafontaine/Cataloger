using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.EventsArgs
{
    public class AutoriserPrecedentEventArgs : EventArgs
    {
        public bool EstPermis { get; set; } = true; // Par défaut accepté
    }
}
