namespace BaseWinform.Utilitaires
{

    /// <summary>
    /// Contient l'état d'un formulaire : valeurs des contrôles et layouts DevExpress sérialisés (Base64).
    /// </summary>

    public sealed class WinformState
    {
        // clé = ctrl.Name
        public Dictionary<string, object> Controls { get; } = new();

        // clé = ctrl.Name, valeur = Base64 du layout
        public Dictionary<string, string> DevExpressLayouts { get; } = new();  
    }
}
