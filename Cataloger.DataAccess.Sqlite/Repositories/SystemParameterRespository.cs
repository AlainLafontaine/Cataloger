using Cataloger.Core.Entities.SystemsParameters;
using Cataloger.Core.Repositories;
using Zzz.App.Core.AccesDonnees.Sql;
using Zzz.App.Core.AccesDonnees.Sqlite.Sql;

namespace Cataloger.DataAccess.Sqlite.Repositories
{
    public class SystemParameterRespository : RepositorySqliteCRUDBase<SystemParameterDb>, ISystemParameterRepository
    {
        public SystemParameterRespository(AppConnexion connexion, IDbObjectProvider dbObjectProvider, ISqlProvider sqlProvider)
            : base(connexion, dbObjectProvider, sqlProvider)
        {
        }
    }
}
