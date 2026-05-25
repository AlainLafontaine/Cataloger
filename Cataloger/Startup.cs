using BaseWinform.Composantes;
using BaseWinform.Forms;
using BaseWinform.Interfaces;
using BaseWinform.Services;
using Cataloger.Business.ParametresSystemes;
using Cataloger.Composantes;
using Cataloger.DataAccess;
using Cataloger.DataAccess.Sqlite.Repositories;
using Cataloger.Presenters.Bases;
using Cataloger.Views;
using Microsoft.Extensions.DependencyInjection;
using Zzz.App.Core.Configuration;
using Zzz.App.Core.Donnees;
using Zzz.App.Core.Extensions;
using Zzz.App.Core.Types;
using Zzz.App.Service.Core;
using Zzz.App.Service.Core.IoC;

namespace Cataloger
{
    // if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();


    public class Startup : ZzzServiceStartup
    {
        protected override void ConfigureServicesApp(IServiceCollection services)
        {
            services.AddSingleton<NavigationService>();
            services.AddSingleton<DevExpressRestaureService>();
            services.AddTransient<IsDesignModeService>();

            this.AddCatalogerComposates(services);
            this.AddWinformService(services);
            this.AddPresenter(services);

            //Mapping du type connexion
            services.AddSingleton<AppConnexion>();

            //Il est important d'ajouter la fonction pour s'assurer de retourner la meme instance que
            //ce soit pour la creation de IConnexion ou IConnexionTransaction
            services.AddSingleton<IConnexion, AppConnexion>((IServiceProvider sp) =>
            {
                return sp.GetRequiredService<AppConnexion>();
            });
            services.AddSingleton<IConnexionTransaction, AppConnexion>((IServiceProvider sp) =>
            {
                return sp.GetRequiredService<AppConnexion>();
            });

            services.AddWithConfigurationServicesProvider(new Zzz.App.Core.AccesDonnees.Sqlite.Sql.ConfigurationServicesProvider());

            // On map toutes les action de la dll Affaire
            services.AddActions(typeof(GetSystemParameterAction).Assembly);

            // On map toutes les actions de la dll Zzz.App.Core     

            //On map les repositories
            services.AddRepositories(typeof(SystemParameterRespository).Assembly);
        }

        private void AddCatalogerComposates(IServiceCollection serviceCollection)
        {
            Type typeImaComposante = typeof(CatalogerComposante);

            foreach (Type item in TypeGetter.GetClassTypesByBaseType(typeImaComposante, this.GetType().Assembly))
            {
                var baseInterfaces = item.BaseType?.GetInterfaces() ?? Array.Empty<Type>();
                Type[] presenterInterface = item.GetInterfaces()
                        .Except(baseInterfaces)
                        .Where(i => !i.IsGenericType && i != typeof(ICatalogerView))
                        .ToArray();

                if (presenterInterface.Length == 1)
                    serviceCollection.AddTransient(presenterInterface[0], item);
                else
                    serviceCollection.AddTransient(item);
            }
        }

        private void AddWinformService(IServiceCollection serviceCollection)
        {
            foreach (Type item in TypeGetter.GetClassTypesByBaseType(typeof(IWinformPresenter), this.GetType().Assembly))
            {
                serviceCollection.AddTransient(item);
            }
        }

        private void AddPresenter(IServiceCollection serviceCollection)
        {
            // Ajoute dans le DI de Microsoft pour les types dérivés de IImaPresenter
            Type typePresenter = typeof(ICatalogerPresenter);

            foreach (Type item in TypeGetter.GetClassTypesByBaseType(typePresenter, this.GetType().Assembly))
            {
                var name = item.Name.Split('`');

                if (item.Name != typePresenter.Name && name[0] != "CatalogerPresenter")
                {
                    Type[] interfaces = item.GetInterfaces();

                    serviceCollection.AddTransient(item);
                }
            }

            // Ajoute dans le DI de Microsoft pour les types dérivés de IImaPresenter
            Type typeChildPresenter = typeof(CatalogerChildPresenter<>);

            foreach (Type item in TypeGetter.GetClassTypesByBaseType(typeChildPresenter, this.GetType().Assembly))
            {
                var name = item.Name.Split('`');

                if (name[0] != typePresenter.Name && name[0] != "CatalogerChildPresenter")
                {
                    Type typeInterface = ObtenirChildPresenter(item);

                    BaseComposante.childPresenters.Add(typeInterface.Name, item);
                    serviceCollection.AddTransient(item);
                }
            }
        }

        private Type ObtenirChildPresenter(Type type)
        {
            Type imaChildPresenter = type.GetBaseClassesAndInterfaces().ToList().Find(type => type.Name == typeof(CatalogerChildPresenter<>).Name)!;

            return imaChildPresenter.GetGenericArguments()[0];

        }
    }
}
