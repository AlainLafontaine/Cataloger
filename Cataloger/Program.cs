using BaseWinform.AccesAction;
using BaseWinform.Interfaces;
using BaseWinform.Presenters;
using BaseWinform.Services;
using BaseWinform.Utilitaires;
using Cataloger.Composantes;
using Cataloger.Core.Entities.SystemsParameters.Dto;
using Cataloger.Presenters;
using Cataloger.Presenters.Bases;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zzz.App.Core.Configuration;
using Zzz.App.Core.IoC;

namespace Cataloger
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // DPI explicite (PerMonitorV2), avant toute UI
            // Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SkinManager.EnableFormSkins();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var host = CreateHostBuilder().Build();
            var config = host.Services.GetRequiredService<IConfigurationApp>();

            // Résolution des composante via DI
            var factory = host.Services.GetRequiredService<IFactory>();
            var navigationService = host.Services.GetRequiredService<NavigationService>();
            var sysParamService = host.Services.GetRequiredService<SystemParameterService>();

            PresenterDirectAccessAction.Init("Cataloger.Business.dll");
            PresenterDirectAccessAction.factory = factory;

            // Pour le support création vis DI des presenter
            CatalogerChildComposante.Factory = factory;

            // Appliquer le skin DevExpress
            SystemParameterDto? skinStyle = sysParamService!.GetSystemParameter("Skin style", "Actif");

            // Si pas de skin style défini dans les paramètres systèmes, on utilise un skin par défaut
            if (skinStyle == null)
            {
                skinStyle = new SystemParameterDto();

                skinStyle.Section = "Skin style";
                skinStyle.Key = "Actif";
                skinStyle.Description = "Skin style actif de l'application";
                skinStyle.ValString = "WXI";
            }
            UserLookAndFeel.Default.SetSkinStyle(skinStyle.ValString);

            // Permet de déterminer si on est en mode design
            var isDesignModeService = host.Services.GetRequiredService<IsDesignModeService>();

            // Chargement de MainForm de l'application
            bool modeTest = bool.Parse(config["App:Demarrer:ModeTest"]) || (Control.ModifierKeys & Keys.Control) == Keys.Control;
            MainForm mainForm = new MainForm(
                navigationService,
                config,
                sysParamService,
                isDesignModeService
            );
            //Type type = typeof(CatalogerComposante);

            // Initialisation des référence pour DI
            navigationService?.Init(mainForm, typeof(CatalogerPresenter<>), [typeof(CatalogerComposante).Assembly]);

            // Chargement de la première composante
            navigationService!.ShowPremierePage(config[modeTest ? "App:Demarrer:PresenterTest" : "App:Demarrer:Presenter"]);

            // Vérifie si on doit ouvrir en mode plien page
            if (bool.Parse(config["App:Demarrer:Maximized"]))
            {
                mainForm.WindowState = FormWindowState.Maximized;
            }

            Application.Run(mainForm);
        }

        static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder().ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                var startup = new Startup();
                startup.ConfigureServices(services);
                // Enregistrement des dépendances ici
            });
    }
}
