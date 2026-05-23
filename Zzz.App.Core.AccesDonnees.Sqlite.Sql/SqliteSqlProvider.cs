using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Zzz.App.Core.AccesDonnees.Sql;
using Zzz.App.Core.AccesDonnees.Sql.Dto;
using Zzz.App.Core.Entites.Grille;
using Zzz.App.Core.Extensions;

namespace Zzz.App.Core.AccesDonnees.Sqlite.Sql
{
    public class SqliteSqlProvider : ISqlProvider
	{
        public string SqlParameterChar => ":";

        public virtual string ObtenirSqlAjouter(string nomTable, IEnumerable<ProprieteChampBd> proprietesChampBd)
        {
            var sqlChamps = string.Join(", ", proprietesChampBd.Where(x => x.LectureSeule == false).Select(c => c.NomBd).ToArray());
            var sqlValues = string.Join($", {this.SqlParameterChar}", proprietesChampBd.Where(x => x.LectureSeule == false).Select(c => c.NomBd).ToArray());

            var sql = string.Format("INSERT INTO {0}({1}) VALUES ({3}{2})", nomTable, sqlChamps, sqlValues, this.SqlParameterChar);

            return sql;
        }

        public virtual string ObtenirSqlCount(string sql)
        {
            return $"SELECT COUNT(1) FROM ({sql}) AS SUBSQL";
        }

        public virtual string ObtenirSqlBaseEntite(string nomTable, IEnumerable<ProprieteChampBd> proprietesChampBd)
        {
            string select = this.ObtenirSqlSelectColonnes(proprietesChampBd);
            var sql = $"SELECT {select} FROM {nomTable}";
            return sql;
        }

        public virtual IEnumerable<string> ObtenirListeSqlWhereSelonParametres(IEnumerable<IDbDataParameter> parametres, List<string> autreWheres = null, string prefixe = null)
        {
            IEnumerable<string> wheres;
            if (prefixe.IsPresent())
                wheres = parametres.Select(p => $"{prefixe}.{p.ParameterName} = {this.SqlParameterChar}{p.ParameterName}").ToArray();
            else
                wheres = parametres.Select(p => string.Format("{0} = {1}{0}", p.ParameterName, this.SqlParameterChar)).ToArray();

            if (autreWheres != null)
            {
                autreWheres.AddRange(wheres);
                return autreWheres;
            }

            return wheres;
        }

        public virtual string ObtenirSqlWhereSelonParametres(IEnumerable<IDbDataParameter> parametres, List<string> autreWheres = null, string prefixe = null)
        {
            return string.Join(" AND ", this.ObtenirListeSqlWhereSelonParametres(parametres, autreWheres, prefixe));
        }

        public virtual IEnumerable<string> ObtenirListeSqlSelectColonnes(IEnumerable<ProprieteChampBd> proprietesChampBd, List<string> autresColonnes = null, string prefixe = null)
        {
            IEnumerable<string> cols;

            if (prefixe.IsPresent())
                cols = proprietesChampBd.Select(c => $"{prefixe}.{c.NomBd} as {c.Propriete.Name}").ToArray();
            else
                cols = proprietesChampBd.Select(c => $"{c.NomBd} as {c.Propriete.Name}").ToArray();

            if (autresColonnes != null)
            {
                autresColonnes.AddRange(cols);
                return autresColonnes;
            }

            return cols;
        }

        public virtual string ObtenirSqlSelectColonnes(IEnumerable<ProprieteChampBd> proprietesChampBd, List<string> autresColonnes = null, string prefixe = null)
        {
            return string.Join(", ", this.ObtenirListeSqlSelectColonnes(proprietesChampBd, autresColonnes, prefixe));
        }

        public virtual string ObtenirSqlModifier(string nomTable, IEnumerable<ProprieteChampBd> champsClePrimaire, IEnumerable<ProprieteChampBd> proprieteChamps)
        {
            //On va chercher les champs à affecter qui ne sont pas des clés primaires
            var sqlSet = string.Join(", ", proprieteChamps
                                          .Where(c => !champsClePrimaire.Any(cle => cle.Equals(c)) && c.LectureSeule == false)
                                          .Select(c => string.Format("{0} = {1}{0}", c.NomBd, this.SqlParameterChar)).ToArray());
            var sqlWhere = string.Join(" AND ", champsClePrimaire.Select(c => string.Format("{0} = {1}{0}", c.NomBd, this.SqlParameterChar)).ToArray());

            var sql = string.Format("UPDATE {0} SET {1} WHERE {2}", nomTable, sqlSet, sqlWhere);

            return sql;
        }

