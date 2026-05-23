using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.Interfaces
{
    public interface IAddAndRemoveChildPresenter
    {
        void Ajout(IChildPresenter childPresenter);
        void Retire(IChildPresenter childPresenter);

    }
}
