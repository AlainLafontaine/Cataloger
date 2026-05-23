using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Zzz.App.Core.AccesDonnees.Sql;
using Zzz.App.Core.Entites.Grille;
using Zzz.App.Core.Extensions;

namespace Zzz.App.Core.AccesDonnees.Sqlite.Sql
{
    public class SqliteSqlBuilder : ISqlBuilder
    {
        public string SqlParameterChar => ":";
        public string Id { get; private set; }
        public string Alias { get; private set; }

        public bool Distinct { get; private set; }
        protected List<ISqlBuilder> sqlBuilders = new List<ISqlBuilder>();
        protected List<string> select = new List<string>();
        protected List<string> from = new List<string>();
        protected List<string> tris = new List<string>();
        protected List<string> wheres = new List<string>();
        protected List<string> wheresCritere = new List<string>();
        protected List<IDbDataParameter> parametres = new List<IDbDataParameter>();
        protected string whereString = string.Empty;
        protected string whereCritereString = string.Empty;
        protected string page;
        private readonly ISqlProvider sqlProvider;

        public List<string> Wheres { get { return this.wheres; } }
        public List<string> WheresCritere { get { return this.wheresCritere; } }
        public List<string> Tris { get { return this.tris; } }
        public string WhereString { get { return this.whereString; } }
        public string WhereCritereString { get { return this.whereCritereString; } }

        public SqliteSqlBuilder(ISqlProvider sqlProvider)
        {
            this.Id = $"[__{DateTime.Now.Ticks}__]";
            this.sqlProvider = sqlProvider;
        }

        public SqliteSqlBuilder(ISqlProvider sqlProvider, bool distinct) :
            this(sqlProvider)
        {
            this.Distinct = distinct;
        }

        public virtual void SetAlias(string alias)
        {
            this.Alias = alias;
            var i = 0;
            foreach (var builder in this.sqlBuilders)
            {
                builder.SetAlias($"{this.Alias}{i++}");
            }
        }

        public static SqliteSqlBuilder Creer()
        {
            return new SqliteSqlBuilder(new SqliteSqlProvider());
        }

        public virtual IEnumerable<IDbDataParameter> Parametres
        {
            get
            {
                var p = this.parametres;

                foreach (var sqlBuilder in this.sqlBuilders)
                {
                    p.AddRange(sqlBuilder.Parametres);
                }
                return p;
            }
        }

        public virtual ISqlBuilder Select(params string[] select)
        {
            this.select.AddRange(select);
            return this;
        }

        public virtual ISqlBuilder Select(IEnumerable<string> select)
        {
            this.select.AddRange(select);
            return this;
        }

        public virtual ISqlBuilder Select(ISqlBuilder selectBuilder, string alias)
        {
            if (selectBuilder != null)
            {
                this.select.Add($"({selectBuilder.Id}) AS {alias}");
                this.sqlBuilders.Add(selectBuilder);
            }

            return this;
        }

        public virtual ISqlBuilder From(params string[] from)
        {
            this.from.AddRange(from);
            return this;
        }

        public virtual ISqlBuilder From(string table, string alias)
        {
            if (table.IsPresent())
            {
                this.from.Add($"{table} {alias}");
            }

            return this;
        }

        public virtual ISqlBuilder From(ISqlBuilder builder, string alias)
        {
            if (alias.IsMissing())
                alias = builder.Id;

            return this.From($"({builder})", alias);
        }

        public virtual ISqlBuilder Where(params string[] where)
        {
            this.wheres.AddRange(where);
            return this;
        }

        public virtual ISqlBuilder WhereCritere(params string[] where)
        {
            this.wheresCritere.AddRange(where);
            return this;
        }

        public virtual ISqlBuilder Where(IEnumerable<string> wheres)
        {
            this.wheres.AddRange(wheres);
            return this;
        }

        public virtual ISqlBuilder WhereCritere(IEnumerable<string> wheres)
        {
            this.wheresCritere.AddRange(wheres);
            return this;
        }

        public virtual ISqlBuilder Where(string where, ISqlBuilder whereBuilder)
        {
            this.wheres.Add($"{where} ({whereBuilder.Id})");
            this.sqlBuilders.Add(whereBuilder);
            return this;
        }

