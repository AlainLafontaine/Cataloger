namespace BaseWinform.Interfaces
{
    public interface IRestaureService
    {
        public string MakeKey(Form form);

        public bool Contains(string key);

        public void Remove(string key);

        public void RestoreFormState(Form form, string key);

        public void SaveFormState(Form form, string key);

    }
}
