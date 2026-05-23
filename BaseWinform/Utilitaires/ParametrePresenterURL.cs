namespace BaseWinform.Utilitaires
{   
    /// -----------------------------------------------------------------------
    /// <summary>
    /// Contient un paramètre provenant de l'extraction de l'url d'un Presenter
    /// </summary>
    public class ParametrePresenterURL
    {
        /// <summary>
        /// Le nom du paramètre
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// La valeur du paramètre
        /// </summary>
        public string Valeur { get; set; }

        /// <summary>
        /// Constructeur du paramètre
        /// </summary>
        /// <param name="nom">
        /// Le nom du paramètre
        /// </param> 
        /// <param name="valeur">
        /// La valeur du paramètre
        /// </param> La va
        public ParametrePresenterURL(string nom, string valeur)
        {
            Nom = nom;
            Valeur = valeur;
        }
    }
}