        public virtual ISqlBuilder WhereCritere(string where, ISqlBuilder whereBuilder)
        {
            this.wheresCritere.Add($"{where} ({whereBuilder.Id})");
            this.sqlBuilders.Add(whereBuilder);
            return this;
        }

        public virtual ISqlBuilder Where(string colonne, IDbDataParameter parametre)
        {
            if (parametre != null && colonne.IsPresent())
            {
                this.Where($"{colonne} = {this.SqlParameterChar}{parametre.ParameterName}");
            }

            return this;
        }

        public virtual ISqlBuilder WhereCritere(string colonne, IDbDataParameter parametre)
        {
            if (parametre != null && colonne.IsPresent())
            {
                this.WhereCritere($"{colonne} = {this.SqlParameterChar}{parametre.ParameterName}");
            }

            return this;
        }

        public virtual ISqlBuilder OrderBy(IEnumerable<TriDto> tris)
        {
            if (tris != null)
            {
                foreach (var t in tris)
                    this.OrderBy(t);
            }

            return this;
        }

        public virtual ISqlBuilder OrderBy(TriDto tri)
        {
            if (tri != null && tri.Order.IsPresent())
            {
                // La valeur order doit être supporté
                if (!ConstantesNoyau.AccesDonnees.Criteres.Order.Supporte.Contains(tri.Order.SafeToLower()))
                {
                    throw new NotSupportedException($"{tri.Order} n'est pas supporté pour la commande Order By (supporté : {ConstantesNoyau.AccesDonnees.Criteres.Order.Supporte.ToFormatString()}");
                }

                this.tris.Add($"{ tri.Colonne } { tri.Order.ToUpper()}");
            }

            return this;
        }

        public virtual ISqlBuilder AjouterParametres(params IDbDataParameter[] parametres)
        {
            this.parametres.AddRange(parametres);
            return this;
        }

        private string BuildWhereIn<T>(IList<IDbDataParameter> parametres, List<T> valeurs, string nomColonne, string nomParametre, IDbObjectProvider dbObjectProvider)
        {
            if (valeurs.Count() == 0)
                return string.Empty;

            var i = 0;
            var builder = new StringBuilder();
            builder.Append("(");
            foreach (var v in valeurs)
            {
                if (i > 0)
                    builder.Append(" OR ");
                var p = dbObjectProvider.GetParametre(v, nomParametre + i);
                parametres.Add(p);
                builder.Append($"{nomColonne} = {SqlParameterChar}{p.ParameterName}");
                i++;
            }

            builder.Append(")");



            return builder.ToString();
        }

        public virtual ISqlBuilder WhereIn<T>(IList<IDbDataParameter> parametres, List<T> valeurs, string nomColonne, string nomParametre, IDbObjectProvider dbObjectProvider)
        {
            var where = this.BuildWhereIn<T>(parametres, valeurs, nomColonne, nomParametre, dbObjectProvider);
            if (where.IsPresent())
            {
                this.Where(where);
            }

            return this;
        }

        public virtual ISqlBuilder WhereInCritere<T>(IList<IDbDataParameter> parametres, List<T> valeurs, string nomAttribut, string nomParametre, IDbObjectProvider dbObjectProvider)
        {
            var where = this.BuildWhereIn<T>(parametres, valeurs, nomAttribut, nomParametre, dbObjectProvider);
            if (where.IsPresent())
            {
                this.WhereCritere(where);
            }

            return this;
        }

        public virtual ISqlBuilder Page(CriteresDto criteres)
        {
            if (criteres != null)
            {
                int rownumMin = criteres.Page * criteres.Take - criteres.Take;
                int rownumMax = rownumMin + criteres.Take - 1;


                this.page = $" LIMIT {this.SqlParameterChar}rownumMin, {criteres.Take} "; //TODO : utiliser le SqlProvider
            }

            return this;
        }

        public override string ToString()
        {
            return this.ToString(true);
        }

        protected virtual string GenererWhere(FiltreDto filtre, Type typeParametre, string colonne, string nomParametre, bool ignoreCase = false)
        {
            return this.sqlProvider.ObtenirSqlFiltre(filtre, typeParametre, colonne, nomParametre, ignoreCase);
        }

