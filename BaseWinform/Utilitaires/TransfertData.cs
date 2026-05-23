using BaseWinform.Interfaces;

namespace BaseWinform.Utilitaires
{
    public abstract class TransfertData : ITransfertData
    {
        protected object? data = null;

        public int? IntValeur { get; protected set; } = null;
        public double? DoubleValeur { get; protected set; } = null;
        public string? StringValeur { get; protected set; } = null;
        public T? ObjectValeur<T>() { return (data != null) ? (T)data : default(T); }
    }
}
