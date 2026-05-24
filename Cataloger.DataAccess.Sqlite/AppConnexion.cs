using Zzz.App.Core.AccesDonnees.Sqlite.Sql;
using Zzz.App.Core.Configuration;
using Zzz.App.Core.Logging;
using Zzz.App.Core.Securite;

namespace Cataloger.DataAccess
{
    public class AppConnexion : ConnexionSqlite
    {
        public AppConnexion(ILogger logger, IConfigurationApp configApp, IConfigurationSecuriteApp configSecurite, IGestionnaireSecurite gs)
            : base(logger, configSecurite, gs)
        {
        }
    }
}
