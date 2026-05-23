using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.Interfaces
{
    public interface IChildPresenterDataShared
    {
        T? Obtenir<T>(string id);
        void Deposer<T>(string id, T t);
    }
}
