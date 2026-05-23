using BaseWinform.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.EventsArgs
{
    public class BasePresenterEventArgs : EventArgs
    {
         public IBasePresenter? BasePresenter {  get; set; }
    }
}
