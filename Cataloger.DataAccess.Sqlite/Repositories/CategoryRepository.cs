using Cataloger.Core.Entities.Categories;
using Cataloger.Core.Repositories;
using Zzz.App.Core.AccesDonnees.Sql;
using Zzz.App.Core.AccesDonnees.Sqlite.Sql;

namespace Cataloger.DataAccess.Sqlite.Repositories
{
    public class CategoryRepository : RepositorySqliteCRUDBase<CategoryDb>, ICategoryRepository
    {
        public CategoryRepository(AppConnexion connexion, IDbObjectProvider dbObjectProvider, ISqlProvider sqlProvider)
            : base(connexion, dbObjectProvider, sqlProvider)
        {
        }
    }
}
