using BaseWinform.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.Interfaces
{
    public interface IBaseComposante
    {
        void AcceptChanges();
        void RemiseAZeroIsDirty();

        event EventHandler? InitComposante2;
    }
}
