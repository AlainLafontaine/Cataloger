using System.Collections.Concurrent;

namespace BaseWinform.Utilitaires
{

    /// <summary>
    /// Magasin en mémoire. Persiste tant que le processus vit.
    /// </summary>

    public class WinformStateStore
    {

        private readonly ConcurrentDictionary<string, WinformState> _store = new();

        public void Save(string key, WinformState state) => _store[key] = state;

        public bool TryGet(string key, out WinformState? state) => _store.TryGetValue(key, out state);

        public void Remove(string key) => _store.TryRemove(key, out _);

        public bool Contains(string key) => _store.ContainsKey(key);
    }
}
