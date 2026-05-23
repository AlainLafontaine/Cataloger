using System.Text.Json;

namespace BaseWinform.Utilitaires
{
    public static class UtilitaireCopie
    {
        public static List<T> DeepCopie<T>(List<T> source)
        {
            string json = JsonSerializer.Serialize(source);
            return JsonSerializer.Deserialize<List<T>>(json)!;
        }
    }
}