        public virtual string ObtenirSqlSupprimer(string nomTable, IEnumerable<ProprieteChampBd> proprieteChamps)
        {
            var sqlWhere = string.Join(" AND ", proprieteChamps.Select(c => string.Format("{0} = {1}{0}", c.NomBd, this.SqlParameterChar)).ToArray());

            var sql = string.Format("DELETE FROM {0} WHERE {1}", nomTable, sqlWhere);

            return sql;
        }

        public virtual string ObtenirSqlPagination(string sql, int page, int pageSize, string orderBy, string orderDirection)
        {
            return this.ObtenirSqlPagination(sql, page, pageSize, $"{orderBy} {orderDirection}");
        }

        public virtual string ObtenirSqlPagination(string sql, int page, int pageSize, string orderByDirection)
        {
            string orderBySql = string.IsNullOrEmpty(orderByDirection) ? string.Empty : $" ORDER BY {orderByDirection}";
            string paginationSql = this.ObtenirSqlPagination(page, pageSize);

            return $"{sql} {orderBySql} {paginationSql}";
        }

        public virtual string ObtenirSqlPagination(int page, int pageSize)
        {
            int rownumMin = page * pageSize - pageSize;

            return $" LIMIT {rownumMin}, {pageSize} ";
        }

        public virtual (string Sql, string SqlPage) ObtenirSqlPaginationCriteres(string sql, CriteresDto criteres)
        {
            return this.ObtenirSqlPaginationCriteres(sql, criteres, null, null);
        }

        public virtual (string Sql, string SqlPage) ObtenirSqlPaginationCriteres(string sql, CriteresDto criteres, IEnumerable<string> colonnesRechercheWildCard)
        {
            return this.ObtenirSqlPaginationCriteres(sql, criteres, colonnesRechercheWildCard, null);
        }

        public virtual (string Sql, string SqlPage) ObtenirSqlPaginationCriteres(string sql, CriteresDto criteres, IEnumerable<ProprieteChampBd> proprietesChampBd)
        {
            return this.ObtenirSqlPaginationCriteres(sql, criteres, null, proprietesChampBd);
        }

        public virtual (string Sql, string SqlPage) ObtenirSqlPaginationCriteres(string sql, CriteresDto criteres, IEnumerable<string> colonnesRechercheWildCard, IEnumerable<ProprieteChampBd> proprietesChampBd)
        {
            var jointureWhere = " WHERE";

            if (sql.Contains("WHERE")) //Risqué...
            {
                jointureWhere = " AND";
            }

            var sqlFiltres = this.ObtenirWhereCriteres(criteres, proprietesChampBd);
            if (!string.IsNullOrWhiteSpace(sqlFiltres))
            {
                sql += $" {jointureWhere} {sqlFiltres}";
                jointureWhere = "AND";
            }

            var sqlRechercheWildcard = this.ObtenirWhereRechercheWildcard(criteres, colonnesRechercheWildCard, proprietesChampBd);
            if (!string.IsNullOrWhiteSpace(sqlRechercheWildcard))
            {
                sql += $" {jointureWhere} {sqlRechercheWildcard}";
            }

            var sqlOrderBy = new List<string>();

            foreach (var item in criteres.Tris)
            {
                sqlOrderBy.Add($"{item.Colonne} {item.Order}");
            }

            var sqlPage = this.ObtenirSqlPagination(sql, criteres.Page, criteres.Take, string.Join(", ", sqlOrderBy));

            return (sql, sqlPage);
        }


