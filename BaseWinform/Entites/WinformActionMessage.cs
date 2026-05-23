using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zzz.App.Core;
using Zzz.App.Core.Entites;

namespace BaseWinform.Entites
{
    public class WinformActionMessage : ActionMessage
    {
        public WinformActionMessage() { }

        public WinformActionMessage(string id, string msg) : base(id, msg) { }

        public WinformActionMessage(string msg) : base(msg) { }
    }
}