        public virtual ISqlBuilder Where(FiltreDto filtre, Type typeParametre, string colonne, string nomParametre, bool ignoreCase = false)
        {
            return this.Where(this.GenererWhere(filtre, typeParametre, colonne, nomParametre, ignoreCase));
        }

        public virtual ISqlBuilder WhereCritere(FiltreDto filtre, Type typeParametre, string colonne, string nomParametre, bool ignoreCase = false)
        {
            return this.WhereCritere(this.GenererWhere(filtre, typeParametre, colonne, nomParametre, ignoreCase));
        }

        public virtual ISqlBuilder Where(FiltreDto filtre, string colonne, string nomParametre, bool ignoreCase = false)
        {
            return this.Where(filtre, null, colonne, nomParametre, ignoreCase);
        }


        public virtual ISqlBuilder WhereCritere(FiltreDto filtre, string colonne, string nomParametre, bool ignoreCase = false)
        {
            return this.WhereCritere(filtre, null, colonne, nomParametre, ignoreCase);
        }

        public virtual string BuildWheres(string join = "AND")
        {
            if (this.Wheres.Any())
            {
                if (this.whereString.IsMissing())
                    this.whereString = string.Join($" {join} ", this.Wheres.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());
                else
                    this.whereString = " AND (" + string.Join($" {join} ", this.Wheres.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray()) + ")";
            }

            this.Wheres.Clear();
            return this.whereString;
        }

        public virtual string BuildWheresCritere(string join = "AND")
        {
            if (this.WheresCritere.Any())
            {
                if (this.whereCritereString.IsMissing())
                    this.whereCritereString = string.Join($" {join} ", this.WheresCritere.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());
                else
                    this.whereCritereString = " AND (" + string.Join($" {join} ", this.WheresCritere.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray()) + ")";
            }

            this.WheresCritere.Clear();
            return this.whereCritereString;
        }

        public virtual string ToString(bool pagination = true)
        {
            var sqlSelect = this.SqlSelect();
            if (this.Alias.IsMissing())
                this.SetAlias("T");

            var sqlFrom = this.SqlFrom();
            var sqlWhere = this.SqlWhere();
            var sqlWhereCritere = this.SqlWhereCritere();

            string innerSql = $"{sqlSelect.Trim()} {sqlFrom.Trim()} {sqlWhere.Trim()} {this.SqlOrderBy().Trim()}";
            string sql = $"SELECT {this.Alias}.* FROM ({innerSql.Trim()}) {this.Alias} {sqlWhereCritere.Trim()}".Trim();

            if (pagination)
                sql += this.SqlPage();

            var i = 0;
            foreach (var sqlBuilder in this.sqlBuilders)
            {
                sqlBuilder.SetAlias($"{this.Alias}{i++}");
                sql = sql.Replace(sqlBuilder.Id, sqlBuilder.ToString());
            }

            return sql.ToString();
        }

        public virtual string SqlFrom()
        {
            return $"FROM {string.Join(",", this.from)} ";
        }

        public virtual string SqlSelect()
        {
            var sqlSelect = $"SELECT ";

            if (this.Distinct)
            {
                sqlSelect = string.Concat(sqlSelect, "DISTINCT ");
            }

            sqlSelect = string.Concat(sqlSelect, string.Join(",", this.select), " ");

            return sqlSelect;
        }

        public virtual string SqlWhere()
        {
            var sqlWhere = string.Empty;

            this.BuildWheres("AND");

            if (this.whereString.IsPresent())
            {
                sqlWhere = $"WHERE {this.whereString} ";
            }

            return sqlWhere;
        }

        public virtual string SqlWhereCritere()
        {
            var sqlWhere = string.Empty;

            this.BuildWheresCritere("AND");

            if (this.whereCritereString.IsPresent())
            {
                sqlWhere = $"WHERE {this.whereCritereString} ";
            }

            return sqlWhere;
        }

        public virtual string SqlOrderBy()
        {
            var sqlTris = string.Empty;
            if (this.tris.Any())
            {
                sqlTris = " ORDER BY ";
                sqlTris += string.Join(", ", this.tris);
            }

            return sqlTris;
        }

        public virtual string SqlPage()
        {
            if (this.page.IsPresent())
                return this.page;

            return string.Empty;
        }
    }
}
