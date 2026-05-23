using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Zzz.App.Core.AccesDonnees.Sql;
using Zzz.App.Core.AccesDonnees.Sql.Dto;
using Zzz.App.Core.Entites.Grille;
using Zzz.App.Core.Langue;

namespace Zzz.App.Core.AccesDonnees.Sqlite.Sql
{
	public class SqliteDbObjectProvider : IDbObjectProvider
	{
		public ILangueService LangueService { get; set; }
        public IReaderService ReaderService { get; }

        public SqliteDbObjectProvider(ILangueService langueService, IReaderService readerService)
		{
			this.LangueService = langueService;
            this.ReaderService = readerService;
        }

		public virtual IDbDataParameter Clone(IDbDataParameter parameter)
		{
			var param = this.GetParametre(parameter.Value, parameter.ParameterName);
			param.DbType = parameter.DbType;
			param.Direction = parameter.Direction;
			param.Size = parameter.Size;
			param.Precision = parameter.Precision;
			param.Scale = parameter.Scale;

			return param;
		}

		public virtual IDbCommand CreateCommand(IDbConnection connection)
		{
			var command = (SqliteCommand)connection.CreateCommand();			
			return command;
		}

		public virtual void SetParameterDbType(IDbDataParameter parameter, Type type)
		{
			((SqliteParameter)parameter).SqliteType = this.GetDbType(type);
		}

		public virtual void SetBulkCommand(IDbCommand command, IEnumerable<IDbDataParameter> parametres)
		{
			
		}

		protected virtual SqliteType GetDbType(Type type)
		{
			type = Nullable.GetUnderlyingType(type) ?? type;

			if (type.Equals(typeof(string))) return SqliteType.Text;
			if (type.Equals(typeof(DateTime))) return SqliteType.Text;
			if (type.Equals(typeof(Int64))) return SqliteType.Integer;
			if (type.Equals(typeof(Int32))) return SqliteType.Integer;
			if (type.Equals(typeof(Int16))) return SqliteType.Integer;
			if (type.Equals(typeof(sbyte))) return SqliteType.Integer;
			if (type.Equals(typeof(byte))) return SqliteType.Integer;
			if (type.Equals(typeof(decimal))) return SqliteType.Text;
			if (type.Equals(typeof(float))) return SqliteType.Real;
			if (type.Equals(typeof(double))) return SqliteType.Real;
			if (type.Equals(typeof(byte[]))) return SqliteType.Blob;
			if (type.Equals(typeof(bool))) return SqliteType.Integer;

			return SqliteType.Text;
		}
		

		public virtual IList<IDbDataParameter> GetParametres<T>(T o)
		{
			var props = o.GetType().GetProperties();

			var parametres = new List<IDbDataParameter>();
			foreach (var prop in props)
			{
				var val = prop.GetValue(o);
				if (val == null) continue;
				parametres.Add(this.GetParametre(val, prop.Name));
			}

			return parametres;
		}

		public virtual IDbDataParameter GetParametre<T>(T o, string nomParametre)
		{
			var type = o == null ? typeof(T) : o.GetType();
			return this.GetParametre(o, nomParametre, type);
		}


		public virtual IDbDataParameter GetParametre<T>(T val, string nomParametre, ParameterDirection direction)
		{
			var parametre = this.GetParametre(val, nomParametre);
			parametre.Direction = direction;
			return parametre;
		}

		public virtual IDbDataParameter GetParametre<T>(T val, string nomParametre, ParameterDirection direction, int size)
		{
			var parametre = this.GetParametre(val, nomParametre, direction);
			parametre.Size = size;
			return parametre;
		}

		public virtual IDbDataParameter GetParametre<T>(T val, string nomParametre, ParameterDirection direction, int size, byte precision, byte scale)
		{
			var parametre = this.GetParametre(val, nomParametre, direction, size);
			parametre.Precision = precision;
			parametre.Scale = scale;
			return parametre;
		}
		public virtual IDbDataParameter GetParametre()
		{
			return new SqliteParameter();
		}

