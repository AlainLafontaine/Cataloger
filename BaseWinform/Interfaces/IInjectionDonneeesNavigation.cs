using BaseWinform.Utilitaires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseWinform.Interfaces
{
    public interface IInjectionDonneeesNavigation
    {
        /// <summary>
        /// Injection des données de navigation dans le Presenter
        /// </summary>
        /// <param name="parametres">
        /// Liste des paramètres extrait de l'url associé au Presenter
        /// </param>
        /// <param name="transfertData">
        /// Référence sur les données qui doit-être transférées au Presenter
        /// </param>
        void InjectionDonneesNavigation(List<ParametrePresenterURL> parametres, ITransfertData? transfertData);
    }
}