        [Obsolete]
        public virtual string ObtenirWhereParCritere(string sql, string criteres, IEnumerable<ColonneAFiltrer> colonnesAfiltrer)
        {
            var criteresRecherche = criteres.Trim().Split(" ").Select(x => x.Trim());
            StringBuilder sqlCriteres = new StringBuilder();

            if (sql.Contains("WHERE"))
            {
                if (!sql.ToUpper().Trim().EndsWith("AND"))
                {
                    sql += " AND ";
                }
            }
            else
            {
                sql += " WHERE ";
            }

            for (int index = 0; index < criteresRecherche.Count(); index++)
            {
                var c = criteresRecherche.ElementAt(index);
                if (index > 0)
                {
                    sqlCriteres.Append(" AND ");
                }

                sqlCriteres.Append("(");


                //TODO : A corriger pour sqlite
                for (int indexChampBD = 0; indexChampBD < colonnesAfiltrer.Count(); indexChampBD++)
                {
                    if (indexChampBD > 0)
                    {
                        sqlCriteres.Append(" OR ");
                    }

                    sqlCriteres.Append("UPPER(");
                    if (!colonnesAfiltrer.ElementAt(indexChampBD).MaskFormat.IsNullOrWhiteSpace())
                    {
                        sqlCriteres.Append($"TO_CHAR({colonnesAfiltrer.ElementAt(indexChampBD).NomColonne}, '{colonnesAfiltrer.ElementAt(indexChampBD).MaskFormat}'))");
                    }
                    else
                    {
                        sqlCriteres.Append($"{colonnesAfiltrer.ElementAt(indexChampBD).NomColonne})");
                    }
                    sqlCriteres.Append(" LIKE ");
                    sqlCriteres.Append($"'%{c.ToString().ToUpper()}%'");
                }

                sqlCriteres.Append(")");
            }

            sql = $"{sql} {sqlCriteres.ToString()}";
            return sql;
        }

        public virtual string ObtenirWhereCriteres(CriteresDto criteres)
        {
            return this.ObtenirWhereCriteres(criteres, new List<ProprieteChampBd>());
        }

        public virtual string ObtenirWhereCriteres(CriteresDto criteres, IEnumerable<ProprieteChampBd> proprieteChampBds)
        {
            return this.ObtenirWhereCriteres(criteres, proprieteChampBds, "AND");
        }

        public virtual string ObtenirWhereCriteres(CriteresDto criteres, string join)
        {
            return this.ObtenirWhereCriteres(criteres, new List<ProprieteChampBd>(), join);
        }

        public virtual string ObtenirWhereCriteres(CriteresDto criteres, IEnumerable<ProprieteChampBd> proprieteChampBds, string join)
        {
            if (criteres == null || criteres.Filtres == null) return string.Empty;

            var arrSql = new List<string>();

            join = string.IsNullOrEmpty(join) ? "AND" : join;

            if (proprieteChampBds == null)
            {
                proprieteChampBds = new List<ProprieteChampBd>();
            }

            foreach (var filtre in criteres.Filtres)
            {
                var propChampBd = proprieteChampBds.FirstOrDefault(p => p.Propriete.Name.Equals(filtre.Colonne, StringComparison.OrdinalIgnoreCase));
                arrSql.Add(this.ObtenirSqlFiltre(filtre, propChampBd, criteres.IgnoreCase));
            }

            return string.Join($" {join} ", arrSql);
        }

        public virtual string ObtenirWhereRechercheWildcard(CriteresDto criteres, IEnumerable<string> colonnes)
        {
            return this.ObtenirWhereRechercheWildcard(criteres, colonnes, null);
        }

        public virtual string ObtenirWhereRechercheWildcard(CriteresDto criteres, IEnumerable<string> colonnes, IEnumerable<ProprieteChampBd> proprieteChampBds)
        {
            if (criteres == null || string.IsNullOrEmpty(criteres.Recherche)) return string.Empty;

            if (proprieteChampBds == null)
            {
                proprieteChampBds = new List<ProprieteChampBd>();
            }

            if (colonnes == null || !colonnes.Any())
            {
                colonnes = proprieteChampBds.Select(p => p.Propriete.Name);
            }

            if (!colonnes.Any()) throw new ArgumentException("[SqlProvider] ObtenirWhereRechercheWildcard : Colonnes ou proprieteChampBds sont obligatoire");

            var arrSql = new List<string>();

            var join = "OR";

            var termesRecherche = criteres.Recherche.Split(' ');

            foreach (var colonne in colonnes)
            {
                int indexTermes = 1;
                foreach (var terme in termesRecherche)
                {
                    var nomParam = $"{ConstantesNoyau.AccesDonnees.Criteres.PrefixParamWC}{indexTermes}";
                    var propChampBd = proprieteChampBds.FirstOrDefault(p => p.Propriete.Name.Equals(colonne, StringComparison.OrdinalIgnoreCase));

                    var filtre = new FiltreDto
                    {
                        Colonne = colonne,
                        Valeur = terme,
                        Operateur = ConstantesNoyau.AccesDonnees.Criteres.Operateurs.Contient
                    };

                    arrSql.Add($"({this.ObtenirSqlFiltre(filtre, nomParam, propChampBd, criteres.IgnoreCase)})");
                    indexTermes++;
                }
            }

            return $"({string.Join($" {join} ", arrSql)})";
        }

