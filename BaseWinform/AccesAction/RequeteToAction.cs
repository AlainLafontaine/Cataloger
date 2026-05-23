using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BaseWinform.AccesAction.PresenterDirectAccessAction;

namespace BaseWinform.AccesAction
{
    /// <summary>
    /// 
    /// </summary>
    public class RequeteToAction
    {
        public TypeRequete TypeRequete { get; set; }
        public Type ItemAction { get; set; }

        public RequeteToAction(TypeRequete typeRequete, Type itemAction)
        {
            TypeRequete = typeRequete;
            ItemAction = itemAction;
        }
    }
}
