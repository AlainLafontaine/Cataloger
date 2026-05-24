using Cataloger.Core.Entities.TypeDirectories;
using Cataloger.Core.Repositories;
using Zzz.App.Core.AccesDonnees.Sql;
using Zzz.App.Core.AccesDonnees.Sqlite.Sql;

namespace Cataloger.DataAccess.Sqlite.Repositories
{
    public class TypeDirectoryRepository : RepositorySqliteCRUDBase<TypeDirectoryDb>, ITypeDirectoryRepository
    {
        public TypeDirectoryRepository(AppConnexion connexion, IDbObjectProvider dbObjectProvider, ISqlProvider sqlProvider)
            : base(connexion, dbObjectProvider, sqlProvider)
        {
        }
    }
}
