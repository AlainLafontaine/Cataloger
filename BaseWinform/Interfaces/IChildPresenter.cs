using BaseWinform.Composantes;
using BaseWinform.EventsArgs;
using BaseWinform.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.Interfaces
{
    public interface IChildPresenter : IBasePresenter
    {
        IChildPresenterDataShared ChildPresenterDataShared { get; }
        void InjectionComposante(object composante, object parent);
        void RecevoirCorrespondance(object sender, CorrespondanceEventArgs e);
    }
}
