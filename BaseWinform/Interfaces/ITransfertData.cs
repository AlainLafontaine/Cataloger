namespace BaseWinform.Interfaces
{
    public interface ITransfertData
    {
        int? IntValeur { get; }
        double? DoubleValeur { get; }
        string? StringValeur { get; }
        T? ObjectValeur<T>();
    }
}
