using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Zzz.App.Core.AccesDonnees.Sql;
using Zzz.App.Core.Extensions;
using Zzz.App.Core.Langue;

namespace Zzz.App.Core.AccesDonnees.Sqlite.Sql
{
    /// <summary>
    /// Classe de base pour les repository create/read/update/delete
    /// </summary>
    public abstract class RepositorySqliteCRUDBase<T> : RepositorySqliteCRUDBase<T, T>
        where T : class
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="connexion"></param>
        public RepositorySqliteCRUDBase(ConnexionSqlite connexion, IDbObjectProvider dbObjectProvider, ISqlProvider sqlProvider)
            : base(connexion, dbObjectProvider, sqlProvider)
        {
        }
    }

    /// <summary>
    /// Classe de base pour les repository create/read/update/delete
    /// </summary>
    public abstract class RepositorySqliteCRUDBase<T, D> : RepositoryCRUDBase<T, D>
        where T : class
        where D : class
    {

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="connexion"></param>
        public RepositorySqliteCRUDBase(ConnexionSqlite connexion, IDbObjectProvider dbObjectProvider, ISqlProvider sqlProvider)
            : base(connexion, dbObjectProvider, sqlProvider)
        {
        }

        /// <summary>
        /// Execute une requete qui n'est pas un "Select"
        /// et retourne le nombre de rangée affectée
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parametres"></param>
        /// <returns></returns>
        protected override int ExecuteNonQuery(string sql, IEnumerable<IDbDataParameter> parametres)
        {

            if (!parametres.Any())
            {
                using (var com = this.dbObjectProvider.CreateCommand(this.Connexion))
                {
                    com.CommandType = CommandType.Text;
                    com.CommandText = sql;
                    return com.ExecuteNonQuery();
                }
            }

            int retour = 0;
            bool valeurEstCollection = parametres.ElementAt(0).Value.GetType().EstCollection();

            int nbrLigne = valeurEstCollection ?
                ((object[])parametres.ElementAt(0).Value).Length : 1;


            for (int i = 0; i < nbrLigne; i++)
            {
                using (var com = this.dbObjectProvider.CreateCommand(this.Connexion))
                {

                    com.CommandType = CommandType.Text;
                    com.CommandText = sql;
                    foreach (SqliteParameter param in parametres)
                    {

                        var newParam = (SqliteParameter)this.dbObjectProvider.GetParametre();
                        newParam.ParameterName = param.ParameterName;
                        newParam.SqliteType = param.SqliteType;

                        newParam.Value = valeurEstCollection ?
                                            ((object[])param.Value)[i] ?? DBNull.Value :
                                            param.Value ?? DBNull.Value;

                        com.Parameters.Add(newParam);
                    }

                    retour += com.ExecuteNonQuery();
                }
            }

            return retour;
        }

        protected override IEnumerable<T> CreerAffecterIdEntites(int nombre)
        {
            var entites = new List<T>();
            using (var com = this.CreateCommand())
            {
                com.CommandText = $"SELECT IFNULL(MAX({this.champsClePrimaire.First().NomBd}),0) AS max_id FROM {this.NomTable}";
                var id = (long)com.ExecuteScalar();
                var propClePrimaire = this.champsClePrimaire.First().Propriete;
                for (int i = 0; i < nombre; i++)
                {
                    id++;
                    var entite = Activator.CreateInstance<T>();
                    propClePrimaire.SetValue(entite, id, null);
                    entites.Add(entite);
                }
            }

            return entites;
        }
    }
}
