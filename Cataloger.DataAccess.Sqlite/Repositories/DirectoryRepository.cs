using Cataloger.Core.Entities.Directories;
using Cataloger.Core.Repositories;
using Zzz.App.Core.AccesDonnees.Sql;
using Zzz.App.Core.AccesDonnees.Sqlite.Sql;

namespace Cataloger.DataAccess.Sqlite.Repositories
{
    public class DirectoryRepository : RepositorySqliteCRUDBase<DirectoryDb>, IDirectoryRepository
    {
        public DirectoryRepository(AppConnexion connexion, IDbObjectProvider dbObjectProvider, ISqlProvider sqlProvider)
            : base(connexion, dbObjectProvider, sqlProvider)
        {
        }
    }
}
