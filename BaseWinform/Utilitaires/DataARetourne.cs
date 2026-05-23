namespace BaseWinform.Utilitaires
{
    public class DataARetourne : TransfertData
    {
        static public DataARetourne Set(int valeur) { return new DataARetourne(valeur); }
        static public DataARetourne Set(double valeur) { return new DataARetourne(valeur); }
        static public DataARetourne Set(string valeur) { return new DataARetourne(valeur); }
        static public DataARetourne Set<T>(T valeur)
        {
            DataARetourne ret = new DataARetourne();

            ret.data = valeur;
            return ret;
        } 

        private DataARetourne() { }
        private DataARetourne(int valeur) { IntValeur = valeur; }
        private DataARetourne(double valeur) { DoubleValeur = valeur; }
        private DataARetourne(string? valeur) { StringValeur = valeur; }
    }
}
