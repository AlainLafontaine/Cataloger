using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zzz.App.Core;

namespace BaseWinform.Entites
{
    public class WinformBadRequestActionMessage : WinformActionMessage
    {
        public WinformBadRequestActionMessage() { type = ConstantesNoyau.ActionMsgType.danger; }

        public WinformBadRequestActionMessage(string id, string msg) : base(id, msg) { type = ConstantesNoyau.ActionMsgType.danger; }

        public WinformBadRequestActionMessage(string msg) : base(msg) { type = ConstantesNoyau.ActionMsgType.danger; }
    }
}
