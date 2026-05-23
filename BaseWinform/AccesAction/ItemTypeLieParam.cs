using BaseWinform.Utilitaires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.AccesAction
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemTypeLieParam
    {
        public Type ItemAction { get; set; }
        public List<ParametrePresenterURL> Parametres { get; set; }

        public ItemTypeLieParam(Type itemAction, List<ParametrePresenterURL> parametres)
        {
            ItemAction = itemAction;
            Parametres = parametres;
        }
    }
}