        public virtual string ObtenirOrderByDirectionTris(IEnumerable<TriDto> tris)
        {
            if (tris == null) return string.Empty;

            var arrOrder = new List<string>();

            foreach (var tri in tris)
            {
                var colonne = tri.Colonne;
                arrOrder.Add($"{colonne} {tri.Order}");
            }

            return string.Join(", ", arrOrder);
        }

        public virtual string ObtenirOrderByDirectionCriteres(CriteresDto criteres)
        {
            return this.ObtenirOrderByDirectionCriteres(criteres, new List<ProprieteChampBd>());
        }

        public virtual string ObtenirOrderByDirectionCriteres(CriteresDto criteres, IEnumerable<ProprieteChampBd> proprieteChampBds)
        {
            if (criteres == null || criteres.Tris == null) return string.Empty;

            var arrOrder = new List<string>();

            if (proprieteChampBds == null)
            {
                proprieteChampBds = new List<ProprieteChampBd>();
            }

            foreach (var tri in criteres.Tris)
            {
                var colonne = tri.Colonne;
                var propChampBd = proprieteChampBds.FirstOrDefault(p => p.Propriete != null && p.Propriete.Name.Equals(tri.Colonne, StringComparison.OrdinalIgnoreCase));
                if (propChampBd != null)
                {
                    colonne = propChampBd.NomBd;
                }

                arrOrder.Add($"{colonne} {tri.Order}");
            }

            return string.Join(", ", arrOrder);
        }

        public virtual string ObtenirSqlFiltre(FiltreDto filtre, bool ignoreCase = true)
        {
            return this.ObtenirSqlFiltre(filtre, null, ignoreCase);
        }

        public virtual IEnumerable<string> ObtenirSqlFiltres(IEnumerable<FiltreDto> filtres, bool ignoreCase = false)
        {
            foreach (var filtre in filtres ?? new List<FiltreDto>())
            {
                yield return this.ObtenirSqlFiltre(filtre, ignoreCase);
            }
        }

        public virtual string ObtenirSqlFiltre(FiltreDto filtre, ProprieteChampBd proprietesChampBd, bool ignoreCase = true)
        {
            return this.ObtenirSqlFiltre(filtre, filtre.Colonne, proprietesChampBd, ignoreCase);
        }

