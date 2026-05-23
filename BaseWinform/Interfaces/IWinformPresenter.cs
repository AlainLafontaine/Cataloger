using BaseWinform.Entites;
using Zzz.App.Core;

namespace BaseWinform.Interfaces
{
    public interface IWinformPresenter
    {
        void AfficherMsg(WinformActionMessage msg);
        void AfficherMsg(string msg, ConstantesNoyau.ActionMsgType msgType = ConstantesNoyau.ActionMsgType.success);
    }
}
