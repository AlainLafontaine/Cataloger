using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zzz.App.Core;
using Zzz.App.Core.Entites;

namespace BaseWinform.Entites
{
    public class WinformWrapActionMessage : WinformActionMessage
    {
        public WinformWrapActionMessage(ActionMessage message) {
            id = message.id;
            msg = message.msg;
            type = message.type;
        }
    }
}
