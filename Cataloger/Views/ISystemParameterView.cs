using Cataloger.Core.Entities.SystemsParameters.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cataloger.Views
{
    public interface ISystemParameterView : ICatalogerView
    {
        // ---------------------------------------------------
        // --- Liste à charger                             ---
        // ---------------------------------------------------
        
        void LoadSkinStyles(IEnumerable<SystemParameterDto> skinStyles, SystemParameterDto skinStyleActif);

        // ---------------------------------------------------
        // --- Propriétés                                  ---
        // ---------------------------------------------------
        
        SystemParameterDto? SkinStyleActif { get; set;  }

        // ---------------------------------------------------
        // --- Les évênements                              ---
        // ---------------------------------------------------

        event EventHandler? OnSkinStyleChanged;
    }
}