        public virtual string ObtenirSqlFiltre(FiltreDto filtre, Type typeParametre, string colonne, string nomParametre, bool ignoreCase = false)
        {
            if (filtre != null)
            {
                if (filtre.Operateur.IsMissing())
                    filtre.Operateur = ConstantesNoyau.AccesDonnees.Criteres.Operateurs.Contient;

                var sql = "";
                typeParametre ??= typeof(string);

                //TODO : A corriger pour sqlite
                switch (filtre.Operateur.SafeToLower())
                {
                    case ConstantesNoyau.AccesDonnees.Criteres.Operateurs.Egale:
                        sql = $"{colonne} = {this.SqlParameterChar}{nomParametre}";
                        break;
                    case ConstantesNoyau.AccesDonnees.Criteres.Operateurs.Different:
                        sql = $"{colonne} != {this.SqlParameterChar}{nomParametre}";
                        break;
                    case ConstantesNoyau.AccesDonnees.Criteres.Operateurs.Contient:
                        if (typeParametre.Equals(typeof(DateTime)) || filtre.TypeColonne == ConstantesNoyau.AccesDonnees.Criteres.TypeColonne.Date)
                        {
                            sql = $"TO_CHAR({colonne}, 'YYYY-MM-DD') LIKE '%' || {this.SqlParameterChar}{nomParametre} || '%'";
                            break;
                        }

                        if (ignoreCase)
                        {
                            sql = $"UPPER({colonne}::text) LIKE '%' || UPPER({this.SqlParameterChar}{nomParametre}::text) || '%'";
                            break;
                        }

                        sql = $"{colonne} LIKE '%' || {this.SqlParameterChar}{nomParametre} || '%'";
                        break;
                    case ConstantesNoyau.AccesDonnees.Criteres.Operateurs.NeContientPas:
                        if (typeParametre.Equals(typeof(DateTime)) || filtre.TypeColonne == ConstantesNoyau.AccesDonnees.Criteres.TypeColonne.Date)
                        {
                            sql = $"TO_CHAR({colonne}, 'YYYY-MM-DD') NOT LIKE '%' || {this.SqlParameterChar}{nomParametre} || '%'";
                            break;
                        }

                        if (ignoreCase)
                        {
                            sql = $"UPPER({colonne}::text) NOT LIKE '%' || UPPER({this.SqlParameterChar}{nomParametre}::text) || '%'";
                            break;
                        }

                        sql = $"{colonne} NOT LIKE '%' || {this.SqlParameterChar}{nomParametre} || '%'";
                        break;

                    case ConstantesNoyau.AccesDonnees.Criteres.Operateurs.DebutePar:
                        if (typeParametre.Equals(typeof(DateTime)) || filtre.TypeColonne == ConstantesNoyau.AccesDonnees.Criteres.TypeColonne.Date)
                        {
                            sql = $"TO_CHAR({colonne}, 'YYYY-MM-DD') LIKE {this.SqlParameterChar}{nomParametre} || '%'";
                            break;
                        }

                        if (ignoreCase)
                        {
                            sql = $"UPPER({colonne}::text) LIKE UPPER({this.SqlParameterChar}{nomParametre}::text) || '%'";
                            break;
                        }

                        sql = $"{colonne} LIKE {this.SqlParameterChar}{nomParametre} || '%'";
                        break;
                    case ConstantesNoyau.AccesDonnees.Criteres.Operateurs.TerminePar:
                        if (typeParametre.Equals(typeof(DateTime)) || filtre.TypeColonne == ConstantesNoyau.AccesDonnees.Criteres.TypeColonne.Date)
                        {
                            sql = $"TO_CHAR({colonne}, 'YYYY-MM-DD') LIKE '%' || {this.SqlParameterChar}{nomParametre}";
                            break;
                        }

                        if (ignoreCase)
                        {
                            sql = $"UPPER({colonne}::text) LIKE '%' || UPPER({this.SqlParameterChar}{nomParametre}::text)";
                            break;
                        }

                        sql = $"{colonne} LIKE '%' || {this.SqlParameterChar}{nomParametre}";
                        break;
                    case ConstantesNoyau.AccesDonnees.Criteres.Operateurs.PlusGrand:
                        sql = $"{colonne} > {this.SqlParameterChar}{nomParametre}";
                        break;
                    case ConstantesNoyau.AccesDonnees.Criteres.Operateurs.PlusGrandOuEgale:
                        sql = $"{colonne} >= {this.SqlParameterChar}{nomParametre}";
                        break;
                    case ConstantesNoyau.AccesDonnees.Criteres.Operateurs.PlusPetit:
                        sql = $"{colonne} < {this.SqlParameterChar}{nomParametre}";
                        break;
                    case ConstantesNoyau.AccesDonnees.Criteres.Operateurs.PlusPetitOuEgale:
                        sql = $"{colonne} <= {this.SqlParameterChar}{nomParametre}";
                        break;
                    case ConstantesNoyau.AccesDonnees.Criteres.Operateurs.Entre:
                        {

                            var nomParamMin = nomParametre + "min";
                            var nomParamMax = nomParametre + "max";
                            sql = $"({colonne} BETWEEN {this.SqlParameterChar}{nomParamMin} AND {this.SqlParameterChar}{nomParamMax})";
                            break;
                        }
                    case ConstantesNoyau.AccesDonnees.Criteres.Operateurs.PasEntre:
                        {
                            var nomParamMin = nomParametre + "min";
                            var nomParamMax = nomParametre + "max";
                            sql = $"( {colonne} NOT BETWEEN {this.SqlParameterChar}{nomParamMin} AND {this.SqlParameterChar}{nomParamMax} )";
                            break;
                        }
                    default:
                        throw new NotSupportedException($"L'opérateur '{filtre.Operateur}' n'est pas supporté");
                }

                return sql;
            }


            return "";
        }

        public virtual string ObtenirSqlFiltre(FiltreDto filtre, string nomParametre, ProprieteChampBd proprietesChampBd, bool ignoreCase = true)
        {
            var typeParametre = proprietesChampBd?.Propriete?.PropertyType ?? typeof(string);
            var colonne = proprietesChampBd?.NomBd ?? filtre.Colonne;
            return this.ObtenirSqlFiltre(filtre, typeParametre, colonne, nomParametre, ignoreCase);
        }

        public virtual ISqlBuilder CreerSqlBuilder()
        {
            return new SqliteSqlBuilder(this);
        }


    }
}