		public virtual IDbDataParameter GetParametre(object val, string nomParametre, Type type)
		{
			var param = this.GetParametre();
			param.ParameterName = nomParametre;
			this.SetParameterDbType(param, type);
			if (val == null)
				param.Value = DBNull.Value;
			else
			{
				type = Nullable.GetUnderlyingType(type) ?? type;
				param.Value = Convert.ChangeType(val, type);
			}

			return param;
		}

		public virtual IDbDataParameter GetParametre(object val, string nomParametre, int dbType)
		{
			var param = this.GetParametre();
			param.ParameterName = nomParametre;
			((SqliteParameter)param).SqliteType = (SqliteType)dbType;
			param.Value = val ?? DBNull.Value;

			return param;
		}

		public virtual void SetNextvalCommand(IDbCommand command, string nomSequence, int nombre)
		{
			throw new NotImplementedException();
		}

		public virtual IList<IDbDataParameter> GetParametresCriteres<T>(CriteresDto criteres)
		{
			return this.GetParametresCriteres(criteres, typeof(T).GetProperties());
		}

		public virtual IList<IDbDataParameter> GetParametresCriteres(CriteresDto criteres, IEnumerable<ProprieteChampBd> proprieteChampBds)
		{
			if (proprieteChampBds == null || !proprieteChampBds.Any()) throw new ArgumentException("[DbObjectProvider] GetParametresCriteres : proprieteChampBds est obbligatoire");

			return this.GetParametresCriteres(criteres, proprieteChampBds.Select(p => p.Propriete));
		}

		public virtual IList<IDbDataParameter> GetParametresCriteres(CriteresDto criteres, IEnumerable<PropertyInfo> proprietes)
		{
			var parametres = new List<IDbDataParameter>();

			if (criteres.Filtres != null)
			{
				foreach (var filtre in criteres.Filtres)
				{
					var prop = proprietes.FirstOrDefault(p => p.Name.Equals(filtre.Colonne, StringComparison.OrdinalIgnoreCase));
					if (prop == null) continue;

					if (filtre.Operateur != ConstantesNoyau.AccesDonnees.Criteres.Operateurs.Entre &&
							filtre.Operateur != ConstantesNoyau.AccesDonnees.Criteres.Operateurs.PasEntre)
					{
						if (filtre.Operateur == ConstantesNoyau.AccesDonnees.Criteres.Operateurs.Contient ||
							filtre.Operateur == ConstantesNoyau.AccesDonnees.Criteres.Operateurs.NeContientPas ||
							filtre.Operateur == ConstantesNoyau.AccesDonnees.Criteres.Operateurs.DebutePar ||
							filtre.Operateur == ConstantesNoyau.AccesDonnees.Criteres.Operateurs.TerminePar)
						{
							parametres.Add(this.GetParametre(filtre.Valeur, prop.Name, typeof(string)));
							continue;
						}

						parametres.Add(this.GetParametre(filtre.Valeur, prop.Name, prop.PropertyType));
					}
					else
					{
						// Operateur Entre et PasEntre doivent passer les valeurs séparées d'un point virgule
						var parts = filtre.Valeur.Split(';');
						if (parts.Length > 1)
						{
							var min = parts[0];
							var max = parts[1];
							parametres.Add(this.GetParametre(min, filtre.Colonne + "min", prop.PropertyType));
							parametres.Add(this.GetParametre(max, filtre.Colonne + "max", prop.PropertyType));
						}
					}
				}

				if (!string.IsNullOrEmpty(criteres.Recherche))
				{
					var arrTermes = criteres.Recherche.Split(' ');
					for (int indexTermes = 0; indexTermes < arrTermes.Length; indexTermes++)
					{
						parametres.Add(this.GetParametre(arrTermes[indexTermes], $"{ConstantesNoyau.AccesDonnees.Criteres.PrefixParamWC}{indexTermes + 1}", typeof(string)));
					}
				}
			}

			return parametres;
		}

	}
}
