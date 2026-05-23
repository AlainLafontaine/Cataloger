using BaseWinform.Controls;
using BaseWinform.Entites;
using BaseWinform.Interfaces;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using Zzz.App.Core;
using Zzz.App.Core.Actions;
using Zzz.App.Core.Actions.Http;
using Zzz.App.Core.IoC;
using Zzz.App.Core.Types;

namespace Winform.Local.Access.Backend.Presenters
{
    public class WinformPresenter : IWinformPresenter
    {
        public enum TypeRequete { get, post, put, delete }

        public bool AfficherMessage { get; set; } = true;

        public class RequeteToAction 
        {
            public TypeRequete TypeRequete { get; set; }
            public Type ItemAction { get; set; }

            public RequeteToAction(TypeRequete typeRequete, Type itemAction) 
            {
                TypeRequete = typeRequete;
                ItemAction = itemAction;
            }
        }

        public class ItemTypeLieParam
        {
            public Type ItemAction { get; set; }
            public List<ParametreWinformURL> Parametres { get; set; }

            public ItemTypeLieParam(Type itemAction, List<ParametreWinformURL> parametres)
            {
                ItemAction = itemAction;
                Parametres = parametres;
            }
        }

        public static Dictionary<string, List<RequeteToAction>> ActionURLToActionItem = new Dictionary<string, List<RequeteToAction>>();
        public static Action<WinformActionMessage>? transmettreErreur = null;
        public static IFactory? factory = null;

        public WinformPresenter() 
        { 
        }

        public static void Init(string assemblyName)
        {
            Action<string, TypeRequete, Type> typeRequeteLieItemAction = (url, typeRequete, item) => {
                if (!ActionURLToActionItem.TryGetValue(url, out List<RequeteToAction>? list))
                {
                    list = new List<RequeteToAction>();
                    ActionURLToActionItem.Add(url, list);
                }

                list.Add(new RequeteToAction(typeRequete, item));
            };

            // Charge dans le contexte *Default* => visible du reste de l'app
            string assemblyPath = Path.Combine(AppContext.BaseDirectory, assemblyName);
            Assembly assemblyAffaire = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);

            // Construction du dictionnaire
            Type typeCtrl = typeof(Zzz.App.Core.Actions.Action);

            foreach (Type item in TypeGetter.GetClassTypesByBaseType(typeCtrl, assemblyAffaire))
            {
                if (item.Name != typeCtrl.Name)
                {
                    List<PostApiAttribute>? postAttributes = item.GetCustomAttributes<PostApiAttribute>(inherit: true).ToList();
                    List<GetApiAttribute>? getAttributes = item.GetCustomAttributes<GetApiAttribute>(inherit: true).ToList();
                    List<PutApiAttribute>? putAttributes = item.GetCustomAttributes<PutApiAttribute>(inherit: true).ToList();
                    List<DeleteApiAttribute>? deleteAttributes = item.GetCustomAttributes<DeleteApiAttribute>(inherit: true).ToList();

                    if (getAttributes != null) foreach (var getAttr in getAttributes) { typeRequeteLieItemAction(getAttr.Path, TypeRequete.get, item); }
                    if (postAttributes != null) foreach (var postAttr in postAttributes) { typeRequeteLieItemAction(postAttr.Path, TypeRequete.post, item); }
                    if (putAttributes != null) foreach (var putAttr in putAttributes) { typeRequeteLieItemAction(putAttr.Path, TypeRequete.put, item); }
                    if (deleteAttributes != null) foreach (var deleteAttr in deleteAttributes) { typeRequeteLieItemAction(deleteAttr.Path, TypeRequete.delete, item); }
                    else
                    {
                        // throw new NotImplementedException($"Attribut non valide pour {item.Name}");
                    }
                }
            }
        }

        public void Afficher(WinformActionMessage msg)
        {
            transmettreErreur?.Invoke(msg);
        }

        protected Rep? delete<Rep>(string URL, dynamic? body = null) where Rep : Reponse
        {
            ItemTypeLieParam? itemTypeLieParam = ObtenirItem(URL, TypeRequete.delete);
            if (itemTypeLieParam == null) return default;

            dynamic action = factory!.Create(itemTypeLieParam.ItemAction);
            var requete = BuildRequete(action, itemTypeLieParam, body);
            Rep? response = action.Executer(requete);

            if (response.EstSucces && AfficherMessage)
            {
                WinformActionMessage msg = new WinformActionMessage("Suppression avec succès")
                {
                    type = ConstantesNoyau.ActionMsgType.success
                };

                Afficher(msg);
            }

            return response;
        }

        protected Rep? get<Rep>(string URL, dynamic? body = null) where Rep : Reponse, new()
        {
            ItemTypeLieParam? itemTypeLieParam = ObtenirItem(URL, TypeRequete.get);
            
            if (itemTypeLieParam == null)
            {
                Rep reponse = new();

                reponse.AddMsg("URL est invalide, pas de end-point");
                return reponse;
            }

            dynamic action = factory!.Create(itemTypeLieParam.ItemAction);
            var requete = BuildRequete(action, itemTypeLieParam, body);
            Rep? response = action.Executer(requete);

            return response;
        }

