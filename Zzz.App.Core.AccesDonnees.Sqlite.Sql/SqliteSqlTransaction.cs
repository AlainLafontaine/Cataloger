using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Zzz.App.Core.AccesDonnees.Sqlite.Sql
{
    public class SqliteSqlTransaction : Transaction
    {
        protected SqliteTransaction SqliteTransaction
        {
            get
            {
                return (SqliteTransaction)this.transaction;
            }
        }
        public SqliteSqlTransaction()
        {
        }

        public SqliteSqlTransaction(SqliteTransaction transaction) 
            : base(transaction)
        {
        }

        public override void Save(string savePoint)
        {
            if (this.SqliteTransaction == null) return;
            
            this.SqliteTransaction.Save(savePoint);            
        }

        public override void Rollback(string savePoint)
        {

            if (this.SqliteTransaction == null) return;

            this.SqliteTransaction.Rollback(savePoint);
        }
    }
}
