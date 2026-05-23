using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using Zzz.App.Core.AccesDonnees.Sql;
using Zzz.App.Core.Configuration;
using Zzz.App.Core.Donnees;
using Zzz.App.Core.Logging;
using Zzz.App.Core.Securite;

namespace Zzz.App.Core.AccesDonnees.Sqlite.Sql
{
    public class ConnexionSqlite : ConnexionBase
    {
        //public event ActivationRolesHandler OnAvantActivationRoles;
        //public event ActivationRolesHandler OnApresActivationRoles;
      
        /// <summary>
        /// Constructeur
        /// </summary>        
        public ConnexionSqlite(
            IDbConnection connexion
        ) : base(connexion)
        {
        }

        /// <summary>
        /// Constructeur
        /// </summary>        
        public ConnexionSqlite(
            ILogger logger, 
            IConfigurationSecuriteApp configSecuriteApp, 
            IGestionnaireSecurite gs
        ) : base(logger, configSecuriteApp, gs)
        {

        }

        /// <summary>
        /// Crée l'instance de la connexion
        /// </summary>
        protected override void CreerInstanceConnexion()
        {
            if (this.connexion == null)
            {
                this.connexion = new SqliteConnection(this.connectionString);
            }
        }

        protected override void OnOpen()
        {
            this.ActiverRoles();
        }

        /// <summary>
        /// Débute une transaction BD
        /// </summary>
        /// <returns></returns>
        public override ITransaction BeginTransaction()
        {
            if (transaction == null || transaction.TransactionIsNull)
            {
                transaction = new SqliteSqlTransaction((SqliteTransaction)this.GetConnexion().BeginTransaction());
                return transaction;
            }

            return new SqliteSqlTransaction();
        }

        /// <summary>
        /// Fonction permettant d'activer les rôles à partir des claims de l'utilisateur connecté
        /// </summary>
        protected virtual void ActiverRoles()
        {
            
        }

        /// <summary>
        /// Retourne la chaine d'activation des roles idz
        /// </summary>
        /// <returns></returns>
        protected virtual string GetActivationRolesIdz()
        {
            return string.Empty;
        }

        /// <summary>
        /// Retour sysdate oracle
        /// </summary>
        /// <returns></returns>
        public override DateTime Sysdate()
        {
            using (var cmd = this.connexion.CreateCommand())
            {
                cmd.CommandText = "SELECT TO_CHAR (SYSDATE, 'MM-DD-YYYY HH24:MI:SS') FROM DUAL"; //TODO : A corriger
                cmd.CommandType = System.Data.CommandType.Text;

                var rep = cmd.ExecuteScalar();

                var now = DateTime.Now;
                if (rep != null)
                {
                    DateTime.TryParse(rep.ToString(), out now);
                }

                return now;
            }
        }

        /// <summary>
        /// Retourne les roles de la session bd en cours
        /// </summary>
        /// <returns></returns>
        protected override List<string> GetRoles()
        {
            throw new NotImplementedException();
        }
    }
}
