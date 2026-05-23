using BaseWinform.Entites;
using BaseWinform.Interfaces;
using BaseWinform.Utilitaires;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using Zzz.App.Core;
using Zzz.App.Core.Actions;
using Zzz.App.Core.Actions.Http;
using Zzz.App.Core.IoC;
using Zzz.App.Core.Types;
using static BaseWinform.AccesAction.PresenterDirectAccessAction;

namespace BaseWinform.AccesAction
{
    /// <summary>
    /// Les type d'accès au Action de la librarie Affaire dans le backend
    /// </summary>
    public enum TypeRequete { get, post, put, delete }


    /// <summary>
    /// 
    /// </summary>
    public class PresenterDirectAccessAction : IWinformPresenter
    {
        public static Dictionary<string, List<RequeteToAction>> ActionURLToActionItem = new Dictionary<string, List<RequeteToAction>>();
        public static Action<WinformActionMessage>? transmettreWinformActionMessage = null;
        public static IFactory? factory = null;

        public PresenterDirectAccessAction() 
        { 
        }

        public static void Init(string assemblyName)
        {
            Action<string, TypeRequete, Type> typeRequeteLieItemAction = (url, typeRequete, item) => {
                if (!ActionURLToActionItem.TryGetValue(url.ToLower(), out List<RequeteToAction>? list))
                {
                    list = new List<RequeteToAction>();
                    ActionURLToActionItem.Add(url.ToLower(), list);
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

        public void AfficherMsg(WinformActionMessage msg)
        {
            transmettreWinformActionMessage?.Invoke(msg);
        }

        public void AfficherMsg(string msg, ConstantesNoyau.ActionMsgType msgType = ConstantesNoyau.ActionMsgType.success)
        {
            WinformActionMessage message = new WinformActionMessage(msg)
            {
                type = msgType
            };
            AfficherMsg(message);
        }

        protected void delete(
            string URL,
            Func<bool>? Succes = null,
            Func<Reponse, bool>? Echec = null,
            dynamic? body = null,
            bool afficheMsg = true
        )
        {
            ItemTypeLieParam? itemTypeLieParam = ObtenirItem(URL, TypeRequete.delete);

            if (itemTypeLieParam == null)
            {
                Reponse reponse = new();
                reponse.AddMsg("URL est invalide, pas de end-point");
            }
            else
            {
                dynamic action = factory!.Create(itemTypeLieParam.ItemAction);
                var requete = BuildRequete(action, itemTypeLieParam, body);
                var rep = action.Executer(requete);

                if (rep.EstSucces)
                {
                    if (Succes?.Invoke() ?? false || Echec == null && afficheMsg)
                    {
                        AfficherMsg("Supprimer avec succès");
                    }
                }
            }
        }

        protected void get<T>(
            string URL,
            out T? dto,
            Action<T?>? Succes = null, 
            Func<Reponse, bool>? Echec = null, 
            dynamic? body = null)
        {
            ItemTypeLieParam? itemTypeLieParam = ObtenirItem(URL, TypeRequete.get);

            dto = default;
            if (itemTypeLieParam == null)
            {
                Reponse reponse = new();
                reponse.AddMsg("URL est invalide, pas de end-point");
            }
            else
            {
                dynamic action = factory!.Create(itemTypeLieParam.ItemAction);
                var requete = BuildRequete(action, itemTypeLieParam, body);
                var rep = action.Executer(requete);

                if (rep!.EstSucces)
                {
                    dto = ObtenirDto<T>(rep);
                    Succes?.Invoke(dto);
                }
                else 
                { 
                
                }
            }
        }

        protected void post<T>(
            string URL,
            dynamic body,
            out T? dto,
            Func<T?, bool>? Succes = null,
            Func<Reponse, bool>? Echec = null,
            bool afficheMsg = true
        )
        {
            ItemTypeLieParam? itemTypeLieParam = ObtenirItem(URL, TypeRequete.post);

            dto = default;
            if (itemTypeLieParam == null)
            {
                Reponse reponse = new Reponse();
                reponse.AddMsg("URL est invalide, pas de end-point");

                if (Echec?.Invoke(reponse) ?? false || Echec == null && afficheMsg)
                {
                    WinformActionMessage msg = new WinformActionMessage("URL est invalide, pas de end-point")
                    {
                        type = ConstantesNoyau.ActionMsgType.success
                    };

                    AfficherMsg("URL est invalide, pas de end-point", ConstantesNoyau.ActionMsgType.danger);
                }
            }
            else
            {
                dynamic action = factory!.Create(itemTypeLieParam.ItemAction);
                var requete = BuildRequete(action, itemTypeLieParam, body);
                var rep = action.Executer(requete);

                if (rep.EstSucces)
                {
                    dto = ObtenirDto<T>(rep);
                    if (Succes?.Invoke(dto) ?? false || Echec == null && afficheMsg)
                    {
                        AfficherMsg("Enregistré avec succès");
                    }
                }
            }
        }

        protected void post(
            string URL,
            dynamic body,
            Func<bool>? Succes = null,
            Func<Reponse, bool>? Echec = null,
            bool afficheMsg = true
        )
        {
            ItemTypeLieParam? itemTypeLieParam = ObtenirItem(URL, TypeRequete.post);

            if (itemTypeLieParam == null)
            {
                Reponse reponse = new Reponse();
                reponse.AddMsg("URL est invalide, pas de end-point");

                if (Echec?.Invoke(reponse) ?? false || Echec == null && afficheMsg)
                {
                    WinformActionMessage msg = new WinformActionMessage("URL est invalide, pas de end-point")
                    {
                        type = ConstantesNoyau.ActionMsgType.success
                    };

                    AfficherMsg("URL est invalide, pas de end-point", ConstantesNoyau.ActionMsgType.danger);
                }
            }
            else
            {
                dynamic action = factory!.Create(itemTypeLieParam.ItemAction);
                var requete = BuildRequete(action, itemTypeLieParam, body);
                var rep = action.Executer(requete);

                if (rep.EstSucces)
                {
                    if (Succes?.Invoke() ?? false || Echec == null && afficheMsg)
                    {
                        AfficherMsg("Enregistré avec succès");
                    }
                }
            }
        }

        protected void put<T>(
            string URL, 
            dynamic body,
            out T? dto,
            Func<T?, bool>? Succes = null,
            Func<Reponse, bool>? Echec = null,
            bool afficheMsg = true
        )
        {
            ItemTypeLieParam? itemTypeLieParam = ObtenirItem(URL, TypeRequete.put);

            dto = default;
            if (itemTypeLieParam == null)
            {
                Reponse reponse = new Reponse();
                reponse.AddMsg("URL est invalide, pas de end-point");

                if (Echec?.Invoke(reponse) ?? false || Echec == null && afficheMsg)
                {
                    AfficherMsg("URL est invalide, pas de end-point", ConstantesNoyau.ActionMsgType.danger);
                }
            }
            else
            {
                dynamic action = factory!.Create(itemTypeLieParam.ItemAction);
                var requete = BuildRequete(action, itemTypeLieParam, body);
                var rep = action.Executer(requete);

                if (rep.EstSucces)
                {
                    dto = ObtenirDto<T>(rep);
                    if (Succes?.Invoke(dto) ?? false || Echec == null && afficheMsg)
                    {
                        AfficherMsg("Enregistré avec succès");
                    }
                }
            }
        }

        protected void put(
            string URL,
            dynamic? body,
            Func<bool>? Succes = null,
            Func<Reponse, bool>? Echec = null,
            bool afficheMsg = true
        )
        {
            ItemTypeLieParam? itemTypeLieParam = ObtenirItem(URL, TypeRequete.put);

            if (itemTypeLieParam == null)
            {
                Reponse reponse = new Reponse();
                reponse.AddMsg("URL est invalide, pas de end-point");

                if (Echec?.Invoke(reponse) ?? false || Echec == null && afficheMsg)
                {
                    AfficherMsg("URL est invalide, pas de end-point", ConstantesNoyau.ActionMsgType.danger);
                }
            }
            else
            {
                dynamic action = factory!.Create(itemTypeLieParam.ItemAction);
                var requete = BuildRequete(action, itemTypeLieParam, body);
                var rep = action.Executer(requete);

                if (rep.EstSucces)
                {
                    if (Succes?.Invoke() ?? false || Echec == null && afficheMsg)
                    {
                        AfficherMsg("Enregistré avec succès");
                    }
                }
            }
        }

        protected dynamic? ConstruireBodies(params Expression<Func<dynamic?>>[] expressions)
        {
            List<(string, dynamic)> retour = new List<(string, dynamic)>();

            foreach (var expression in expressions)
            {
                if (expression != null)
                {
                    var member = (MemberExpression)expression.Body;

                    if (member != null)
                    {
                        dynamic? valeur = expression.Compile().Invoke();
                        if (valeur == null) continue;

                        string nom = member.Member.Name;
                        retour.Add((nom, valeur));
                    }
                }
            }

            return retour.Count > 0 ? retour : null;
        }


        private ItemTypeLieParam? ObtenirItem(string URL, TypeRequete typeRequete)
        {
            // Recherche le contrôle a instancier selon url passé en paramètre
            string[] subURLs = URL.Split('/');
            string keyTrouvee = "";
            List<ParametrePresenterURL> parametres = new List<ParametrePresenterURL>();

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

                            parametres.Add(new ParametrePresenterURL(nomParametre, subURLs[index]));
                        }
                        else
                        {
							// La comparaison d'URL est case insensitive
                            if (subURLs[index].Equals(subKeys[index],StringComparison.InvariantCultureIgnoreCase))
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
                AfficherMsg(new WinformBadRequestActionMessage($"Aucune composante trouvée pour '{URL}'."));
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
                    if (prop != null)
                    {
                        prop.SetValue(requete, Convertir(parametrer.Valeur, prop.PropertyType.FullName ?? ""));
                    }
                    else
                    {
                        throw new NotImplementedException("Problème pour contruire l'objet requete");
                    }
                }
                else
                {
                    throw new NotImplementedException("Problème pour contruire l'objet requete");
                }
            }

            if (body != null) {
                if (body.GetType() == typeof(List<(string, dynamic)>))
                {
                    if (body is List<(string, dynamic)> list)
                    {
                        var propsAvecHttpBody = typeDeRequete
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                            .Where(p => p.IsDefined(typeof(HttpBodyAttribute), inherit: false))
                            .ToList();

                        foreach (var prop in propsAvecHttpBody)
                        {
                            foreach ((string, dynamic) obj in list)
                            {
                                string nom = obj.Item1;

                                if (prop.Name.ToLower() != obj.Item1.ToLower()) continue;
                                prop!.SetValue(requete, obj.Item2);  // <-- affecte la valeur
                                break;
                            }
                        }
                    }
                }
                else
                {
                    var propsAvecHttpBody = typeDeRequete
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .Where(p => p.IsDefined(typeof(HttpBodyAttribute), inherit: false))
                        .ToList();

                    foreach (var prop in propsAvecHttpBody)
                    {
                        prop!.SetValue(requete, body);  // <-- affecte la valeur
                    }
                }
            }

            return requete;
        }

        private T? ObtenirDto<T>(dynamic repAction)
        {
            Type typeDeRequete = repAction.GetType();
            FieldInfo[] fields = typeDeRequete.GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.FlattenHierarchy
            );

            if (fields.Length == 0) { return default; }
            if (fields.Length == 1)
            {
                FieldInfo field = fields[0];
                string name = Regex.Match(field.Name, @"(?<=<)[^>]+(?=>)").Groups[0].Value;

                var prop = typeDeRequete.GetProperty(
                    Regex.Match(field.Name, @"(?<=<)[^>]+(?=>)").Groups[0].Value,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                );

                return (T?)prop!.GetValue(repAction);
            }

            throw new NotImplementedException($"Plus de deux propriétés dans la réponse - avisez Architect logiciel");
        }

        public dynamic Convertir(string valeur, string type)
        {
            return type == "System.Int64" ? long.Parse(valeur) :
                   type == "System.Int32" ? int.Parse(valeur) : 
                   type == "System.Double" ? double.Parse(valeur) : valeur;
        }
    }
}