        protected Rep? post<Rep>(string URL, dynamic body) where Rep : Reponse
        {
            ItemTypeLieParam? itemTypeLieParam = ObtenirItem(URL, TypeRequete.post);
            if (itemTypeLieParam == null) return default;

            dynamic action = factory!.Create(itemTypeLieParam.ItemAction);
            var requete = BuildRequete(action, itemTypeLieParam, body);
            Rep? response = action.Executer(requete);

            if (response.EstSucces && AfficherMessage)
            {
                WinformActionMessage msg = new WinformActionMessage("Enregistré avec succès")
                {
                    type = ConstantesNoyau.ActionMsgType.success
                };

                Afficher(msg);
            }

            return response;
        }

        protected Rep? put<Rep>(string URL, dynamic body) where Rep : Reponse
        {
            ItemTypeLieParam? itemTypeLieParam = ObtenirItem(URL, TypeRequete.put);
            if (itemTypeLieParam == null) return default;

            dynamic action = factory!.Create(itemTypeLieParam.ItemAction);
            var requete = BuildRequete(action, itemTypeLieParam, body);
            Rep? response = action.Executer(requete);

            if (response.EstSucces && AfficherMessage)
            {
                WinformActionMessage msg = new WinformActionMessage("Enregistré avec succès")
                {
                    type = ConstantesNoyau.ActionMsgType.success
                };

                Afficher(msg);
            }

            return response;
        }

        private ItemTypeLieParam? ObtenirItem(string URL, TypeRequete typeRequete)
        {
            // Recherche le contrôle a instancier selon url passé en paramètre
            string[] subURLs = URL.Split('/');
            string keyTrouvee = "";
            List<ParametreWinformURL> parametres = new List<ParametreWinformURL>();

            foreach (string key in ActionURLToActionItem!.Keys)
            {
                string[] subKeys = key.Split("/");

                if (subKeys.Length == subURLs.Length)
                {
                    for (int index = 0; index < subKeys.Length; index++)
                    {
                        if (subKeys[index][0] == '{')
                        {
                            keyTrouvee += index == 0 ? subKeys[index] : $"/{subKeys[index]}";

                            // Extraction du paramètres
                            string text = subKeys[index];
                            var match = Regex.Matches(text, @"\{([^}]*)\}")[0];
                            string nomParametre = match.Groups[1].Value;

                            parametres.Add(new ParametreWinformURL(nomParametre, subURLs[index]));
                        }
                        else
                        {
                            if (subURLs[index] == subKeys[index])
                            {
                                keyTrouvee += index == 0 ? subKeys[index] : $"/{subKeys[index]}";
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    if (keyTrouvee == key)
                    {
                        break;
                    }
                    else
                    {
                        keyTrouvee = "";
                        parametres.Clear();
                    }
                }
            }

            // Vérifie si la clé existe dans le dictionnaire
            if (!ActionURLToActionItem.TryGetValue(keyTrouvee, out List<RequeteToAction>? list))
            {
                Afficher(new WinformBadRequestActionMessage($"Aucune composante trouvée pour '{URL}'."));
                return null;
            }

            Type? type = list.FirstOrDefault(x => x.TypeRequete == typeRequete)?.ItemAction;

            return type != null ? new ItemTypeLieParam(type, parametres) : null;
        }

        private dynamic BuildRequete(dynamic action, ItemTypeLieParam itemTypeLieParam, dynamic? body = null)
        {
            var requete = action.CreerRequete();
            Type typeDeRequete = requete.GetType();
            FieldInfo[] fields = typeDeRequete.GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.FlattenHierarchy
            );

            foreach (var parametrer in itemTypeLieParam.Parametres)
            {
                FieldInfo? fieldInfo = fields.FirstOrDefault(x =>
                    Regex.Match(x.Name, @"(?<=<)[^>]+(?=>)").Groups[0].Value.ToLower().CompareTo(parametrer.Nom.ToLower()) == 0
                );

                if (fieldInfo != null)
                {
                    var prop = typeDeRequete.GetProperty(
                        Regex.Match(fieldInfo.Name, @"(?<=<)[^>]+(?=>)").Groups[0].Value,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                    );

                    // Affecte la valeur du paramètre
                    prop!.SetValue(requete, Convertir(parametrer.Valeur, prop.PropertyType.FullName));  
                }
                else
                {
                    throw new NotImplementedException($"Problème pour contruire l'objet requete");
                }
            }

            if (body != null) {

                var propsAvecHttpBody = typeDeRequete
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(p => p.IsDefined(typeof(HttpBodyAttribute), inherit: false))
                    .ToList();

                foreach (var prop in propsAvecHttpBody) 
                {
                    prop!.SetValue(requete, body);  // <-- affecte la valeur
                }
            }

            return requete;
        }

        public dynamic Convertir(string valeur, string type)
        {
            return type == "System.Int64" ? long.Parse(valeur) :
                   type == "System.Int32" ? int.Parse(valeur) : valeur;
        }
    }
}
