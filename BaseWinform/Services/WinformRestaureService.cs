using BaseWinform.Interfaces;
using BaseWinform.Utilitaires;

namespace BaseWinform.Services
{
    public class WinformRestaureService : WinformService, IRestaureService
    {
        private WinformStateHelper helper = new WinformStateHelper();

        public WinformRestaureService() { }

        public string MakeKey(Form form) => helper.MakeKey(form);

        public bool Contains(string key) => helper.Contains(key);

        public void Remove(string key) => helper.Remove(key);

        public void RestoreFormState(Form form, string key) => helper.RestoreFormState(form, key);

        public void SaveFormState(Form form, string key) => helper.SaveFormState(form, key);
    }
}