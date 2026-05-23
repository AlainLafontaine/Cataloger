using BaseWinform.EventsArgs;
using BaseWinform.Utilitaires;
using DevExpress.CodeParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.Interfaces
{
    public delegate void EnvoyerCorrespondanceHandler(object sender, CorrespondanceEventArgs e);

    public interface IBasePresenter
    {
        bool Initialise { get; }

        void InitPresenter(object? sender, EventArgs? e);
        
        void ReleasePresenter();

        void RemiseAZeroIsDirty();

        void MajEtatControl();

        event EnvoyerCorrespondanceHandler? envoyerCorrespondance;
    }

    public interface IBasePresenter<I> : IBasePresenter where I : IBaseComposante
    {
        /// <summary>
        /// Référence sur la composante - le user control pour le UI
        /// </summary>
        I? Composante { get; }
    }
}
