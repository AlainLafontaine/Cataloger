using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.Interfaces
{
    public interface IDirtyPresenter
    {
        bool IsDirty { get; set; }
    }
}
