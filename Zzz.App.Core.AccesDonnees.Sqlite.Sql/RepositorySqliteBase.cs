using Zzz.App.Core.AccesDonnees.Sql;
using Zzz.App.Core.Langue;

namespace Zzz.App.Core.AccesDonnees.Sqlite.Sql
{
    /// <summary>
    /// Classe de base pour les repository
    /// </summary>
    public abstract class RepositorySqLiteBase : RepositoryBase
    {
        
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="connexion"></param>
        public RepositorySqLiteBase(ConnexionSqlite connexion, IDbObjectProvider dbObjectProvider, ISqlProvider sqlProvider)
            : base(connexion, dbObjectProvider, sqlProvider)
        {            
        }
    }
}
