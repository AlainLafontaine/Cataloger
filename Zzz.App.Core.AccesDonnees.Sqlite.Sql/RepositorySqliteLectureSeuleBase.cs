using System;
using System.Collections.Generic;
using System.Text;
using Zzz.App.Core.AccesDonnees.Sql;

namespace Zzz.App.Core.AccesDonnees.Sqlite.Sql
{

    public abstract class RepositorySqliteLectureSeuleBase<T> : RepositorySqliteLectureSeuleBase<T, T>
        where T : class
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="connexion"></param>
        public RepositorySqliteLectureSeuleBase(ConnexionSqlite connexion, IDbObjectProvider dbObjectProvider, ISqlProvider sqlProvider)
            : base(connexion, dbObjectProvider, sqlProvider)
        {
        }

    }

    public abstract class RepositorySqliteLectureSeuleBase<T, D> : RepositoryLectureSeule<T, D>
        where T : class
        where D : class
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="connexion"></param>
        public RepositorySqliteLectureSeuleBase(ConnexionSqlite connexion, IDbObjectProvider dbObjectProvider, ISqlProvider sqlProvider)
            : base(connexion, dbObjectProvider, sqlProvider)
        {
        }
    }
}
